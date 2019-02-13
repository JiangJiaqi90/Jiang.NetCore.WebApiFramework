
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 职位
    /// </summary>
    public class Auth_Job : BaseEntity<Guid>
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        [Required]
        public Guid DepartmentId {get;set;}
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code         {get;set;}
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name         {get;set;}
        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Sort         {get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark { get; set; }

    }
    /// <summary>
    /// 职位模型
    /// </summary>
    public class VAuthJob : Auth_Job
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
    }
}
