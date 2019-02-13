using Microsoft.EntityFrameworkCore;




using Netson.HotelManage2.Service.IService.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Netson.HotelManage2.Core.Util;
using System.ComponentModel;

namespace Jiang.NetCore.WebApiFramework
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<ManageContext> _unitOfWork;
        private readonly ManageContext _db;
        public UserService(IUnitOfWork<ManageContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.DbContext;
        }
        public OperateResult<Auth_User> Add(UserParam param)
        {
            if (_db.Auth_User.Any(l => l.UserName == param.UserName))
            {
                return OperateResult<Auth_User>.Error("账号重复");
            }
            //判断职位是否存在
            if (!_db.Auth_Job.Any(l => l.Id == param.JobId))
            {
                return OperateResult<Auth_User>.Error("所选职位不存在");
            }
            var obj = param.ToEntity();
            if (obj.Id == Guid.Empty)
            {
                obj.Id = Guid.NewGuid();
            }
            var repo = _unitOfWork.GetRepository<Auth_User>();
            repo.Insert(obj);
            var sucess = _unitOfWork.SaveChanges() == 1;
            return new OperateResult<Auth_User>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", obj);
        }

        public OperateResult<IEnumerable<Auth_RoleUser>> BindRoles(UserBindRolesParam param)
        {
            //校验用户
            if (!_db.Auth_User.Any(l => l.Id == param.UserId))
            {
                return OperateResult<IEnumerable<Auth_RoleUser>>.Error("用户不存在");
            }
            //删除用户的角色
            var userRoles = _db.Auth_RoleUser.Where(l => l.UserId == param.UserId);
            var repo = _unitOfWork.GetRepository<Auth_RoleUser>();
            repo.Delete(userRoles);
            //生成实体
            var list = new List<Auth_RoleUser>();
            foreach (var roleId in param.RoleIds)
            {
                var obj = new Auth_RoleUser()
                {
                    Id = Guid.NewGuid(),
                    UserId = param.UserId,
                    RoleId = roleId
                };
                list.Add(obj);
                repo.Insert(obj);
            }
            _unitOfWork.SaveChanges();
            return OperateResult<IEnumerable<Auth_RoleUser>>.Ok("成功", list);
        }

        public OperateResult<Auth_User> Delete(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Auth_User>();
            repo.Delete(id);
            //删除员工，还需删除该员工绑定的角色关联
            var userroles = _db.Auth_RoleUser.Where(l => l.UserId == id);
            var urrepo = _unitOfWork.GetRepository<Auth_RoleUser>();
            foreach (var ur in userroles)
            {
                urrepo.Delete(ur.Id);
            }
            _unitOfWork.SaveChanges();//提交到数据库
            return OperateResult<Auth_User>.Ok();
        }

        public OperateResult<Auth_User> FindPassword(UserFindPassword param)
        {
            //验证码校验，
            //时间有效期验证--10分钟内
            var time = DateTime.Now.AddMinutes(-10);
            var query = _db.Mem_CheckCode.Where(l => l.Phone == param.Phone && l.Code == param.Code && l.Type == Entity.SMSType.HMFindPassword && l.CreateTime >= time).OrderByDescending(l => l.CreateTime);
            var code = query.FirstOrDefault();
            if (code == null)
            {
                return OperateResult<Auth_User>.Error("验证码无效");
            }
            //用户查找
            var user = _db.Auth_User.Where(l => l.UserName == param.UserName).FirstOrDefault();
            if (user == null)
            {
                return OperateResult<Auth_User>.Error("用户不存在");
            }
            user.Password = param.Password.To32MD5();
            user.ModifyTime = DateTime.Now;
            var repo = _unitOfWork.GetRepository<Auth_User>();
            repo.Update(user);
            //设置密码
            _unitOfWork.SaveChanges();
            return OperateResult<Auth_User>.Ok("成功", user);
        }

        public VUser GetById(Guid id)
        {
            var query = from temp in _db.Auth_User
                        join jobtemp in _db.Auth_Job on temp.JobId equals jobtemp.Id into jobinto
                        from job in jobinto.DefaultIfEmpty()
                        join departmenttemp in _db.Auth_Department on job.DepartmentId equals departmenttemp.Id into departmentinto
                        from department in departmentinto.DefaultIfEmpty()
                        where temp.Id == id
                        select new VUser()
                        {
                            Id = temp.Id,
                            IdCard = temp.IdCard,
                            JobId = temp.JobId,
                            DepartmentId = department.Id,
                            JobName = job.Name,
                            Address = temp.Address,
                            IsFreeze = temp.IsFreeze,
                            Code = temp.Code,
                            CreateTime = temp.CreateTime,
                            DepartmentName = department.Name,
                            Email = temp.Email,
                            ModifyTime = temp.ModifyTime,
                            Password = temp.Password,
                            Phone = temp.Phone,
                            RealName = temp.RealName,
                            Remark = temp.Remark,
                            Sex = temp.Sex,
                            Telephone = temp.Telephone,
                            UserName = temp.UserName,
                            Type = temp.Type
                        };
            return query.FirstOrDefault();
        }

        public IEnumerable<VUser> GetByRole(Guid roleId)
        {
            var query = from temp in _db.Auth_User
                        join jobtemp in _db.Auth_Job on temp.JobId equals jobtemp.Id into jobinto
                        from job in jobinto.DefaultIfEmpty()
                        join departmenttemp in _db.Auth_Department on job.DepartmentId equals departmenttemp.Id into departmentinto
                        from department in departmentinto.DefaultIfEmpty()
                        join userRole in _db.Auth_RoleUser on temp.Id equals userRole.UserId
                        where userRole.RoleId == roleId
                        select new VUser()
                        {
                            Id = temp.Id,
                            IdCard = temp.IdCard,
                            JobId = temp.JobId,
                            DepartmentId = department.Id,
                            JobName = job.Name,
                            Address = temp.Address,
                            IsFreeze = temp.IsFreeze,
                            Code = temp.Code,
                            CreateTime = temp.CreateTime,
                            DepartmentName = department.Name,
                            Email = temp.Email,
                            ModifyTime = temp.ModifyTime,
                            Password = temp.Password,
                            Phone = temp.Phone,
                            RealName = temp.RealName,
                            Remark = temp.Remark,
                            Sex = temp.Sex,
                            Telephone = temp.Telephone,
                            UserName = temp.UserName,
                            Type = temp.Type
                        };
            return query.Distinct().OrderByDescending(l => l.ModifyTime).ToList();
        }

        public IEnumerable<VUser> GetCheckRoom()
        {
            var query = from temp in _db.Auth_User
                        join jobtemp in _db.Auth_Job on temp.JobId equals jobtemp.Id into jobinto
                        from job in jobinto.DefaultIfEmpty()
                        join departmenttemp in _db.Auth_Department on job.DepartmentId equals departmenttemp.Id into departmentinto
                        from department in departmentinto.DefaultIfEmpty()
                        where job.Code == "checkRoom"
                        select new VUser()
                        {
                            Id = temp.Id,
                            IdCard = temp.IdCard,
                            JobId = temp.JobId,
                            DepartmentId = department.Id,
                            JobName = job.Name,
                            Address = temp.Address,
                            IsFreeze = temp.IsFreeze,
                            Code = temp.Code,
                            CreateTime = temp.CreateTime,
                            DepartmentName = department.Name,
                            Email = temp.Email,
                            ModifyTime = temp.ModifyTime,
                            Password = temp.Password,
                            Phone = temp.Phone,
                            RealName = temp.RealName,
                            Remark = temp.Remark,
                            Sex = temp.Sex,
                            Telephone = temp.Telephone,
                            UserName = temp.UserName,
                            Type = temp.Type
                        };
            return query.ToList();
        }

        public IPagedList<VUser> GetPage(int pageSize, int pageIndex, string userName, string realName, Guid? departmentId, Guid? jobId)
        {
            var query = from temp in _db.Auth_User
                        join jobtemp in _db.Auth_Job on temp.JobId equals jobtemp.Id into jobinto
                        from job in jobinto.DefaultIfEmpty()
                        join departmenttemp in _db.Auth_Department on job.DepartmentId equals departmenttemp.Id into departmentinto
                        from department in departmentinto.DefaultIfEmpty()
                        select new VUser()
                        {
                            Id = temp.Id,
                            IdCard = temp.IdCard,
                            JobId = temp.JobId,
                            DepartmentId = department.Id,
                            JobName = job.Name,
                            Address = temp.Address,
                            IsFreeze = temp.IsFreeze,
                            Code = temp.Code,
                            CreateTime = temp.CreateTime,
                            DepartmentName = department.Name,
                            Email = temp.Email,
                            ModifyTime = temp.ModifyTime,
                            Password = temp.Password,
                            Phone = temp.Phone,
                            RealName = temp.RealName,
                            Remark = temp.Remark,
                            Sex = temp.Sex,
                            Telephone = temp.Telephone,
                            UserName = temp.UserName,
                            Type = temp.Type
                        };
            //筛选条件
            Expression<Func<VUser, bool>> expression = (p) => true;
            if (!string.IsNullOrEmpty(userName))
            {
                expression = expression.And(p => p.UserName.Contains(userName));
            }
            if (!string.IsNullOrEmpty(realName))
            {
                expression = expression.And(p => p.RealName.Contains(realName));
            }
            if (departmentId != null)
            {
                expression = expression.And(p => p.DepartmentId == departmentId);
            }
            if (jobId != null)
            {
                expression = expression.And(p => p.JobId == jobId);
            }
            //排序
            Func<IQueryable<VUser>, IOrderedQueryable<VUser>> orderBy = (q) => q.OrderByDescending(p => p.ModifyTime);
            var repo = _unitOfWork.GetRepository<Auth_User>();

            return repo.GetPagedList(query, expression, orderBy, null, pageIndex, pageSize);
        }

        public List<VUsers> GetList(string userName, string realName, Guid? departmentId, Guid? jobId)
        {
            List<VUsers> vUsers = new List<VUsers>();
            //筛选条件
            Expression<Func<VUsers, bool>> expression = (p) => true;
            if (!string.IsNullOrEmpty(userName))
            {
                expression = expression.And(p => p.UserName.Contains(userName));
            }
            if (!string.IsNullOrEmpty(realName))
            {
                expression = expression.And(p => p.RealName.Contains(realName));
            }
            if (departmentId != null)
            {
                expression = expression.And(p => p.DepartmentId == departmentId);
            }
            if (jobId != null)
            {
                expression = expression.And(p => p.JobId == jobId);
            }

            var query = (from temp in _db.Auth_User
                         join jobtemp in _db.Auth_Job on temp.JobId equals jobtemp.Id into jobinto
                         from job in jobinto.DefaultIfEmpty()
                         join departmenttemp in _db.Auth_Department on job.DepartmentId equals departmenttemp.Id into departmentinto
                         from department in departmentinto.DefaultIfEmpty()
                         select new VUsers()
                         {
                             UserId = temp.Id,
                             IdCard = temp.IdCard,
                             JobId = temp.JobId,
                             DepartmentId = department.Id,
                             JobName = job.Name,
                             Address = temp.Address,
                             IsFreeze = temp.IsFreeze,
                             Code = temp.Code,
                             CreateTime = temp.CreateTime,
                             DepartmentName = department.Name,
                             Email = temp.Email,
                             ModifyTime = temp.ModifyTime,
                             Password = temp.Password,
                             Phone = temp.Phone,
                             RealName = temp.RealName,
                             Remark = temp.Remark,
                             Sex = temp.Sex,
                             Telephone = temp.Telephone,
                             UserName = temp.UserName,
                             Type = temp.Type
                         }).Where(expression).ToList();
            var user = (from use in _db.Steward_RowroomUser select use.UserId).ToList();

            foreach (var item in query)
            {
                if (!user.Contains(item.UserId))
                {
                    vUsers.Add(item);
                }
            }

            return vUsers;
        }

        public IEnumerable<VUser> GetRecheckRoom()
        {
            var query = from temp in _db.Auth_User
                        join jobtemp in _db.Auth_Job on temp.JobId equals jobtemp.Id into jobinto
                        from job in jobinto.DefaultIfEmpty()
                        join departmenttemp in _db.Auth_Department on job.DepartmentId equals departmenttemp.Id into departmentinto
                        from department in departmentinto.DefaultIfEmpty()
                        where job.Code == "checkRoomAgain"
                        select new VUser()
                        {
                            Id = temp.Id,
                            IdCard = temp.IdCard,
                            JobId = temp.JobId,
                            DepartmentId = department.Id,
                            JobName = job.Name,
                            Address = temp.Address,
                            IsFreeze = temp.IsFreeze,
                            Code = temp.Code,
                            CreateTime = temp.CreateTime,
                            DepartmentName = department.Name,
                            Email = temp.Email,
                            ModifyTime = temp.ModifyTime,
                            Password = temp.Password,
                            Phone = temp.Phone,
                            RealName = temp.RealName,
                            Remark = temp.Remark,
                            Sex = temp.Sex,
                            Telephone = temp.Telephone,
                            UserName = temp.UserName,
                            Type = temp.Type
                        };
            return query.ToList();
        }

        public IEnumerable<Auth_User> GetSalesman()
        {
            return _db.Auth_User.Where(l => l.Type == UserType.Salesman).OrderByDescending(l => l.ModifyTime).ToList();
        }

        public IEnumerable<KeyValuePair<int, string>> GetUserTypes()
        {
            var list = new List<KeyValuePair<int, string>>();
            var fieldinfo = typeof(UserType).GetFields();
            foreach (var item in fieldinfo)
            {
                var obj = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (obj != null && obj.Length != 0)
                {
                    DescriptionAttribute des = (DescriptionAttribute)obj[0];
                    var key = item.GetValue(item.Name);
                    var keyint = Convert.ToInt32(key);
                    var kv = new KeyValuePair<int, string>(keyint, des.Description);
                    list.Add(kv);
                }
            }
            return list;
        }

        public OperateResult<LoginUserModel> LoginCheck(string userName, string password)
        {
            var user = _db.Auth_User.Where(l => l.UserName == userName).FirstOrDefault();
            if (user == null)
            {
                return OperateResult<LoginUserModel>.Error("用户名或密码错误");
            }
            if (user.Password != password.To32MD5())
            {
                return OperateResult<LoginUserModel>.Error("用户名或密码错误");
            }
            //获取用户角色
            var query = from temp in _db.Auth_Role
                        join roleusertemp in _db.Auth_RoleUser on temp.Id equals roleusertemp.RoleId into roleuserinto
                        from roleuser in roleuserinto.DefaultIfEmpty()
                        where roleuser.UserId == user.Id
                        select temp;
            var roles = query.Distinct().ToList();
            var auths = new List<Auth_Auth>();
            if (roles != null)
            {
                var roleIds = roles.Select(l => l.Id).ToList();
                //获取用户的功能项
                var fetureIds = _db.Auth_RoleFeature.Where(l => roleIds.Contains(l.RoleId)).Select(l => l.FeatureId).ToList();
                if (fetureIds != null)
                {
                    auths = _db.Auth_Auth.Where(l => fetureIds.Contains(l.FeatureId)).ToList();
                }
            }
            //创建登录历史
            var repos = _unitOfWork.GetRepository<Auth_UserLoginHistory>();
            var his = new Auth_UserLoginHistory()
            {
                UserId = user.Id,
                UserName = user.UserName
            };
            repos.Insert(his);
            _unitOfWork.SaveChanges();
            return OperateResult<LoginUserModel>.Ok("成功", new LoginUserModel()
            {
                User = user,
                Roles = roles,
                Auths = auths
            });
        }

        public OperateResult<Auth_UserLoginHistory> Logout(Guid userId)
        {
            //获取该用户最近的一条登录记录
            var history = _db.Auth_UserLoginHistory.Where(l => l.UserId == userId).OrderByDescending(l => l.CreateTime)
                .FirstOrDefault();
            if (history != null)
            {
                history.LogoutTime = DateTime.Now;
                history.ModifyTime = DateTime.Now;
            }

            var repo = _unitOfWork.GetRepository<Auth_UserLoginHistory>();
            repo.Update(history);
            _unitOfWork.SaveChanges();
            return OperateResult<Auth_UserLoginHistory>.Ok("成功", history);
        }

        public OperateResult<Auth_User> ResetPassword(UserResetPassword param)
        {
            //查找用户，直接修改密码
            var repo = _unitOfWork.GetRepository<Auth_User>();
            var old = repo.Find(param.UserId);
            if (old == null)
                return OperateResult<Auth_User>.Error("用户不存在");

            old.Password = param.Password.To32MD5();
            old.ModifyTime = DateTime.Now;
            repo.Update(old);
            _unitOfWork.SaveChanges();
            return OperateResult<Auth_User>.Ok("修改成功", old);
        }

        public OperateResult<Auth_User> Update(UserParam param)
        {
            var repo = _unitOfWork.GetRepository<Auth_User>();
            var old = repo.Find(param.Id);
            if (old == null)
            {
                return OperateResult<Auth_User>.Error("对象不存在", old);
            }
            //判断职位是否存在
            if (!_db.Auth_Job.Any(l => l.Id == param.JobId))
            {
                return OperateResult<Auth_User>.Error("所选职位不存在");
            }
            if (_db.Auth_User.Any(l => l.UserName == param.UserName && l.Id != param.Id))
            {
                return OperateResult<Auth_User>.Error("账号重复", old);
            }
            old.UserName = param.UserName;
            old.JobId = param.JobId;
            old.Code = param.Code;
            old.RealName = param.RealName;
            old.Password = param.Password.To32MD5();
            old.Sex = param.Sex;
            old.IdCard = param.IdCard;
            old.Email = param.Email;
            old.Telephone = param.Telephone;
            old.Phone = param.Phone;
            old.Address = param.Address;
            old.Remark = param.Remark;
            old.ModifyTime = DateTime.Now;
            old.Type = param.Type;
            repo.Update(old);
            var sucess = _unitOfWork.SaveChanges() == 1;//提交到数据库
            return new OperateResult<Auth_User>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", old);
        }

        public OperateResult<Auth_User> UpdatePassword(UserUpdatePassword param, Guid userId)
        {
            //获取旧用户，验证旧密码，修改新密码
            var repo = _unitOfWork.GetRepository<Auth_User>();
            var old = repo.Find(userId);
            if (old == null)
                return OperateResult<Auth_User>.Error("用户不存在");
            if (old.Password != param.OldPassword.To32MD5())
            {
                return OperateResult<Auth_User>.Error("密码错误");
            }
            old.Password = param.NewPassword.To32MD5();
            repo.Update(old);
            _unitOfWork.SaveChanges();
            return OperateResult<Auth_User>.Ok("修改成功", old);
        }
    }
}
