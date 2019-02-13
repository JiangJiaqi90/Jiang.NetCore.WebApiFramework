
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 角色参数
    /// </summary>
    public class AuthRoleParam
    {
        /// <summary>
        /// Id:修改时必传
        /// </summary>
        public Guid Id { get; set; }
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
        public Auth_Role ToEntity()
        {
            return new Auth_Role()
            {
                Id=Id,
                Code=Code,
                Name=Name,
                Remark=Remark,
                Sort=Sort
            };
        }
    }
}
