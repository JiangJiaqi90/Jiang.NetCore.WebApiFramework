


using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    public interface IFeatureService
    {
        /// <summary>
        /// 更新功能项
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<IEnumerable<Auth_Feature>> Update(AuthFeatureParam param);
        /// <summary>
        /// 根据菜单ID获取功能项
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        IEnumerable<Auth_Feature> GetByMenuId(Guid menuId);
    }
}
