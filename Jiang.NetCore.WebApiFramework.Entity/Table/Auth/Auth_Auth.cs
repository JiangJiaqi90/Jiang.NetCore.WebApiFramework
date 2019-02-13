
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Auth_Auth : BaseEntity<Guid>
    {
        /// <summary>
        /// 功能项Id
        /// </summary>
        [Required]
        public Guid FeatureId {get;set;}
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code      {get;set;}
        /// <summary>
        /// 权限名称
        /// </summary>
        [StringLength(50)]
        public string Name      {get;set;}
        /// <summary>
        /// URL
        /// </summary>
        [StringLength(255)]
        public string Url       {get;set;}
        /// <summary>
        /// 按钮ID
        /// </summary>
        [StringLength(50)]
        public string ButtonId  {get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark { get; set; }

    }
}
