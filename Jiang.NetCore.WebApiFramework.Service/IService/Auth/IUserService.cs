using Microsoft.EntityFrameworkCore;



using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    public interface IUserService
    {
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_User> Add(UserParam param);
        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_User> Update(UserParam param);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_User> UpdatePassword(UserUpdatePassword param, Guid userId);
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_User> FindPassword(UserFindPassword param);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<Auth_User> ResetPassword(UserResetPassword param);
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OperateResult<Auth_User> Delete(Guid id);
        /// <summary>
        /// 根据ID查询员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        VUser GetById(Guid id);
        /// <summary>
        /// 分页查询员工
        /// </summary>
        /// <returns></returns>
        IPagedList<VUser> GetPage(int pageSize, int pageIndex, string userName, string realName, Guid? departmentId, Guid? jobId);

        List<VUsers> GetList(string userName, string realName, Guid? departmentId, Guid? jobId);
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userName">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        OperateResult<LoginUserModel> LoginCheck(string userName, string password);
        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        OperateResult<Auth_UserLoginHistory> Logout(Guid userId);
        /// <summary>
        /// 用户绑定角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        OperateResult<IEnumerable<Auth_RoleUser>> BindRoles(UserBindRolesParam param);
        /// <summary>
        /// 获取用户类别
        /// </summary>
        /// <returns></returns>
        IEnumerable<KeyValuePair<int, string>> GetUserTypes();
        /// <summary>
        /// 获取销售员
        /// </summary>
        /// <returns></returns>
        IEnumerable<Auth_User> GetSalesman();
        /// <summary>
        /// 根据角色ID查询用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IEnumerable<VUser> GetByRole(Guid roleId);
        /// <summary>
        /// 获取查房人员
        /// </summary>
        /// <returns></returns>
        IEnumerable<VUser> GetCheckRoom();
        /// <summary>
        /// 获取复审人员
        /// </summary>
        /// <returns></returns>
        IEnumerable<VUser> GetRecheckRoom();
    }
}
