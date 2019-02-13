
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 菜单功能项参数
    /// </summary>
    public class AuthFeatureParam
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [Required]
        public Guid MenuId { get; set; }
        /// <summary>
        /// 功能项列表
        /// </summary>
        [Required]
        public List<FeatureParam> Features { get; set; }
    }
    /// <summary>
    /// 功能项参数
    /// </summary>
    public class FeatureParam
    {
        /// <summary>
        /// 功能项Id (修改必传)
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code { get; set; }
        /// <summary>
        /// 功能项名称
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
        public Auth_Feature ToEntity()
        {
            return new Auth_Feature()
            {
                Id = Id,
                Code = Code,
                Name = Name,
                Remark = Remark,
                Sort = Sort
            };
        }
    }
}
