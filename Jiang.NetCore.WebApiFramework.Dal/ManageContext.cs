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
        /// <summary>
        /// 操作日志
        /// </summary>
        public DbSet<Sys_OperateLog> Sys_OperateLog { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //添加初始数据
            modelBuilder.Entity<Auth_Menu>().HasData(new Auth_Menu()
            {
                Id = new Guid("1e229079-e8da-4db2-ae94-3160ba229b14"),
                Url = "",
                Name = "默认",
                ParentId = Guid.Empty,
                Remark = "默认菜单"
            });
            //默认管理员角色
            var role = new Auth_Role()
            {
                Id = new Guid("b7f744b4-4f18-4a56-a3a4-a8ea9933f998"),
                Code = "admin",
                Name = "超级管理员"
            };
            modelBuilder.Entity<Auth_Role>().HasData(role);
            //默认账号--接口测试账号
            var user = new Auth_User()
            {
                Id = new Guid("e133e990-e216-4273-b7d5-7720b0fc4c45"),
                UserName = "admin",
                RealName = "接口测试账号",
                Password = "123456".To32MD5()
            };
            var userWeb = new Auth_User()
            {
                Id = new Guid("be6b10d5-e9f3-4224-b0d4-35dfe4af582a"),
                UserName = "netson_admin",
                RealName = "系统管理员",
                Password = "e10adc3949ba59abbe56e057f20f883e".To32MD5()
            };
            modelBuilder.Entity<Auth_User>().HasData(user);
            modelBuilder.Entity<Auth_User>().HasData(userWeb);
            var userRole = new Auth_RoleUser()
            {
                Id = new Guid("365584e2-cc61-4043-b142-1dc5683d49de"),
                RoleId = role.Id,
                UserId = user.Id
            };
            modelBuilder.Entity<Auth_RoleUser>().HasData(userRole);
        }



    }
}
