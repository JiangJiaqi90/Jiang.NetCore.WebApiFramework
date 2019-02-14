using Microsoft.EntityFrameworkCore;
using Jiang.NetCore.WebApiFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Jiang.NetCore.WebApiFramework
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork<ManageContext> _unitOfWork;
        private readonly ManageContext _db;
        public MenuService(IUnitOfWork<ManageContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _db = _unitOfWork.DbContext;
        }
        public OperateResult<Auth_Menu> Add(AuthMenuParam param)
        {
            //校验父级菜单是否存在
            if (!_db.Auth_Menu.Any(l => l.Id == param.ParentId))
            {
                return OperateResult<Auth_Menu>.Error("父级菜单不存在");
            }
            var obj = param.ToEntity();
            if (obj.Id == Guid.Empty)
            {
                obj.Id = Guid.NewGuid();
            }
            var repo = _unitOfWork.GetRepository<Auth_Menu>();
            repo.Insert(obj);
            var sucess = _unitOfWork.SaveChanges() == 1;
            return new OperateResult<Auth_Menu>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", obj);
        }

        public OperateResult<Auth_Menu> Delete(Guid id)
        {
            //校验是否有子菜单
            if (_db.Auth_Menu.Any(l => l.ParentId == id))
            {
                return OperateResult<Auth_Menu>.Error("存在子菜单");
            }
            var repo = _unitOfWork.GetRepository<Auth_Menu>();
            repo.Delete(id);
            var success = _unitOfWork.SaveChanges() == 1;//提交到数据库
            if (success)
                return OperateResult<Auth_Menu>.Ok();
            return OperateResult<Auth_Menu>.Error("失败");
        }

        public List<VAuthMenu> GetAll()
        {
            var query = from temp in _db.Auth_Menu
                        join parenttemp in _db.Auth_Menu on temp.ParentId equals parenttemp.Id into parentinto
                        from parent in parentinto.DefaultIfEmpty()
                        orderby temp.Sort
                        select new VAuthMenu()
                        {
                            Id = temp.Id,
                            Name = temp.Name,
                            MenuIcon = temp.MenuIcon,
                            Code = temp.Code,
                            ParentId = temp.ParentId,
                            CreateTime = temp.CreateTime,
                            ModifyTime = temp.ModifyTime,
                            ParentName = parent.Name,
                            Remark = temp.Remark,
                            Sort = temp.Sort,
                            Url = temp.Url
                        };
            return query.ToList();
        }

        public VAuthMenu GetById(Guid id)
        {
            var query = from temp in _db.Auth_Menu
                        join parenttemp in _db.Auth_Menu on temp.ParentId equals parenttemp.Id into parentinto
                        from parent in parentinto.DefaultIfEmpty()
                        where temp.Id == id
                        select new VAuthMenu()
                        {
                            Id = temp.Id,
                            Name = temp.Name,
                            MenuIcon = temp.MenuIcon,
                            Code = temp.Code,
                            ParentId = temp.ParentId,
                            CreateTime = temp.CreateTime,
                            ModifyTime = temp.ModifyTime,
                            ParentName = parent.Name,
                            Remark = temp.Remark,
                            Sort = temp.Sort,
                            Url = temp.Url
                        };
            return query.FirstOrDefault();

        }

        public VAuthMenu GetTree()
        {
            return GetTreeList().FirstOrDefault(l => l.ParentId == Guid.Empty);
        }

        public VAuthMenu GetTree(Guid userId)
        {
            //获取用户角色
            var roles = from role in _db.Auth_Role
                        join roleusertemp in _db.Auth_RoleUser on role.Id equals roleusertemp.RoleId into roleuserinto
                        from roleuser in roleuserinto.DefaultIfEmpty()
                        where roleuser.UserId == userId
                        select role;
            if (roles.Any(l => l.Code == "admin"))
            {
                //超级管理员直接全部
                return GetTree();
            }
            //获取用户的功能项菜单Id
            var roleIds = roles.Select(l => l.Id).ToList();
            //用户关联的功能项
            var features = from feature in _db.Auth_Feature
                           join rolefeaturetemp in _db.Auth_RoleFeature on feature.Id equals rolefeaturetemp.FeatureId into rolefeturninto
                           from rolefeature in rolefeturninto.DefaultIfEmpty()
                           where roleIds.Contains(rolefeature.RoleId)
                           select feature;
            //用户关联的菜单ID
            var menuIds = features.Select(l => l.MenuId).Distinct().ToList();
            if (menuIds == null|| menuIds.Count == 0)
            {
                return null;
            }
            //获取所有菜单
            var all = GetAll();
            var dic = all.ToDictionary(l=>l.Id.ToString());
            //添加树形数据字典
            var authDic = new Dictionary<string,VAuthMenu>();
            foreach(var id in menuIds)
            {
                //往上找父节点
                var cid = id.ToString();
                while (dic.ContainsKey(cid))
                {
                    //找到根节点，不找了
                    if (cid == Guid.Empty.ToString())
                    {
                        break;
                    }
                    var menu = dic[cid];
                    if (!authDic.ContainsKey(cid))
                    {
                        //不包含添加
                        authDic.Add(cid, menu);
                    }
                    cid = menu.ParentId.ToString();
                }
            }
            var list = authDic.Values.ToList();
            return CreateTree(list).FirstOrDefault(l => l.ParentId == Guid.Empty);
        }

        public VAuthMenu GetTreeAndFeatures(Guid roleId)
        {
            var menus = GetAll();
            //绑定功能项
            //获取所有功能项
            var features = _db.Auth_Feature.OrderBy(l=>l.Sort).ToList();
            //标记功能项状态
            //角色的功能项Id
            var roleFeatureIds = _db.Auth_RoleFeature.Where(l => l.RoleId == roleId).Select(l => l.FeatureId).ToList();
            //构建菜单Id,功能项列表字典，以绑定菜单功能项
            var menuDic = new Dictionary<string, List<VAuthFeature>>();
            //循环变量
            VAuthFeature vfeature;
            foreach(var feature in features)
            {
                vfeature = feature.ToModel();
                if (roleFeatureIds.Contains(feature.Id))
                {
                    vfeature.Selected = true;
                }
                if (menuDic.ContainsKey(vfeature.MenuId.ToString()))
                {
                    menuDic[vfeature.MenuId.ToString()].Add(vfeature);
                }
                else
                {
                    menuDic.Add(vfeature.MenuId.ToString(), new List<VAuthFeature>() { vfeature });
                }
            }
            //菜单绑定功能项
            foreach(var menu in menus)
            {
                if (menuDic.ContainsKey(menu.Id.ToString()))
                {
                    menu.Features = menuDic[menu.Id.ToString()];
                }
            }
            //生成树
            var list = CreateTree(menus);
            return list.Where(l => l.ParentId == Guid.Empty).FirstOrDefault();

        }

        public IEnumerable<VAuthMenu> GetTreeList()
        {
            var list = GetAll();
            return CreateTree(list);
        }

        public OperateResult<Auth_Menu> Update(AuthMenuParam param)
        {
            var repo = _unitOfWork.GetRepository<Auth_Menu>();
            var old = repo.Find(param.Id);
            if (old == null)
            {
                return OperateResult<Auth_Menu>.Error("对象不存在", old);
            }
            //校验父级菜单是否存在
            if (!_db.Auth_Menu.Any(l => l.Id == param.ParentId))
            {
                return OperateResult<Auth_Menu>.Error("父级菜单不存在");
            }
            old.Sort = param.Sort;
            old.Name = param.Name;
            old.Code = param.Code;
            old.Remark = param.Remark;
            old.ParentId = param.ParentId;
            old.Url = param.Url;
            old.MenuIcon = param.MenuIcon;
            old.ModifyTime = DateTime.Now;
            repo.Update(old);
            var sucess = _unitOfWork.SaveChanges() == 1;//提交到数据库
            return new OperateResult<Auth_Menu>(sucess ? ResultCode.OK : ResultCode.Error, sucess ? "成功" : "失败", old);
        }
        #region 封装方法
        /// <summary>
        /// 生成树形列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IEnumerable<VAuthMenu> CreateTree(List<VAuthMenu> list)
        {
            if (list == null || list.Count == 0)
            {
                return list;
            }
            foreach (var menu in list)
            {
                //绑定子菜单
                var parent = list.FirstOrDefault(l => l.Id == menu.ParentId);
                if (parent != null)
                {
                    parent.ChildMenus.Add(menu);
                }
            }
            return list;
        }
        #endregion
    }
}
