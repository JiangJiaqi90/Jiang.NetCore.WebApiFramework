
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class Sys_OperateLog: BaseEntity<Guid>
    {
        /// <summary>
        /// 操作员ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户IP
        /// </summary>
        [StringLength(50)]
        public string ClientIp{get;set;}
        /// <summary>
        /// 服务IP
        /// </summary>
        [StringLength(50)]
        public string ServerIp{get;set;}
        
        /// <summary>
        /// 请求方式
        /// </summary>
        [StringLength(20)]
        public string RequestType{get;set;}
        /// <summary>
        /// 请求地址
        /// </summary>
        [StringLength(500)]
        public string Url{get;set;}
        /// <summary>
        /// 控制器名称
        /// </summary>
        [StringLength(50)]
        public string ControllerName{get;set;}
        /// <summary>
        /// 方法名
        /// </summary>
        [StringLength(50)]
        public string ActionName{get;set;}
        /// <summary>
        /// 响应code
        /// </summary>
        public ResultCode ResponseCode {get;set;}
        /// <summary>
        /// 操作说明
        /// </summary>
        [StringLength(255)]
        public string ActionMemo { get; set; }
        /// <summary>
        /// 响应字符串
        /// </summary>
        [StringLength(255)]
        public string ResponseMessage { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        [StringLength(1000)]
        public string Data { get; set; }
        
    }
}
