using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace OcelotGetway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((hostingContext, config) => {
                config
                    .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddJsonFile("configuration.json")
                    .AddEnvironmentVariables();
            })
            .ConfigureServices(service => {
                service.AddOcelot();
            })
            .ConfigureLogging((hostingContext, logging) => {
                //add your logging
            })
            .UseIISIntegration()
            .Configure(app => {
                app.UseOcelot().Wait();
            })
            .Build();


        //public static IWebHost BuildWebHost(string[] args)
        //{
        //    IWebHostBuilder builder = new WebHostBuilder();

        //    return builder
        //        .ConfigureServices(service => {
        //            service.AddSingleton(builder);
        //        })
        //        .ConfigureAppConfiguration(conbuilder => {
        //            conbuilder.AddJsonFile("configuration.json");
        //        })
        //        .UseKestrel()
        //        .UseUrls("http://*:5000")
        //        .UseStartup<Startup>()
        //        .Build();
        //}
    }
}
