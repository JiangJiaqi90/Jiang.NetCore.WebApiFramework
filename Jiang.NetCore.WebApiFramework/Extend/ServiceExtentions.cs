using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 扩展服务注册
    /// </summary>
    public static class ServiceExtentions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration Configuration)
        {
            #region Configure Jwt Authentication
            //将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);
            //Use Jwt bearer authentication
            //TimeSpan expiration = TimeSpan.FromMinutes(Convert.ToDouble(expire));
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

            services.AddAuthorization(options =>
            {
                //1、Definition authorization policy
                options.AddPolicy("Permission",
                   policy => policy.Requirements.Add(new PolicyRequirement()));
            }).AddAuthentication(s =>
            {
                //2、Authentication
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(s =>
            {
                //3、Use Jwt bearer 
                s.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = key,
                    //ClockSkew = expiration,
                    ValidateLifetime = true
                };
                s.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("token", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["token"];
                        context.Token = token.FirstOrDefault();
                        return Task.CompletedTask;
                    }
                };
                
            });

            //DI handler process function
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();

            #endregion
        }
        /// <summary>
        /// 添加swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwaggerHelp(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1.0",
                    Title = "API doc",
                    Description = "",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "JiangJiaqi", Email = "610877596@qq.com", Url = "" }
                });
                //添加读取注释服务
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, Configuration.GetSection("AppSetting:webApiXmlFile").Value);
                var entityXmlPath = Path.Combine(basePath, "Jiang.NetCore.WebApiFramework.Entity.xml");
                var coreXmlPath = Path.Combine(basePath, "Jiang.NetCore.WebApiFramework.Core.xml");
                c.IncludeXmlComments(entityXmlPath);
                c.IncludeXmlComments(coreXmlPath);
                c.IncludeXmlComments(xmlPath, true);//添加控制器层注释（true表示显示控制器注释）
                c.SchemaFilter<AutoRestSchemaFilter>();//架构过滤器
                c.OperationFilter<AuthResponsesOperationFilter>();
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                
                //枚举
                //c.DescribeAllEnumsAsStrings();
                //c.DescribeStringEnumsInCamelCase();
                var security = new Dictionary<string, IEnumerable<string>> { { JwtBearerDefaults.AuthenticationScheme, new string[] { } }, };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"token:{token}\"",
                    Name = "token",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                // UseFullTypeNameInSchemaIds replacement for .NET Core
                //c.CustomSchemaIds(x => x.FullName);
            });
        }
    }
    // AutoRestSchemaFilter.cs
    public class AutoRestSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            if (context.SystemType.IsEnum)
            {
                schema.Extensions.Add(
                    "x-ms-enum",
                    new OpenApiObject
                    {
                        ["name"] = new OpenApiString(context.SystemType.Name),
                        ["modelAsString"] = new OpenApiBoolean(true)
                    }
                );
            };
        }

    }
    // AuthResponsesOperationFilter.cs
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            //if (authAttributes.Any())
            //    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        }
    }
   
}
