using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 实时通信操作
    /// </summary>
    public interface ISignalrAction
    {
        /// <summary>
        /// 实时同步房态操作
        /// </summary>
        /// <param name="obj">实时房态数据</param>
        void UpdateRoomState(object obj);
        /// <summary>
        /// 实时更新消息
        /// </summary>
        /// <param name="obj">消息数据</param>
        void UpdateMessage(object obj);
        /// <summary>
        /// 夜审推送
        /// </summary>
        /// <param name="obj">消息数据</param>
        void UpdateNANightAudit(object obj);
    }
}
