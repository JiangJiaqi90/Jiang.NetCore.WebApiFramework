
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 部门表
    /// </summary>
    public class Auth_Department : BaseEntity<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name   {get;set;}
        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Sort   {get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark {get;set;}

    }
}
