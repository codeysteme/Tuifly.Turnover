using Microsoft.AspNetCore.Mvc;

namespace TuiFly.Turnover.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
