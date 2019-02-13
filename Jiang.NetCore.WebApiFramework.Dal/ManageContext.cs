using Microsoft.EntityFrameworkCore;
using System;

namespace Jiang.NetCore.WebApiFramework
{
    public class ManageContext : DbContext
    {
        public ManageContext() : base()
        {
            doConn = true;
        }
        private bool doConn = false;
        //https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/complex-data-model
        public ManageContext(DbContextOptions<ManageContext> options) : base(options)
        {
            //在此可对数据库连接字符串做加解密操作
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (doConn)
                optionsBuilder.UseMySql(GlobalParams.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

   
        #region 权限
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<Auth_Menu> Auth_Menu { get; set; }
        /// <summary>
        /// 功能项
        /// </summary>
        public DbSet<Auth_Feature> Auth_Feature { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public DbSet<Auth_Auth> Auth_Auth { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public DbSet<Auth_Department> Auth_Department { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public DbSet<Auth_Job> Auth_Job { get; set; }
        /// <summary>
        /// 角色功能项关联
        /// </summary>
        public DbSet<Auth_RoleFeature> Auth_RoleFeature { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public DbSet<Auth_User> Auth_User { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Auth_Role> Auth_Role { get; set; }
        /// <summary>
        /// 角色用户关联
        /// </summary>
        public DbSet<Auth_RoleUser> Auth_RoleUser { get; set; }
        /// <summary>
        /// 用户登录历史
        /// </summary>
        public DbSet<Auth_UserLoginHistory> Auth_UserLoginHistory { get; set; }

        #endregion

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }



    }
}
