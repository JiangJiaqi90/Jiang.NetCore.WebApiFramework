


using System;
using System.Collections.Generic;
using System.Text;

namespace Netson.HotelManage2.Service.IService.Auth
{
    public interface IJobService
    {
        /// <summary>
        /// 添加职位
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_Job> Add(AuthJobParam param);
        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_Job> Update(AuthJobParam param);
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperateResult<Auth_Job> Delete(Guid id);
        /// <summary>
        /// 根据ID查询职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        VAuthJob GetById(Guid id);
        /// <summary>
        /// 查询所有职位
        /// </summary>
        /// <returns></returns>
        IEnumerable<VAuthJob> GetAll();
    }
}
