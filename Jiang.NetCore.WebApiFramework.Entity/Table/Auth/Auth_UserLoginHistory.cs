
using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 用户登录历史
    /// </summary>
    public class Auth_UserLoginHistory : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public LoginState State { get; set; }
        /// <summary>
        /// 注销时间
        /// </summary>
        public DateTime LogoutTime { get; set; }
    }
    /// <summary>
    /// 登录状态
    /// </summary>
    public enum LoginState
    {
        /// <summary>
        /// 登录
        /// </summary>
        Login = 0,
        /// <summary>
        /// 注销
        /// </summary>
        Logout=1
    }
}
