using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 业务模型注册
    /// </summary>
    public class ServiceModule : Module
    {        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.Name.EndsWith("Service")
                )
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
