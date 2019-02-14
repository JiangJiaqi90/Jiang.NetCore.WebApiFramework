
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 员工参数
    /// </summary>
    public class UserParam
    {
        /// <summary>
        /// Id 修改时必传
        /// </summary>
        public Guid Id { get; set; }
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
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark { get; set; }
        /// <summary>
        /// 员工类别
        /// </summary>
        [Required]
        public UserType Type { get; set; }
        /// <summary>
        /// 转实体
        /// </summary>
        /// <returns></returns>
        public Auth_User ToEntity()
        {
            return new Auth_User()
            {
                Id = Id,
                IdCard = IdCard,
                Address = Address,
                Code = Code,
                Email = Email,
                JobId = JobId,
                Password = Password.To32MD5(),
                Phone= Phone,
                RealName=RealName,
                Remark=Remark,
                Sex=Sex,
                Telephone= Telephone,
                UserName= UserName,
                Type = Type
            };
        }
    }
    /// <summary>
    /// 用户修改自己密码
    /// </summary>
    public class UserUpdatePassword
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
    /// <summary>
    /// 找回密码
    /// </summary>
    public class UserFindPassword
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [Required]
        public string Phone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
    /// <summary>
    /// 重置用户密码
    /// </summary>
    public class UserResetPassword
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
