using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 登录账户参数
    /// </summary>
    public class LoginUserModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public Auth_User User { get; set; }
        /// <summary>
        /// 用户所属的角色
        /// </summary>
        public List<Auth_Role> Roles {get;set;}
        /// <summary>
        /// 登录用户的权限项
        /// </summary>
        public List<Auth_Auth> Auths { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
    }
}
