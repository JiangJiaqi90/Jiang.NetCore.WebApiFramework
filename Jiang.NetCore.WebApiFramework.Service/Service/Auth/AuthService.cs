using Microsoft.EntityFrameworkCore;




using Netson.HotelManage2.Service.IService.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork<ManageContext> _unitOfWork;
        private readonly ManageContext _db;
        public AuthService(IUnitOfWork<ManageContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.DbContext;
        }

        public List<Auth_Auth> GetAll()
        {
            return _db.Auth_Auth.ToList();
        }

        public IEnumerable<Auth_Auth> GetByFeatureId(Guid featureId)
        {
            return _db.Auth_Auth.Where(l => l.FeatureId == featureId).ToList();
        }

        public OperateResult<IEnumerable<Auth_Auth>> Update(FeatureAuthParam param)
        {
            //校验功能项
            if (!_db.Auth_Feature.Any(l => l.Id == param.FeatureId))
            {
                return OperateResult<IEnumerable<Auth_Auth>>.Error("功能项不存在");
            }
            //获取功能项已有的权限
            var haveList = _db.Auth_Auth.Where(l => l.FeatureId == param.FeatureId).ToList();
            //已有的字典
            var haveDic = haveList.ToDictionary(l => l.Id.ToString());
            //需要删除的Id--初始为所有
            var delList = haveList.Select(l => l.Id).ToList();
            //没有传主键的是添加，需要添加的权限
            var addList = new List<Auth_Auth>();
            //需要修改的功能项
            var updateList = new List<Auth_Auth>();
            foreach (var auth in param.Auths)
            {
                //添加
                if (auth.Id == Guid.Empty)
                {
                    auth.Id = Guid.NewGuid();
                    var obj = auth.ToEntity();
                    obj.FeatureId = param.FeatureId;
                    addList.Add(obj);
                }
                else
                {
                    if (haveDic.ContainsKey(auth.Id.ToString()))
                    {
                        //有就修改
                        //从可删除中移除
                        delList.Remove(auth.Id);
                        //执行修改
                        var old = haveDic[auth.Id.ToString()];
                        old.FeatureId = param.FeatureId;
                        old.ModifyTime = DateTime.Now;
                        old.Name = auth.Name;
                        old.Remark = auth.Remark;
                        old.ButtonId = auth.ButtonId;
                        old.Code = auth.Code;
                        old.Url = auth.Url;
                        updateList.Add(old);
                    }
                    else
                    {
                        //没有的话还是添加
                        var obj = auth.ToEntity();
                        obj.FeatureId = param.FeatureId;
                        addList.Add(obj);
                    }
                }
            }
            var repo = _unitOfWork.GetRepository<Auth_Auth>();
            //执行数据库更改
            foreach (var f in addList)
            {
                repo.Insert(f);
            }
            foreach (var f in updateList)
            {
                repo.Update(f);
            }
            foreach (var id in delList)
            {
                repo.Delete(id);
            }
            _unitOfWork.SaveChanges();
            var result = new List<Auth_Auth>();
            result.AddRange(addList);
            result.AddRange(updateList);
            return OperateResult<IEnumerable<Auth_Auth>>.Ok("成功", result);
        }
    }
}
