using Microsoft.AspNetCore.Mvc;

namespace WebApiD.Controllers
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
            return BadRequest("YZQ Error");
            return Ok("ok");
        }
    }
}
