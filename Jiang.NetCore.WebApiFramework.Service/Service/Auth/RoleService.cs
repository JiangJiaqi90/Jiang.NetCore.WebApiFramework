using Microsoft.EntityFrameworkCore;




using Jiang.NetCore.WebApiFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace Jiang.NetCore.WebApiFramework
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork<ManageContext> _unitOfWork;
        private readonly ManageContext _db;
        public RoleService(IUnitOfWork<ManageContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.DbContext;
        }
        public OperateResult<Auth_Role> Add(AuthRoleParam param)
        {
            if (_db.Auth_Role.Any(l => l.Name == param.Name))
            {
                return OperateResult<Auth_Role>.Error("名称重复");
            }
            //if (_db.Auth_Role.Any(l => l.Code == param.Code))
            //{
            //    return OperateResult<Auth_Role>.Error("编码重复");
            //}
            var obj = param.ToEntity();
            if (obj.Id == Guid.Empty)
            {
                obj.Id = Guid.NewGuid();
            }
            var repo = _unitOfWork.GetRepository<Auth_Role>();
            repo.Insert(obj);
            var sucess = _unitOfWork.SaveChanges() == 1;
            return new OperateResult<Auth_Role>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", obj);
        }

        public OperateResult<IEnumerable<Auth_RoleFeature>> BindFeatures(RoleBindFeaturesParam param)
        {
            //校验角色
            if (!_db.Auth_Role.Any(l => l.Id == param.RoleId))
            {
                return OperateResult<IEnumerable<Auth_RoleFeature>>.Error("角色不存在");
            }
            //删除角色功能项
            var roleFeatures = _db.Auth_RoleFeature.Where(l => l.RoleId == param.RoleId);
            var repo = _unitOfWork.GetRepository<Auth_RoleFeature>();
            repo.Delete(roleFeatures);
            //生成实体
            var list = new List<Auth_RoleFeature>();
            foreach (var featureId in param.FeatureIds)
            {
                var obj = new Auth_RoleFeature()
                {
                    Id = Guid.NewGuid(),
                    FeatureId = featureId,
                    RoleId = param.RoleId
                };
                list.Add(obj);
                repo.Insert(obj);
            }
            _unitOfWork.SaveChanges();
            return OperateResult<IEnumerable<Auth_RoleFeature>>.Ok("成功", list);
        }

        public OperateResult<IEnumerable<Auth_RoleUser>> BindUsers(RoleBindUsersParam param)
        {
            //校验角色
            if (!_db.Auth_Role.Any(l => l.Id == param.RoleId))
            {
                return OperateResult<IEnumerable<Auth_RoleUser>>.Error("角色不存在");
            }
            //删除角色用户
            var roleUsers = _db.Auth_RoleUser.Where(l => l.RoleId == param.RoleId);
            var repo = _unitOfWork.GetRepository<Auth_RoleUser>();
            repo.Delete(roleUsers);
            //生成实体
            var list = new List<Auth_RoleUser>();
            foreach (var userId in param.UserIds)
            {
                var obj = new Auth_RoleUser()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = param.RoleId
                };
                list.Add(obj);
                repo.Insert(obj);
            }
            _unitOfWork.SaveChanges();
            return OperateResult<IEnumerable<Auth_RoleUser>>.Ok("成功", list);
        }

        public OperateResult<Auth_Role> Delete(Guid id)
        {
            //判断角色是否绑定用户
            if (_db.Auth_RoleUser.Any(l => l.RoleId == id))
            {
                return OperateResult<Auth_Role>.Error("角色已经绑定了用户");
            }
            //判断角色是否绑定功能项
            if (_db.Auth_RoleFeature.Any(l => l.RoleId == id))
            {
                return OperateResult<Auth_Role>.Error("角色已经绑定了功能项");
            }
            var role = GetById(id);
            if (role.Code == "admin")
            {
                return OperateResult<Auth_Role>.Error("系统默认管理员角色不允许删除");
            }
            var repo = _unitOfWork.GetRepository<Auth_Role>();
            repo.Delete(id);
            var success = _unitOfWork.SaveChanges() == 1;//提交到数据库
            if (success)
                return OperateResult<Auth_Role>.Ok();
            return OperateResult<Auth_Role>.Error("失败");
        }

        public List<Auth_Role> GetAll()
        {
            return _db.Auth_Role.OrderBy(l=>l.Sort).ToList();
        }

        public List<Auth_RoleFeature> GetAllRoleFeature()
        {
            return _db.Auth_RoleFeature.ToList();
        }

        public Auth_Role GetById(Guid id)
        {
            return _unitOfWork.GetRepository<Auth_Role>().Find(id);
        }

        public IEnumerable<VAuthRole> GetByUser(Guid userId)
        {
            //获取所有角色，获取当前用户的角色ID
            var roles = _db.Auth_Role.OrderBy(l=>l.Sort);
            var list = new List<VAuthRole>();
            var userRoleIds = _db.Auth_RoleUser.Where(l => l.UserId == userId).Select(l => l.RoleId).ToList();
            foreach(var role in roles)
            {
                var vrole = new VAuthRole(role);
                list.Add(vrole);
                if (userRoleIds != null)
                {
                    if (userRoleIds.Contains(vrole.Id))
                    {
                        vrole.Selected = true;
                    }
                }
            }
            return list;
        }

        public IPagedList<Auth_Role> GetPage(int pageSize, int pageIndex, string name)
        {
            var repo = _unitOfWork.GetRepository<Auth_Role>();
            //筛选条件
            Expression<Func<Auth_Role, bool>> expression = (p) => true;
            if (!string.IsNullOrEmpty(name))
            {
                expression = expression.And(p => p.Name.Contains(name));
            }
            //排序
            Func<IQueryable<Auth_Role>, IOrderedQueryable<Auth_Role>> orderBy = (q) => q.OrderBy(p => p.Sort );
            return repo.GetPagedList(expression, orderBy, null, pageIndex, pageSize);
        }

        public OperateResult<Auth_Role> Update(AuthRoleParam param)
        {
            var repo = _unitOfWork.GetRepository<Auth_Role>();
            var old = GetById(param.Id);
            if (old == null)
            {
                return OperateResult<Auth_Role>.Error("对象不存在", old);
            }
            if (_db.Auth_Role.Any(l => l.Name == param.Name && l.Id != param.Id))
            {
                return OperateResult<Auth_Role>.Error("名称重复", old);
            }
            //if (_db.Auth_Role.Any(l => l.Code == param.Code && l.Id != param.Id))
            //{
            //    return OperateResult<Auth_Role>.Error("编码重复", old);
            //}
            old.Sort = param.Sort;
            old.Name = param.Name;
            //old.Code = param.Code;//这个编码不允许修改
            old.Remark = param.Remark;
            old.ModifyTime = DateTime.Now;
            repo.Update(old);
            var sucess = _unitOfWork.SaveChanges() == 1;//提交到数据库
            return new OperateResult<Auth_Role>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", old);
        }
    }
}
