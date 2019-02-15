using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    public class MyMemoryCache
    {
        public MemoryCache Cache { get; set; }
        public MyMemoryCache()
        {
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                //SizeLimit = 512//设置缓存限制大小
            });
        }
        /// <summary>
        /// 角色缓存key
        /// </summary>
        public const string RoleCacheKey = "rolecachekey";
        /// <summary>
        /// 角色功能项关联缓存Key
        /// </summary>
        public const string RoleFeatureCacheKey = "rolefeaturecachekey";
        /// <summary>
        /// 权限缓存key
        /// </summary>
        public const string AuthCacheKey = "authcachekey";
        /// <summary>
        /// 缓存时间
        /// </summary>
        public static TimeSpan CacheTimeSpan = TimeSpan.FromDays(1);
    }
}
