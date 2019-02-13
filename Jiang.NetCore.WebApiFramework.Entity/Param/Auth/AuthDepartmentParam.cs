
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 部门管理参数
    /// </summary>
    public class AuthDepartmentParam
    {
        /// <summary>
        /// 主键（新增不传，修改必须传）
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Sort { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark { get; set; }
        /// <summary>
        /// 转实体
        /// </summary>
        /// <returns></returns>
        public Auth_Department ToEntity()
        {
            return new Auth_Department()
            {
                Id = Id,
                Code = Code,
                Name = Name,
                Sort = Sort,
                Remark = Remark
            };
        }
    }
}
