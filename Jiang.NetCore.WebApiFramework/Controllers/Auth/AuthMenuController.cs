using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class AuthMenuController : BaseController
    {
        private IMenuService _service;
        public AuthMenuController(IMenuService service)
        {
            _service = service;
        }
        /// <summary>
        /// 获取树形菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<VAuthMenu>> GetMenu()
        {
            return Json(new OperateResult<VAuthMenu>(_service.GetTree()));
        }
        /// <summary>
        /// 获取树形菜单和关联的功能项
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        public ActionResult<OperateResult<VAuthMenu>> GetMenuAndFeatures(Guid roleId)
        {
            return Json(new OperateResult<VAuthMenu>(_service.GetTreeAndFeatures(roleId)));
        }
        /// <summary>
        /// 获取登录账户的权限菜单（树形）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<VAuthMenu>> GetAuthMenu()
        {
            var userId = new Guid(User.Identity.Name);
            return Json(new OperateResult<VAuthMenu>(_service.GetTree(userId)));
        }
        /// <summary>
        /// 获取单个菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<OperateResult<VAuthMenu>> Get(Guid id)
        {
            return Json(new OperateResult<VAuthMenu>(_service.GetById(id))); 
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [OperateLog("添加菜单")]
        public ActionResult<OperateResult<Auth_Menu>> Post([FromBody]AuthMenuParam value)
        {
            return Json(_service.Add(value));
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [OperateLog("修改菜单")]
        public ActionResult<OperateResult<Auth_Menu>> Put([FromBody]AuthMenuParam value)
        {
            return Json(_service.Update(value));
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [OperateLog("删除菜单")]
        public ActionResult<OperateResult<Auth_Menu>> Delete(Guid id)
        {
            return Json(_service.Delete(id));
        }
    }
}
