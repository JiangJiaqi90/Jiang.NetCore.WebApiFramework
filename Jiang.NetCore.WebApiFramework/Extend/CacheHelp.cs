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
    public class CacheHelp
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private MemoryCache _cache;
        private IRoleService _roleService;
        private IAuthService _authService;
        public CacheHelp(MyMemoryCache memoryCache, IRoleService roleService, IAuthService authService)
        {
            _cache = memoryCache.Cache;
            _roleService = roleService;
            _authService = authService;
        }
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public List<Auth_Role> GetRoles()
        {
            return _cache.GetOrCreate(MyMemoryCache.RoleCacheKey, entry =>
            {
                entry.SlidingExpiration = MyMemoryCache.CacheTimeSpan;
                return _roleService.GetAll();
            });
        }
        /// <summary>
        /// 获取角色功能项 
        /// </summary>
        /// <returns></returns>
        public List<Auth_RoleFeature> GetRoleFeatures()
        {
            return _cache.GetOrCreate(MyMemoryCache.RoleFeatureCacheKey, entry =>
            {
                entry.SlidingExpiration = MyMemoryCache.CacheTimeSpan;
                return _roleService.GetAllRoleFeature();
            });
        }
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public List<Auth_Auth> GetAuths()
        {
            return  _cache.GetOrCreate(MyMemoryCache.AuthCacheKey, entry =>
            {
                entry.SlidingExpiration = MyMemoryCache.CacheTimeSpan;
                return _authService.GetAll();
            });
        }
        /// <summary>
        /// 根据角色ID获取关联的权限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public List<Auth_Auth> GetAuthByRoleIds(List<Guid> roleIds)
        {
            if (roleIds == null||roleIds.Count==0)
                return null;
            var featureIds = GetRoleFeatures().Where(l => roleIds.Contains(l.RoleId)).Select(l=>l.Id).ToList() ;
            if (featureIds == null || featureIds.Count == 0)
                return null;
            return GetAuths().Where(l => featureIds.Contains(l.FeatureId)).ToList();
        }
        /// <summary>
        /// 更新角色功能关联
        /// </summary>
        public void UpdateRoleFeatureCache()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Set cache entry size by extension method.
                .SetSize(1)
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(MyMemoryCache.CacheTimeSpan);

            // Set cache entry size via property.
            // cacheEntryOptions.Size = 1;
            // Save data in cache.
            _cache.Set(MyMemoryCache.RoleFeatureCacheKey, _roleService.GetAllRoleFeature(), cacheEntryOptions);
        }
        /// <summary>
        /// 更新角色缓存
        /// </summary>
        public void UpdateRoleCache()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Set cache entry size by extension method.
                .SetSize(1)
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(MyMemoryCache.CacheTimeSpan);

            // Set cache entry size via property.
            // cacheEntryOptions.Size = 1;
            // Save data in cache.
            _cache.Set(MyMemoryCache.RoleCacheKey, _roleService.GetAll(), cacheEntryOptions);
        }
        /// <summary>
        /// 更新权限缓存
        /// </summary>
        public void UpdateAuthCache()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Set cache entry size by extension method.
                .SetSize(1)
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(MyMemoryCache.CacheTimeSpan);

            // Set cache entry size via property.
            // cacheEntryOptions.Size = 1;
            // Save data in cache.
            _cache.Set(MyMemoryCache.AuthCacheKey, _authService.GetAll(), cacheEntryOptions);
        }
    }
}
