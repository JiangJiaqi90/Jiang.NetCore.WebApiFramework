//---------------------
// Description: 角色策略授权处理
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jiang.NetCore.WebApiFramework
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        /// <summary>
        /// 授权方式（cookie, bearer, oauth, openid）
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }
        /// <summary>
        /// 缓存
        /// </summary>
        private MemoryCache _cache;
        private CacheHelp _cacheHelp;
        private readonly ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="memoryCache"></param>
        /// <param name="cacheHelp"></param>
        /// <param name="logger"></param>
        public PolicyHandler(IAuthenticationSchemeProvider schemes, MyMemoryCache memoryCache, CacheHelp cacheHelp, ILogger<PolicyHandler> logger)
        {
            Schemes = schemes;
            _cache = memoryCache.Cache;
            _cacheHelp = cacheHelp;
            _logger = logger;
        }

        //授权处理
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            try
            {
                var httpContext = (context.Resource as AuthorizationFilterContext).HttpContext;

                //获取授权方式
                var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
                if (defaultAuthenticate != null)
                {
                    //验证签发的用户信息
                    var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                    if (result.Succeeded)
                    {

                        httpContext.User = result.Principal;

                        //判断角色与 Url 是否对应--校验角色接口权限
                        //当前用户角色
                        var roleNames = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(l => l.Value);
                        //当前用户角色
                        var allRoles = _cacheHelp.GetRoles();
                        var roles = allRoles.Where(l => roleNames.Contains(l.Code)).ToList();
                        if (roles.Count < 1)
                        {
                            context.Fail();
                            return;
                        }
                        //有角色才进行以下判断
                        //若为管理员，直接给权限
                        if (roles.Any(l => l.Code == "admin"))
                        {
                            context.Succeed(requirement);
                            return;
                        }
                        //判断接口权限--角色接口权限
                        //获取当前路由
                        var currentRoute = httpContext.Request.Path.Value.ToLower();
                        //如果当前路由不在权限表里面视为有权限 即：!currentRoute.Contains(l.Url.ToLower())
                        var allAuths = _cacheHelp.GetAuths().Where(l => !string.IsNullOrEmpty(l.Url)).ToList();
                        if (allAuths != null && allAuths.Count > 0 && allAuths.Any(l => currentRoute.Contains(l.Url.ToLower())))
                        {
                            //获取角色权限
                            var auths = _cacheHelp.GetAuthByRoleIds(roles.Select(l => l.Id).ToList());
                            var authRoutes = auths.Where(l => l.Url.Contains("/")).Select(l => l.Url).ToList();//当前角色配置的所有权限URL
                            foreach (var r in authRoutes)
                            {
                                if (currentRoute.Contains(r))
                                {
                                    //包含这个接口，则有权限
                                    context.Succeed(requirement);
                                    return;
                                }
                            }
                            //循环完毕，没有返回，表示不包含这个接口，没权限
                            context.Fail();
                            return;
                        }
                        context.Succeed(requirement);
                        return;//如果当前路由不在权限表里面视为有权限
                    }
                }
                context.Fail();
            }
            catch (Exception ex)
            {
                _logger.LogError($"身份验证异常：{ex.Message}");
                context.Fail();
            }
            
        }

    }
}
