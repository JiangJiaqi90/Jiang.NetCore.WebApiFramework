using Jiang.NetCore.WebApiFramework;
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
        /// 缓存
        /// </summary>
        private CacheHelp _cacheHelp;
        /// <summary>
        /// 日志业务
        /// </summary>
        private ISysOperateLogService _logService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheHelp">缓存</param>
        /// <param name="logService">日志逻辑</param>
        public OperateLogAttribute(CacheHelp cacheHelp, ISysOperateLogService logService)
        {
            _cacheHelp = cacheHelp;
            _logService = logService;
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
                    RequestType = context.HttpContext.Request.Method
                };
                if (typeof(Controller).IsAssignableFrom(context.Controller.GetType()))
                {
                    var controller = (Controller)context.Controller;
                    log.ControllerName = controller.ControllerContext.ActionDescriptor.ControllerName;
                    log.ActionName = controller.ControllerContext.ActionDescriptor.ActionName;
                    //获取方法描述
                    var key = $"{log.ControllerName}_{log.ActionName}";
                    var dic = _cacheHelp.GetActionDictionary();
                    if (dic.ContainsKey(key))
                    {
                        log.ActionMemo = dic[key];
                    }
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
                    if (Guid.TryParse(userIdstr, out userid))
                    {
                        log.UserId = userid;
                    }
                    //写入数据库
                    _logService.Add(log);
                }
            }
            catch (Exception ex)
            {
                NLogHelp.ErrorLog(ex);
            }

        }

    }
}
