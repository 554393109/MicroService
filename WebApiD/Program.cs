using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApiD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .ConfigureAppConfiguration((context, builder) => {
                var env = context.HostingEnvironment;

                // 此处配置后覆盖前
                builder
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("/Conf/config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"/Conf/config.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            });
    }
}
