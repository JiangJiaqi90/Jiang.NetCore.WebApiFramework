using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jiang.NetCore.WebApiFramework
{
    public static class ApplicationExtentions
    {
        /// <summary>
        /// 配置swagger帮助页
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerInfo(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
                c.ShowExtensions();
                c.EnableValidator();

                c.EnableFilter();
                //c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);
            });
        }
    }
}
