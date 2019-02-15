using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jiang.NetCore.WebApiFramework
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("autofac.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            GlobalParams.ConnectionString = Configuration.GetConnectionString("MySqlConnection");
            //GlobalParams.AttachmentBasePath = Configuration.GetSection("AppSetting:AttachmentPath").Value;
            //GlobalParams.SysAttachmentFileUrl = Configuration.GetSection("AppSetting:SysAttachmentFileUrl").Value;
        }

        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<ParamCheckActionFilter>();
            });
            //Asp.Net Core中注册EF的上下文处理，在Startup文件中,注册服务--这行代码不能用于生产环境，会极大降低效率
            //services.AddDbContext<HotelManageContext>(options => {
            //    //使用ef core mysql 连接
            //    var loggerFactory = new LoggerFactory();
            //    loggerFactory.AddProvider(new EFLoggerProvider());

            //    options.UseMySql(Configuration.GetConnectionString("MySqlConnection"))
            //        .UseLoggerFactory(loggerFactory);
            //});
            services.AddDbContext<ManageContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));
            ////注入Hangfire服务
            //services.AddHangfire(config =>
            //{
            //    config.UseStorage(new MySqlStorage(Configuration.GetConnectionString("MySqlConnection")));//注意，这里使用的是mysql
            //});
            
            services.AddUnitOfWork<ManageContext>();//添加UnitOfWork支持
            services.AddSingleton<MyMemoryCache>();//添加缓存
            //添加jwt
            services.AddJwt(Configuration);
            //依赖注入
            //services.AddScoped(typeof(ISignalrAction), typeof(SignalrAction));
            services.AddScoped(typeof(IRoleService), typeof(RoleService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped<CacheHelp>();
            services.AddMvc(opt =>
            {
                //全局路由前缀
                opt.UseCentralRoutePrefix(new RouteAttribute(""));
                //opt.UseCentralRoutePrefix(new RouteAttribute("api/[controller]/[action]"));

            });//.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //配置signalR
            services.AddSignalR();

            //允许所有来源
            services.AddCors(options => options.AddPolicy("CorsPolicy",
           build =>
           {
               build.AllowAnyMethod().AllowAnyHeader()
                      .WithOrigins("*")//允许任何源
                      .AllowCredentials();
           }));

            // Register the Swagger services
            services.AddSwaggerHelp();

            services.AddRouting();
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var module = new ConfigurationModule(Configuration);
            builder.RegisterModule(module);
            //属性注入
            this.Container = builder.Build();



            return new AutofacServiceProvider(this.Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => builder.Run(async context => await ErrorEvent(context)));

            }
            /*
             *在调用 UseAuthentication 或类似的身份验证方案中间件之前，
             * 调用 Startup.Configure 中的 UseForwardedHeaders 方法。 
             * 配置中间件以转接 X-Forwarded-For 和 X-Forwarded-Proto 标头：
             */
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            //app.UseHttpsRedirection();注释掉重定向
            // 套用 Policy 到 Middleware()
            app.UseCors("CorsPolicy");//此代码是作用于全局,每个controller和action自动允许跨域

            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<SignalrHubs>("/signalrHubs");
            //});
            app.UseWebSockets();
            app.UseSwaggerInfo();
            app.UseAuthentication();
            //app.UseHangfireServer();//启动Hangfire服务
            //app.UseHangfireDashboard();//启动Hangfire面板

            app.UseMvc();

        }
        /// <summary>
        /// 生产环境异常处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Task ErrorEvent(HttpContext context)
        {
            var feature = context.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            NLogHelp.ErrorLog(error);
            return context.Response.WriteAsync("异常", Encoding.GetEncoding("GBK"));
        }
    }
}
