using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 部门管理
    /// </summary>
    public class AuthDepartmentController : BaseController
    {
        private IDepartmentService _service;
        public AuthDepartmentController(IDepartmentService service)
        {
            _service = service;
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ServiceFilter(typeof(OperateLogAttribute))]
        public ActionResult<OperateResult<Auth_Department>> Post([FromBody]AuthDepartmentParam value)
        {
            return Json(_service.Add(value));
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceFilter(typeof(OperateLogAttribute))]
        public ActionResult<OperateResult<Auth_Department>> Put([FromBody]AuthDepartmentParam value)
        {
            return Json(_service.Update(value));
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(OperateLogAttribute))]
        public ActionResult<OperateResult<Auth_Department>> Delete(Guid id)
        {
            return Json(_service.Delete(id));
        }
        /// <summary>
        /// 获取单个部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<OperateResult<Auth_Department>> Get(Guid id)
        {
            return Json(new OperateResult<Auth_Department>(_service.GetById(id)));
        }
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<IEnumerable< Auth_Department>>> GetAll()
        {
            return Json(new OperateResult<IEnumerable<Auth_Department>> (_service.GetAll()));
        }
    }
}
