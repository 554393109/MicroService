using Microsoft.AspNetCore.Mvc;

namespace WebApiC.Controllers
{
    /// <summary>
    /// 用于服务存活检测
    /// </summary>
    [Produces("application/json")]
    [Route("Health")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("ok");
            return BadRequest("YZQ Error");
        }
    }
}
