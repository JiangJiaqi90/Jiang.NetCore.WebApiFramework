using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 职位管理
    /// </summary>
    public class AuthJobController : BaseController
    {

        private IJobService _service;
        public AuthJobController(IJobService service)
        {
            _service = service;
        }
        /// <summary>
        /// 根据Id获取职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<OperateResult<VAuthJob>> Get(Guid id)
        {
            return Json(_service.GetById(id));
        }

        /// <summary>
        /// 添加职位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperateLog("添加职位")]
        [HttpPost]
        public ActionResult<OperateResult<Auth_Job>> Post([FromBody]AuthJobParam value)
        {
            return Json(_service.Add(value));
        }

        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [OperateLog("修改职位")]
        public ActionResult<OperateResult<Auth_Job>> Put([FromBody]AuthJobParam value)
        {
            return Json(_service.Update(value));
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [OperateLog("删除职位")]
        public ActionResult<OperateResult<Auth_Job>> Delete(Guid id)
        {
            return Json(_service.Delete(id));
        }
        /// <summary>
        /// 获取所有职位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<IEnumerable<VAuthJob>>> GetAll()
        {
            return Json(new OperateResult<IEnumerable<VAuthJob>>(_service.GetAll()));
        }
        /// <summary>
        /// 根据部门ID获取职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<OperateResult<IEnumerable<Auth_Job>>> GetByDepartment(Guid id)
        {
            return Json(new OperateResult<IEnumerable<Auth_Job>>(_service.GetByDepartment(id)));
        }
    }
}
