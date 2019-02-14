
using System;
using System.Collections.Generic;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 菜单页面模型
    /// </summary>
    public class VAuthMenu:Auth_Menu
    {
        /// <summary>
        /// 父级菜单名称
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<VAuthMenu> ChildMenus { get; set; } = new List<VAuthMenu>();
        /// <summary>
        /// 关联的功能项列表
        /// </summary>
        public List<VAuthFeature> Features { get; set; } = new List<VAuthFeature>();
    }
    /// <summary>
    /// 功能项视图模型
    /// </summary>
    public class VAuthFeature: Auth_Feature
    {
        /// <summary>
        /// 是否被某角色绑定
        /// </summary>
        public bool Selected { get; set; }
    }
    /// <summary>
    /// 实体扩展方法
    /// </summary>
    public static class EntityExtentions
    {
        /// <summary>
        /// 功能项实体转模型
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static VAuthFeature ToModel(this Auth_Feature feature)
        {
            return new VAuthFeature()
            {
                Id = feature.Id,
                Code = feature.Code,
                CreateTime = feature.CreateTime,
                MenuId = feature.MenuId,
                ModifyTime = feature.ModifyTime,
                Name = feature.Name,
                Remark = feature.Remark,
                Sort = feature.Sort
            };
        }
    }
}
