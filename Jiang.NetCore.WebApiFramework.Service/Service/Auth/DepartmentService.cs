using Microsoft.EntityFrameworkCore;




using Jiang.NetCore.WebApiFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Jiang.NetCore.WebApiFramework
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork<ManageContext> _unitOfWork;
        //因为需要上下文所以用泛型
        private readonly ManageContext _db;
        public DepartmentService(IUnitOfWork<ManageContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.DbContext;
        }
        public OperateResult<Auth_Department> Add(AuthDepartmentParam param)
        {
            var obj = param.ToEntity();
            //判断同名
            if(_db.Auth_Department.Any(l => l.Code == param.Code))
            {
                return OperateResult<Auth_Department>.Error("编号有重复");
            }
            if (_db.Auth_Department.Any(l => l.Name == param.Name))
            {
                return OperateResult<Auth_Department>.Error("名称有重复");
            }
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            var repo = _unitOfWork.GetRepository<Auth_Department>();
            repo.Insert(obj);
            var sucess = _unitOfWork.SaveChanges() == 1;//提交到数据库
            return new OperateResult<Auth_Department>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", obj);

        }

        public OperateResult<Auth_Department> Delete(Guid id)
        {
            //判断部门下是否有职位
            if (_db.Auth_Job.Any(l => l.DepartmentId == id))
            {
                return OperateResult<Auth_Department>.Error("该部门下还有职位，请先删除职位");
            }
            var repo = _unitOfWork.GetRepository<Auth_Department>();
            var old = repo.Find(id);
            if(old.Code== "GJ")
            {
                return OperateResult<Auth_Department>.Error("系统预置管家职位，不允许删除");
            }
            repo.Delete(id);
            var success = _unitOfWork.SaveChanges() == 1;//提交到数据库
            if (success)
                return OperateResult<Auth_Department>.Ok();
            return OperateResult<Auth_Department>.Error("失败");
        }

        public IEnumerable<Auth_Department> GetAll()
        {
            return _db.Auth_Department.OrderBy(l=>l.Sort).ToList();
        }

        public Auth_Department GetById(Guid id)
        {
            return _unitOfWork.GetRepository<Auth_Department>().Find(id);
        }

        public OperateResult<Auth_Department> Update(AuthDepartmentParam param)
        {
            var repo = _unitOfWork.GetRepository<Auth_Department>();
            var old = GetById(param.Id);
            if (old == null)
            {
                return OperateResult<Auth_Department>.Error("对象不存在", old);
            }
            if (_db.Auth_Department.Any(l => l.Code == param.Code && l.Id != param.Id))
            {
                return OperateResult<Auth_Department>.Error("编号重复", old);
            }
            if (_db.Auth_Department.Any(l => l.Name == param.Name && l.Id != param.Id))
            {
                return OperateResult<Auth_Department>.Error("名称重复", old);
            }
            old.Name = param.Name;
            old.Code = param.Code;
            old.Remark = param.Remark;
            old.Sort = param.Sort;
            old.ModifyTime = DateTime.Now;
            repo.Update(old);
            var sucess = _unitOfWork.SaveChanges() == 1;//提交到数据库
            return new OperateResult<Auth_Department>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", old);
        }
    }
}
