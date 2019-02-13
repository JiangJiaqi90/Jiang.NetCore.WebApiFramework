using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// 主键（新增是不需要添加）
        /// </summary>
        [DataMember]
        [Key]
        public T Id { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [ConcurrencyCheck]
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
