
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 职位参数
    /// </summary>
    public class AuthJobParam
    {
        /// <summary>
        /// 主键（修改必传）
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        [Required]
        public Guid DepartmentId { get; set; }
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
        public Auth_Job ToEntity()
        {
            return new Auth_Job()
            {
                Id = Id,
                Code = Code,
                DepartmentId = DepartmentId,
                Name = Name,
                Sort = Sort,
                Remark = Remark
            };
        }
    }
}
