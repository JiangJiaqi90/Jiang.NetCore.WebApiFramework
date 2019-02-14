using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class AuthRoleController : BaseController
    {
        //private IRoleService _service;
        ///// <summary>
        ///// 缓存
        ///// </summary>
        //private MemoryCache _cache;
        //private CacheHelp _cacheHelp;
        //public AuthRoleController(IRoleService service, MyMemoryCache myMemoryCache, CacheHelp cacheHelp)
        //{
        //    _service = service;
        //    _cache = myMemoryCache.Cache;
        //    _cacheHelp = cacheHelp;
        //}
        ///// <summary>
        ///// 根据Id获取角色
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public ActionResult<OperateResult<Auth_Role>> Get(Guid id)
        //{
        //    return Json(new OperateResult<Auth_Role>(_service.GetById(id)));
        //}

        ///// <summary>
        ///// 添加角色
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult<OperateResult<Auth_Role>> Post([FromBody]AuthRoleParam value)
        //{
        //    var result = Json(_service.Add(value));
        //    _cacheHelp.UpdateRoleCache();
        //    return result;
        //}

        ///// <summary>
        ///// 修改角色
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public ActionResult<OperateResult<Auth_Role>> Put([FromBody]AuthRoleParam value)
        //{
        //    var result= Json(_service.Update(value));
        //    _cacheHelp.UpdateRoleCache();
        //    return result;
        //}

        ///// <summary>
        ///// 删除角色
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete("{id}")]
        //public ActionResult<OperateResult<Auth_Role>> Delete(Guid id)
        //{
        //    var result = Json(_service.Delete(id));
        //    _cacheHelp.UpdateRoleCache();
        //    return result;
        //}
        ///// <summary>
        ///// 根据用户获取角色
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult<OperateResult<IEnumerable<VAuthRole>>> GetByUser(Guid userId)
        //{
        //    return Json(new OperateResult<IEnumerable<VAuthRole>>(_service.GetByUser(userId)));
        //}
        ///// <summary>
        ///// 分页查询角色
        ///// </summary>
        ///// <param name="pageSize">分页大小</param>
        ///// <param name="pageIndex">页码：从0开始</param>
        ///// <param name="name">名称</param>
        ///// <returns></returns>
        //[HttpGet("{pageIndex}/{pageSize}")]
        //public ActionResult<OperateResult<IPagedList<Auth_Role>>> GetPage(int pageSize, int pageIndex, string name=null)
        //{
        //    return Json(new OperateResult<IPagedList<Auth_Role>>(_service.GetPage(pageSize, pageIndex, name)));
        //}
        ///// <summary>
        ///// 角色绑定用户
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult<OperateResult<IEnumerable<Auth_RoleUser>>> BindUsers([FromBody]RoleBindUsersParam param)
        //{
        //    return Json(_service.BindUsers(param));
        //}

        ///// <summary>
        ///// 角色绑定功能项
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult<OperateResult<IEnumerable<Auth_RoleFeature>>> BindFeatures([FromBody]RoleBindFeaturesParam param)
        //{
        //    var result = Json(_service.BindFeatures(param));
        //    _cacheHelp.UpdateRoleFeatureCache();
        //    return result;
        //}
        
    }
}
