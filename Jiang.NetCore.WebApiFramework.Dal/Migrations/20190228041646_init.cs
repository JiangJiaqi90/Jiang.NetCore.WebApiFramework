using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Jiang.NetCore.WebApiFramework.Dal.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auth_Auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    FeatureId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Url = table.Column<string>(maxLength: 255, nullable: true),
                    ButtonId = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Auth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Feature",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    MenuId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Feature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Job",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    DepartmentId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Menu",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 255, nullable: true),
                    Remark = table.Column<string>(maxLength: 100, nullable: true),
                    MenuIcon = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_RoleFeature",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    FeatureId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_RoleFeature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_RoleUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_RoleUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    JobId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    RealName = table.Column<string>(maxLength: 20, nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    IdCard = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Telephone = table.Column<string>(maxLength: 20, nullable: true),
                    Phone = table.Column<string>(maxLength: 11, nullable: true),
                    Address = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    IsFreeze = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auth_UserLoginHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    LogoutTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_UserLoginHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_OperateLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ClientIp = table.Column<string>(maxLength: 50, nullable: true),
                    ServerIp = table.Column<string>(maxLength: 50, nullable: true),
                    RequestType = table.Column<string>(maxLength: 20, nullable: true),
                    Url = table.Column<string>(maxLength: 500, nullable: true),
                    ControllerName = table.Column<string>(maxLength: 50, nullable: true),
                    ActionName = table.Column<string>(maxLength: 50, nullable: true),
                    ResponseCode = table.Column<int>(nullable: false),
                    ActionMemo = table.Column<string>(maxLength: 255, nullable: true),
                    ResponseMessage = table.Column<string>(maxLength: 255, nullable: true),
                    Data = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_OperateLog", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Auth_Menu",
                columns: new[] { "Id", "Code", "CreateTime", "MenuIcon", "ModifyTime", "Name", "ParentId", "Remark", "Sort", "Url" },
                values: new object[] { new Guid("1e229079-e8da-4db2-ae94-3160ba229b14"), null, new DateTime(2019, 2, 28, 12, 16, 45, 256, DateTimeKind.Local).AddTicks(9980), null, new DateTime(2019, 2, 28, 12, 16, 45, 257, DateTimeKind.Local).AddTicks(711), "默认", new Guid("00000000-0000-0000-0000-000000000000"), "默认菜单", 0, "" });

            migrationBuilder.InsertData(
                table: "Auth_Role",
                columns: new[] { "Id", "Code", "CreateTime", "ModifyTime", "Name", "Remark", "Sort" },
                values: new object[] { new Guid("b7f744b4-4f18-4a56-a3a4-a8ea9933f998"), "admin", new DateTime(2019, 2, 28, 12, 16, 45, 259, DateTimeKind.Local).AddTicks(5354), new DateTime(2019, 2, 28, 12, 16, 45, 259, DateTimeKind.Local).AddTicks(5378), "超级管理员", null, 0 });

            migrationBuilder.InsertData(
                table: "Auth_RoleUser",
                columns: new[] { "Id", "CreateTime", "ModifyTime", "RoleId", "UserId" },
                values: new object[] { new Guid("365584e2-cc61-4043-b142-1dc5683d49de"), new DateTime(2019, 2, 28, 12, 16, 45, 265, DateTimeKind.Local).AddTicks(2625), new DateTime(2019, 2, 28, 12, 16, 45, 265, DateTimeKind.Local).AddTicks(2637), new Guid("b7f744b4-4f18-4a56-a3a4-a8ea9933f998"), new Guid("e133e990-e216-4273-b7d5-7720b0fc4c45") });

            migrationBuilder.InsertData(
                table: "Auth_User",
                columns: new[] { "Id", "Address", "Code", "CreateTime", "Email", "IdCard", "IsFreeze", "JobId", "ModifyTime", "Password", "Phone", "RealName", "Remark", "Sex", "Telephone", "Type", "UserName" },
                values: new object[,]
                {
                    { new Guid("e133e990-e216-4273-b7d5-7720b0fc4c45"), null, null, new DateTime(2019, 2, 28, 12, 16, 45, 259, DateTimeKind.Local).AddTicks(7785), null, null, false, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2019, 2, 28, 12, 16, 45, 259, DateTimeKind.Local).AddTicks(7788), "e10adc3949ba59abbe56e057f20f883e", null, "接口测试账号", null, 0, null, 0, "admin" },
                    { new Guid("be6b10d5-e9f3-4224-b0d4-35dfe4af582a"), null, null, new DateTime(2019, 2, 28, 12, 16, 45, 264, DateTimeKind.Local).AddTicks(847), null, null, false, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2019, 2, 28, 12, 16, 45, 264, DateTimeKind.Local).AddTicks(861), "14e1b600b1fd579f47433b88e8d85291", null, "系统管理员", null, 0, null, 0, "netson_admin" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auth_Auth");

            migrationBuilder.DropTable(
                name: "Auth_Department");

            migrationBuilder.DropTable(
                name: "Auth_Feature");

            migrationBuilder.DropTable(
                name: "Auth_Job");

            migrationBuilder.DropTable(
                name: "Auth_Menu");

            migrationBuilder.DropTable(
                name: "Auth_Role");

            migrationBuilder.DropTable(
                name: "Auth_RoleFeature");

            migrationBuilder.DropTable(
                name: "Auth_RoleUser");

            migrationBuilder.DropTable(
                name: "Auth_User");

            migrationBuilder.DropTable(
                name: "Auth_UserLoginHistory");

            migrationBuilder.DropTable(
                name: "Sys_OperateLog");
        }
    }
}
