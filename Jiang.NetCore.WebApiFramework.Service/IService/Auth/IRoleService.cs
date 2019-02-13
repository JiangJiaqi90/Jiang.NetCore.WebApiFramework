using Microsoft.EntityFrameworkCore;



using System;
using System.Collections.Generic;
using System.Text;

namespace Netson.HotelManage2.Service.IService.Auth
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_Role> Add(AuthRoleParam param);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_Role> Update(AuthRoleParam param);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperateResult<Auth_Role> Delete(Guid id);
        /// <summary>
        /// 根据ID查询角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Auth_Role GetById(Guid id);
        /// <summary>
        /// 分页查询角色
        /// </summary>
        /// <returns></returns>
        IPagedList<Auth_Role> GetPage(int pageSize, int pageIndex, string name);
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        List<Auth_Role> GetAll();
        /// <summary>
        /// 获取所有角色功能关联
        /// </summary>
        /// <returns></returns>
        List<Auth_RoleFeature> GetAllRoleFeature();
        /// <summary>
        /// 角色绑定用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<IEnumerable<Auth_RoleUser>> BindUsers(RoleBindUsersParam param);

        /// <summary>
        /// 用户绑定功能项
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<IEnumerable<Auth_RoleFeature>> BindFeatures(RoleBindFeaturesParam param);
        /// <summary>
        /// 获取用户绑定的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<VAuthRole> GetByUser(Guid userId);
    }
}
