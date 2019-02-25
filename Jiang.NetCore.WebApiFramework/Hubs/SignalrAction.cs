using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 实时通信操作
    /// </summary>
    public class SignalrAction : ISignalrAction
    {
        /// <summary>
        /// 实时推送集线器上下文
        /// </summary>
        private IHubContext<SignalrHubs> _hubContext;
        private IConfiguration _configuration { get; }
        public SignalrAction(IHubContext<SignalrHubs> hubContext, IConfiguration configuration)
        {
            _hubContext = hubContext;
            _configuration = configuration;
        }
        /// <summary>
        /// 实时更新房态
        /// </summary>
        /// <param name="obj">实时房态数据</param>
        public void UpdateRoomState(object obj)
        {
            //此处获取实时房态的json
            var value = "";
            if (obj != null)
            {
                value = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            var mothod = _configuration.GetSection("AppSetting:UpdateRoomStateMethod").Value;
            _hubContext.Clients.All.SendAsync(mothod, value);
        }
        /// <summary>
        /// 实时更新消息
        /// </summary>
        /// <param name="obj">消息数据</param>
        public void UpdateMessage(object obj)
        {
            var value = "";
            if (obj != null)
            {
                value = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            var mothod = _configuration.GetSection("AppSetting:UpdateMessageMethod").Value;
            _hubContext.Clients.All.SendAsync(mothod, value);
        }
        /// <summary>
        /// 夜审
        /// </summary>
        /// <param name="obj">消息数据</param>
        public void UpdateNANightAudit(object obj)
        {
            var value = "";
            if (obj != null)
            {
                value = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            var mothod = _configuration.GetSection("AppSetting:UpdateNANightAuditMethod").Value;
            _hubContext.Clients.All.SendAsync(mothod, value);
        }
    }
}
