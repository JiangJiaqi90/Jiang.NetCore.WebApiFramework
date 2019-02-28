using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public interface ISysOperateLogService
    {
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        OperateResult<Sys_OperateLog> Add(Sys_OperateLog log);
    }
}
