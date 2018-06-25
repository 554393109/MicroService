using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    /// <summary>
    /// 用于服务存活检测
    /// </summary>
    [Produces("application/json")]
    [Route("api/health")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok("ok");
    }
}
