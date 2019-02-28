using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 功能项管理
    /// </summary>
    public class AuthFeatureController : BaseController
    {
        private IFeatureService _service;
        public AuthFeatureController(IFeatureService service)
        {
            _service = service;
        }
        /// <summary>
        /// 更新功能项
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(OperateLogAttribute))]
        public ActionResult<OperateResult<IEnumerable<Auth_Feature>>> Update([FromBody]AuthFeatureParam param)
        {
            return Json(_service.Update(param));
        }
        /// <summary>
        /// 根据菜单ID获取功能项
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet("{menuId}")]
        public ActionResult<OperateResult<IEnumerable<Auth_Feature>>> GetByMenuId(Guid menuId)
        {
            return Json(new OperateResult<IEnumerable<Auth_Feature>>(_service.GetByMenuId(menuId)));
        }

    }
}
