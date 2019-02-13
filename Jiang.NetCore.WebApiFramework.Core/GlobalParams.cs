using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 全局参数
    /// </summary>
    public static class GlobalParams
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }
        /// <summary>
        /// 系统附件根目录
        /// </summary>
        public static string AttachmentBasePath { get; set; }
        /// <summary>
        /// 系统附件下载地址前缀
        /// </summary>
        public static string SysAttachmentFileUrl { get; set; }
        
    }
}
