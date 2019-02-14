


using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 部门管理
    /// </summary>
    public interface IDepartmentService
    {
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_Department> Add(AuthDepartmentParam param);
        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_Department> Update(AuthDepartmentParam param);
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperateResult<Auth_Department> Delete(Guid id);
        /// <summary>
        /// 根据ID查询部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Auth_Department GetById(Guid id);
        /// <summary>
        /// 查询所有部门
        /// </summary>
        /// <returns></returns>
        IEnumerable<Auth_Department> GetAll();
    }
}
