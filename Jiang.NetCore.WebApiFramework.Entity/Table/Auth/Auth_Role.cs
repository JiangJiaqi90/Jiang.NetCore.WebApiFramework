
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Auth_Role : BaseEntity<Guid>
    {
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code { get; set; }
        /// <summary>
        /// 角色名称
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
    /// <summary>
    /// 角色页面模型
    /// </summary>
    public class VAuthRole: Auth_Role
    {
        /// <summary>
        /// 是否被选择
        /// </summary>
        public bool Selected { get; set; } = false;
        /// <summary>
        /// 默认五参数构造器
        /// </summary>
        public VAuthRole()
        {

        }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="role"></param>
        public VAuthRole(Auth_Role role)
        {
            Id = role.Id;
            Code = role.Code;
            Name = role.Name;
            Sort = role.Sort;
            Remark = role.Remark;
            CreateTime = role.CreateTime;
            ModifyTime = role.ModifyTime;
        }
    }
}
