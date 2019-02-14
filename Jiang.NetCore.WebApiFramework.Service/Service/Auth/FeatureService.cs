using Microsoft.EntityFrameworkCore;




using Jiang.NetCore.WebApiFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    public class FeatureService : IFeatureService
    {
        private readonly IUnitOfWork<ManageContext> _unitOfWork;
        private readonly ManageContext _db;
        public FeatureService(IUnitOfWork<ManageContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.DbContext;
        }

        public IEnumerable<Auth_Feature> GetByMenuId(Guid menuId)
        {
            return _db.Auth_Feature.Where(l => l.MenuId == menuId).OrderBy(l=>l.Sort).ToList();
        }

        public OperateResult<IEnumerable<Auth_Feature>> Update(AuthFeatureParam param)
        {
            //校验菜单
            if (!_db.Auth_Menu.Any(l => l.Id == param.MenuId))
            {
                return OperateResult<IEnumerable<Auth_Feature>>.Error("菜单不存在");
            }
            //获取菜单已有的功能项
            var haveList = _db.Auth_Feature.Where(l => l.MenuId == param.MenuId).ToList();
            //已有的字典
            var haveDic = haveList.ToDictionary(l => l.Id.ToString());
            //需要删除的Id--初始为所有
            var delList = haveList.Select(l => l.Id).ToList();
            //没有传主键的是添加，需要添加的功能项
            var addList = new List<Auth_Feature>();
            //需要修改的功能项
            var updateList = new List<Auth_Feature>();
            foreach(var feature in param.Features)
            {
                //添加
                if (feature.Id == Guid.Empty)
                {
                    feature.Id = Guid.NewGuid();
                    var obj = feature.ToEntity();
                    obj.MenuId = param.MenuId;
                    addList.Add(obj);
                }
                else
                {
                    if (haveDic.ContainsKey(feature.Id.ToString()))
                    {
                        //有就修改
                        //从可删除中移除
                        delList.Remove(feature.Id);
                        //执行修改
                        var old = haveDic[feature.Id.ToString()];
                        old.MenuId = param.MenuId;
                        old.ModifyTime = DateTime.Now;
                        old.Name = feature.Name;
                        old.Remark = feature.Remark;
                        old.Sort = feature.Sort;
                        updateList.Add(old);
                    }
                    else
                    {
                        //没有的话还是添加
                        var obj = feature.ToEntity();
                        obj.MenuId = param.MenuId;
                        addList.Add(obj);
                    }
                }
            }
            var repo = _unitOfWork.GetRepository<Auth_Feature>();
            //执行数据库更改
            foreach(var f in addList)
            {
                repo.Insert(f);
            }
            foreach(var f in updateList)
            {
                repo.Update(f);
            }
            foreach(var id in delList)
            {
                repo.Delete(id);
            }
            _unitOfWork.SaveChanges();
            var result = new List<Auth_Feature>();
            result.AddRange(addList);
            result.AddRange(updateList);
            return OperateResult<IEnumerable<Auth_Feature>>.Ok("成功",  result);
        }
    }
}
