using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OperateResult<T> where T : class
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public ResultCode Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 响应内容
        /// </summary>
        public T Content { get; set; }
        public OperateResult()
        {

        }
        public OperateResult(T t)
        {
            Content = t;
            Code = ResultCode.OK;
        }

        public OperateResult(ResultCode code,string message,T content)
        {
            this.Code = code;
            this.Message = message;
            this.Content = content;
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static OperateResult<T> Ok(string message="成功",T content = default(T))
        {
            return new OperateResult<T>(ResultCode.OK, message, content);
        }
        /// <summary>
        /// 操作失败或报错
        /// </summary>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static OperateResult<T> Error(string message="失败",T content = null)
        {
            return new OperateResult<T>(ResultCode.Error, message, content);
        }
    }
}
