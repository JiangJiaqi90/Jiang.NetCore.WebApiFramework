
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 角色员工关联
    /// </summary>
    public class Auth_RoleUser : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public Guid UserId {get;set;}
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public Guid RoleId { get; set; }

    }
}
