using Microsoft.EntityFrameworkCore;




using Netson.HotelManage2.Service.IService.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork<ManageContext> _unitOfWork;
        private readonly ManageContext _db;
        public JobService(IUnitOfWork<ManageContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.DbContext;
        }
        public OperateResult<Auth_Job> Add(AuthJobParam param)
        {
            var obj = param.ToEntity();
            //判断部门是否存在
            if (!_db.Auth_Department.Any(l => l.Id == param.DepartmentId))
            {
                return OperateResult<Auth_Job>.Error("所选部门不存在");
            }
            if (_db.Auth_Job.Any(l => l.Code == param.Code))
            {
                return OperateResult<Auth_Job>.Error("编号有重复");
            }
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            var repo = _unitOfWork.GetRepository<Auth_Job>();
            repo.Insert(obj);
            var sucess = _unitOfWork.SaveChanges() == 1;
            return new OperateResult<Auth_Job>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", obj);
        }

        public OperateResult<Auth_Job> Delete(Guid id)
        {
            //判断职位下是否有员工
            if (_db.Auth_User.Any(l => l.JobId == id))
            {
                return OperateResult<Auth_Job>.Error("该职位下存在员工");
            }
            var repo = _unitOfWork.GetRepository<Auth_Job>();
            var old = repo.Find(id);
            if(old.Code == "checkRoom" || old.Code== "checkRoomAgain")
            {
                return OperateResult<Auth_Job>.Error("系统预置职位，不允许删除");
            }
            repo.Delete(id);
            var success = _unitOfWork.SaveChanges() == 1;//提交到数据库
            if (success)
                return OperateResult<Auth_Job>.Ok();
            return OperateResult<Auth_Job>.Error("失败");
        }

        public IEnumerable<VAuthJob> GetAll()
        {
            var query = from temp in _db.Auth_Job
                        join departmentTemp in _db.Auth_Department on temp.DepartmentId equals departmentTemp.Id into departmentInto
                        from department in departmentInto.DefaultIfEmpty()
                        select new VAuthJob()
                        {
                            Id = temp.Id,
                            Name = temp.Name,
                            DepartmentId = temp.DepartmentId,
                            DepartmentName = department.Name,
                            Code = temp.Code,
                            ModifyTime = temp.ModifyTime,
                            CreateTime = temp.CreateTime,
                            Remark = temp.Remark,
                            Sort = temp.Sort
                        };
            return query.OrderBy(l=>l.Sort).ToList();
        }

        public VAuthJob GetById(Guid id)
        {
            var query = from temp in _db.Auth_Job
                        join departmentTemp in _db.Auth_Department on temp.DepartmentId equals departmentTemp.Id into departmentInto
                        from department in departmentInto.DefaultIfEmpty()
                        where temp.Id ==id
                        select new VAuthJob()
                        {
                            Id = temp.Id,
                            Name = temp.Name,
                            DepartmentId = temp.DepartmentId,
                            DepartmentName = department.Name,
                            Code = temp.Code,
                            ModifyTime = temp.ModifyTime,
                            CreateTime = temp.CreateTime,
                            Remark = temp.Remark,
                            Sort = temp.Sort
                        };
            return query.FirstOrDefault();

        }

        public OperateResult<Auth_Job> Update(AuthJobParam param)
        {
            var repo = _unitOfWork.GetRepository<Auth_Job>();
            var old = repo.Find(param.Id);
            if (old == null)
            {
                return OperateResult<Auth_Job>.Error("对象不存在", old);
            }
            //判断部门是否存在
            if (!_db.Auth_Department.Any(l => l.Id == param.DepartmentId))
            {
                return OperateResult<Auth_Job>.Error("所选部门不存在");
            }
            if (_db.Auth_Job.Any(l => l.Code == param.Code && l.Id != param.Id))
            {
                return OperateResult<Auth_Job>.Error("编号重复", old);
            }
            old.Name = param.Name;
            old.Code = param.Code;
            old.DepartmentId = param.DepartmentId;
            old.Remark = param.Remark;
            old.Sort = param.Sort;
            old.ModifyTime = DateTime.Now;
            repo.Update(old);
            var sucess = _unitOfWork.SaveChanges() == 1;//提交到数据库
            return new OperateResult<Auth_Job>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", old);
        }
    }
}
