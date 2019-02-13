using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 角色绑定功能项参数
    /// </summary>
    public class RoleBindFeaturesParam
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Required]
        public Guid RoleId { get; set; }
        /// <summary>
        /// 功能项ID列表
        /// </summary>
        [Required]
        public List<Guid> FeatureIds { get; set; }
    }
}
