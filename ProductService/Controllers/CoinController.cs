using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoinController : ControllerBase
    {

        public static readonly ImmutableArray<Coin> Summaries = ImmutableArray.Create(new Coin[]
        {
            new Coin("https://goerli.etherscan.io/address/", "TESTHCN", "Test HCN", 50.00000000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "GTHCN", "GTESTHCN", 0.00000000.ToString()),
            new Coin("https://blockstream.info/testnet/tx/", "BTC", "Bitcoin", 0.00040000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "GTETH", "Goerli Test Ethereum", 0.00011000.ToString()),
            new Coin("https://mempool.space/testnet/tx/", "TBTC", "Test Bitcoin", 0.00040000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "TETH", "Test Ethereum", 0.00500000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "TESTHCN1", "Test HCN1", 50.00000000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "HCN", "Himalaya Coin", 0.10000000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "USD", "US Dollar", 100.00000000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "HDO", "Himalaya Dollar", 0.10000000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "FCOIN", "FCoin", 100.00000000.ToString()),
            new Coin("https://goerli.etherscan.io/address/", "ETH", "Ethereum", 0.00500000.ToString()),
        });
        
        
        [HttpGet]
        public ActionResult<Coin[]> Get()
        {
            return Summaries.ToArray();
        }
    }
}
