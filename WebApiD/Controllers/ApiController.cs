using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiD.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    public class ApiController : ControllerBase
    {
        private volatile static int _count = 0;

        [HttpGet]
        public string Count()
        {
            int delay = new Random().Next(100, 3000);
            //Thread.Sleep(TimeSpan.FromMilliseconds(delay));
            return $"D服务 被访问 {++_count} 次, 耗时 {delay} 毫秒";
        }

        [HttpGet]
        [Route("api")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {
                $"SignKey："
            };

            return new string[] {
                $"D服务：{Guid.NewGuid().ToString("N").ToUpper()}",
                $"D服务：{Guid.NewGuid().ToString("N").ToUpper()}"
            };
        }
    }
}
