
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 权限参数
    /// </summary>
    public class AuthParam
    {
        /// <summary>
        /// 权限Id:修改时必传
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(50)]
        public string Code { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        [StringLength(255)]
        public string Url { get; set; }
        /// <summary>
        /// 按钮ID
        /// </summary>
        [StringLength(50)]
        public string ButtonId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark { get; set; }
        /// <summary>
        /// 转实体
        /// </summary>
        /// <returns></returns>
        public Auth_Auth ToEntity()
        {
            return new Auth_Auth()
            {
                Id = Id,
                ButtonId = ButtonId,
                Code = Code,
                Name = Name,
                Remark = Remark,
                Url = Url
            };
        }
    }
}
