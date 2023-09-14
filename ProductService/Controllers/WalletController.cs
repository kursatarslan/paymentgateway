using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "This is OrderController on product api!";
        }
    }
}
