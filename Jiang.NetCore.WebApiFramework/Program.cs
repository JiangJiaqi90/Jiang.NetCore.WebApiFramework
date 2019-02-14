using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Jiang.NetCore.WebApiFramework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            /*
             * EnsureCreated 确保存在上下文数据库。 如果存在，则不需要任何操作。 如果不存在，则会创建数据库及其所有架构。 EnsureCreated 不使用迁移创建数据库。 使用 EnsureCreated 创建的数据库稍后无法使用迁移更新。
                启动应用时会调用 EnsureCreated，以进行以下工作流：
                删除数据库。
                更改数据库架构（例如添加一个 EmailAddress 字段）。
                运行应用。
                EnsureCreated 创建一个带有 EmailAddress 列的数据库。
                架构快速演变时，在开发初期使用 EnsureCreated 很方便。 本教程后面将删除 DB 并使用迁移。
             * 
             */
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ManageContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel().UseUrls("http://*:5020")
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()// NLog: setup NLog for Dependency injection
                .Build();
    }
}
