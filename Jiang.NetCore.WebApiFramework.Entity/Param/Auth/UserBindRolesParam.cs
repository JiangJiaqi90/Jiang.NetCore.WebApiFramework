using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 用户绑定角色参数
    /// </summary>
    public class UserBindRolesParam
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        /// <summary>
        /// 角色ID列表
        /// </summary>
        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
