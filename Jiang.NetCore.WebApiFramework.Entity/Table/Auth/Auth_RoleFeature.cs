
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 角色功能项关联
    /// </summary>
    public class Auth_RoleFeature : BaseEntity<Guid>
    {
        /// <summary>
        /// 功能项ID
        /// </summary>
        [Required]
        public Guid FeatureId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public Guid RoleId { get; set; }
    }
}
