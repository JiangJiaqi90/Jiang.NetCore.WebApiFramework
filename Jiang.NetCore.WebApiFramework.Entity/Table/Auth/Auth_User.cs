
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class Auth_User : BaseEntity<Guid>
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 职位ID
        /// </summary>
        [Required]
        public Guid JobId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(20)]
        public string RealName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        public SexEnum Sex { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(20)]
        public string IdCard { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(100)]
        public string Email { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        [StringLength(20)]
        public string Telephone { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(11)]
        public string Phone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(100)]
        public string Address { get; set; }
        /// <summary>
        /// 员工类别
        /// </summary>
        [Required]
        public UserType Type { get; set; }
        /// <summary>
        /// 是否冻结
        /// </summary>
        [Required]
        public bool IsFreeze { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark { get; set; }
    }
    /// <summary>
    /// 员工模型
    /// </summary>
    public class VUser : Auth_User
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string JobName { get; set; }
    }

    /// <summary>
    /// 员工模型
    /// </summary>
    public class VUsers
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        public Guid JobId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexEnum Sex { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 座机
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 员工类别
        /// </summary>
        public UserType Type { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string JobName { get; set; }
    }
    /// <summary>
    /// 员工类型
    /// </summary>
    [Description("员工类型")]
    public enum UserType
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 销售员
        /// </summary>
        [Description("销售员")]
        Salesman = 1
    }
    /// <summary>
    /// 性别
    /// </summary>
    public enum SexEnum
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unkown = 0,
        /// <summary>
        /// 男人
        /// </summary>
        Man = 1,
        /// <summary>
        /// 女人
        /// </summary>
        Woman = 2
    }
}
