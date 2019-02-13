using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 功能项绑定权限参数
    /// </summary>
    public class FeatureAuthParam
    {
        /// <summary>
        /// 功能项Id
        /// </summary>
        [Required]
        public Guid FeatureId { get; set; }
        /// <summary>
        /// 权限列表
        /// </summary>
        public List<AuthParam> Auths { get; set; }
    }
    
}
