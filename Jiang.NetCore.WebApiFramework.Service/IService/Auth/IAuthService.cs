
using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    public interface IAuthService
    {
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<IEnumerable<Auth_Auth>> Update(FeatureAuthParam param);
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        List<Auth_Auth> GetAll();
        /// <summary>
        /// 根据功能项ID获取权限
        /// </summary>
        /// <param name="featureId"></param>
        /// <returns></returns>
        IEnumerable<Auth_Auth> GetByFeatureId(Guid featureId);
    }
}
