/************************************************************************
 * 文件标识：  3fcc9651-b38e-49d4-9cc0-b9610835ef81
 * 项目名称：  Utility
 * 项目描述：  
 * 类 名 称：  AppBuilderExtensions
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/6/24 16:06:59
 * 更新时间：  2018/6/24 16:06:59
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Utility
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, ServiceEntity serviceEntity)
        {
            //请求注册的 Consul 地址
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceEntity.ConsulIP}:{serviceEntity.ConsulPort}"));

            var httpCheck = new AgentServiceCheck() {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),                      //服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),                                            //健康检查时间间隔，或者称为心跳间隔
                HTTP = $"http://{serviceEntity.IP}:{serviceEntity.Port}/api/health",            //健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration() {
                Checks = new[] { httpCheck },
                ID = Guid.Empty.ToString(),     // Guid.NewGuid().ToString(),
                Name = serviceEntity.ServiceName,
                Address = serviceEntity.IP,
                Port = serviceEntity.Port,
                //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别 
                Tags = new[] { $"urlprefix-/{serviceEntity.ServiceName}" }
            };

            //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            consulClient.Agent.ServiceRegister(registration).Wait();
            lifetime.ApplicationStopping.Register(() => {
                //服务停止时取消注册 
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}
