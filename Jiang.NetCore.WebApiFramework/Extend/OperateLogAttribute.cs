using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 操作日志特性：api返回结果必须为ActionResult<OperateResult>;
    /// </summary>
    public class OperateLogAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        private readonly string _action;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="actionName">操作名称</param>
        public OperateLogAttribute(string actionName)
        {
            _action = actionName;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                base.OnActionExecuted(context);
                var str = context.ToString();
                
                //请求IP
                var ip = context.HttpContext.Request.Host.Host;
                var result = context.Result;
                var log = new Sys_OperateLog()
                {
                    ServerIp = ip,
                    Id = Guid.NewGuid(),
                    Url = $"{context.HttpContext.Request.Scheme}://{ip}{context.HttpContext.Request.Path.Value}",
                    ModifyTime = DateTime.Now,
                    ActionMemo=_action
                    
                };
                if (typeof(Controller).IsAssignableFrom(context.Controller.GetType()))
                {
                    var controller = (Controller)context.Controller;
                    log.ControllerName = controller.ControllerContext.ActionDescriptor.ControllerName;
                    log.RequestType = controller.ControllerContext.ActionDescriptor.ActionName;
                }
                var resultType = result.GetType();
                object operateResult = resultType.GetProperty("Value").GetValue(result, null);
                //操作响应结果
                if (operateResult != null)
                {
                    var otype = operateResult.GetType();
                    object code = otype.GetProperty("Code").GetValue(operateResult, null);
                    if (code != null)
                    {
                        log.ResponseCode = (ResultCode)code;
                    }
                    object message = otype.GetProperty("Message").GetValue(operateResult, null);
                    if (message != null)
                    {
                        log.ResponseMessage = message.ToString();
                        if (log.ResponseMessage.Length > 255)
                        {
                            log.ResponseMessage = log.ResponseMessage.Substring(0, 240);
                        }
                    }
                    object content = otype.GetProperty("Content").GetValue(operateResult, null);
                    if (content != null)
                    {
                        log.Data = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                        if (log.Data.Length > 1000)
                        {
                            log.Data = log.Data.Substring(0, 990);
                        }
                    }
                    //用户ID
                    var userIdstr = context.HttpContext.User.Identity.Name;
                    Guid userid;
                    if(Guid.TryParse(userIdstr,out userid))
                    {
                        log.UserId = userid;
                    }
                    //写入数据库
                    //LogManager.WriteLog(log);
                }
            }
            catch(Exception ex)
            {
                NLogHelp.ErrorLog(ex);
            }
            
        }
        
    }
}
