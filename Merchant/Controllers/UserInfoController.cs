using System;
using System.Net.Http;
using System.Threading.Tasks;
using Merchant.Hubs;
using IdentityModel.Client;
using Merchant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Merchant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly IHubContext<UsersHub> _hubContext;
        private readonly AuthOptions _options;
        private readonly ILogger<AuthController> _logger;

        public UserInfoController(ILogger<AuthController> logger,
            IHubContext<UsersHub> usersHub,
            IOptions<AuthOptions> options)
        {
            _hubContext = usersHub;
            _options = options.Value;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetByUserName(string userName)
        {
            _logger.LogInformation("Username " + userName);

            using (var client = new HttpClient())
            {
                var user = new UserInfo()
                {
                    Id = "1",
                    Avatar=  "",
                    Email = userName,
                    FirstName = "Kursat",
                    Job = "Developer",
                    LastName = "Arslan",
                    Progress=  75,
                    Role =  "Admin"
                };
                try
                {

                    return Ok(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}