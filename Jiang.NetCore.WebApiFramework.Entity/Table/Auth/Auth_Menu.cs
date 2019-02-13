
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 菜单表
    /// </summary>
    public class Auth_Menu : BaseEntity<Guid>
    {
        /// <summary>
        /// 父级菜单
        /// </summary>
        [Required]
        public Guid ParentId {get;set;}
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code     {get;set;}
        /// <summary>
        /// 菜单名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name     {get;set;}
        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Sort     {get;set;}
        /// <summary>
        /// url
        /// </summary>
        [StringLength(255)]
        public string Url      {get;set;}
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark   {get;set;}
        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(100)]
        public string MenuIcon { get; set; }

    }
}
