using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]/[action]")]
    public class ApiController : Controller
    {
        private volatile static int _count = 0;

        [HttpGet]
        public string Count()
        {
            int delay = new Random().Next(100, 3000);
            //Thread.Sleep(TimeSpan.FromMilliseconds(delay));
            return $"A服务 被访问 {++_count} 次, 耗时 {delay} 毫秒";
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {
                $"A服务：{Guid.NewGuid().ToString("N").ToUpper()}",
                $"A服务：{Guid.NewGuid().ToString("N").ToUpper()}"
            };
        }
    }
}
