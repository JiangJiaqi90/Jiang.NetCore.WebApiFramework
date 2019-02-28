using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class AuthController : BaseController
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private MemoryCache _cache;
        private CacheHelp _cacheHelp;
        private IAuthService _service;
        public AuthController(IAuthService service, MyMemoryCache myMemoryCache, CacheHelp cacheHelp)
        {
            _service = service;
            _cache = myMemoryCache.Cache;
            _cacheHelp = cacheHelp;
        }
        /// <summary>
        /// 更新功能项
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperateLogAttribute))]
        public ActionResult<OperateResult<IEnumerable<Auth_Auth>>> Update([FromBody]FeatureAuthParam param)
        {
            var result = Json(_service.Update(param));
            _cacheHelp.UpdateAuthCache();
            return result;
        }
        /// <summary>
        /// 根据功能项ID获取权限
        /// </summary>
        /// <param name="featureId"></param>
        /// <returns></returns>
        [HttpGet("{featureId}")]
        public ActionResult<OperateResult<IEnumerable<Auth_Auth>>> GetByFeatureId(Guid featureId)
        {
            return Json(new OperateResult<IEnumerable<Auth_Auth>>(_service.GetByFeatureId(featureId)));
        }


    }
}
