using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 用户模型参数
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
    /// <summary>
    /// 会员登录模型
    /// </summary>
    public class MemeberLoginModel
    {
        /// <summary>
        /// 手机号/会员号
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
