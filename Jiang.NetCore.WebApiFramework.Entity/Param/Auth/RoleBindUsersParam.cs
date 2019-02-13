using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 角色编订用户参数
    /// </summary>
    public class RoleBindUsersParam
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Required]
        public Guid RoleId { get; set; }
        /// <summary>
        /// 用户ID列表
        /// </summary>
        [Required]
        public List<Guid> UserIds { get; set; }
    }
}
