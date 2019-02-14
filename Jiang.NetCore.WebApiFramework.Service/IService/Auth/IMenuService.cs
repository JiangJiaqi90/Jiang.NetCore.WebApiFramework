
using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    public interface IMenuService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_Menu> Add(AuthMenuParam param);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_Menu> Update(AuthMenuParam param);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperateResult<Auth_Menu> Delete(Guid id);
        /// <summary>
        /// 根据ID查询角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        VAuthMenu GetById(Guid id);
        /// <summary>
        /// 获取所有菜单--不含子菜单
        /// </summary>
        /// <returns></returns>
        List<VAuthMenu> GetAll();
        /// <summary>
        /// 获取所有菜单--包含子菜单
        /// </summary>
        /// <returns></returns>
        IEnumerable<VAuthMenu> GetTreeList();
        /// <summary>
        /// 获取树形菜单
        /// </summary>
        /// <returns></returns>
        VAuthMenu GetTree();
        /// <summary>
        /// 获取用户有权限的菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        VAuthMenu GetTree(Guid userId);
        /// <summary>
        /// 根据角色Id获取树形菜单（包含关联的功能项）
        /// 同时将该角色关联的功能项标记为已绑定
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        VAuthMenu GetTreeAndFeatures(Guid roleId);

    }
}
