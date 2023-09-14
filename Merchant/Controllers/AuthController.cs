using System;
using System.Net.Http;
using System.Threading.Tasks;
using Merchant.Hubs;
using IdentityModel.Client;
using Merchant.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Merchant.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IHubContext<UsersHub> _hubContext;
        private readonly AuthOptions _options;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger,
            IHubContext<UsersHub> usersHub,
            IOptions<AuthOptions> options)
        {
            _hubContext = usersHub;
            _options = options.Value;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthUser), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody]Credentials request)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                 httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(httpClientHandler))
                {
                   
                    var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                    {
                        Address = _options.Issuer,
                        Policy = { RequireHttps = false }
                    });
                    if (disco.IsError)
                    {
                        Console.WriteLine(disco.Error);
                        Console.ReadLine();
                        return Problem(disco.Error);
                    }

                    var tokenOptions = new TokenClientOptions
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = _options.ClientId,
                        ClientSecret = "secret"
                    };
                    try
                    {
                        var tokenClient = new TokenClient(client,tokenOptions);

                        var tokenResponse = await tokenClient.RequestPasswordTokenAsync(request.userName, request.password, _options.ScopeList);

                        if (tokenResponse.IsError)
                        {
                            Console.WriteLine(tokenResponse.Error);
                            return Problem(tokenResponse.Error);
                        }
                    
                        await _hubContext.Clients.All.SendAsync("UserLogin");
                        var authUser = new AuthUser("success", tokenResponse.AccessToken, request?.userName,tokenResponse.RefreshToken);
                        return Ok(authUser);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                
                }
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            await _hubContext.Clients.All.SendAsync("UserLogout");
            return Ok();
        }
    }
}