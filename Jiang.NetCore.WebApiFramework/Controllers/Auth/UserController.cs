using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jiang.NetCore.WebApiFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Netson.HotelManage2.WebApi
{
    /// <summary>
    /// 账户管理
    /// </summary>
    public class UserController : BaseController
    {
        private JwtSettings _jwtSettings;
        private IUserService _service;
        public UserController(IOptions<JwtSettings> _jwtSettingsAccesser, IUserService service)
        {
            _jwtSettings = _jwtSettingsAccesser.Value;
            _service = service;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<OperateResult<LoginUserModel>> Login([FromBody]LoginViewModel viewModel)
        {
            var result = _service.LoginCheck(viewModel.UserName, viewModel.Password);

            if (result.Code == ResultCode.OK)
            {
                var model = (LoginUserModel)(result.Content);
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

                DateTime authTime = DateTime.UtcNow;
                //DateTime expiresAt = authTime.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

                //将用户信息添加到 Claim 中
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, model.User.Id.ToString())
                };
                //添加角色
                foreach (var role in model.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Code));
                }
                identity.AddClaims(claims);

                //签发一个加密后的用户信息凭证，用来标识用户的身份
                //_httpContextAccessor.HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),//创建声明信息
                    Issuer = _jwtSettings.Issuer,//Jwt token 的签发者
                    Audience = _jwtSettings.Audience,//Jwt token 的接收者
                    //Expires = expiresAt,//过期时间
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)//创建 token
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                model.Token = new JwtSecurityTokenHandler().WriteToken(token);
                result.Content = model;
                return Json(result);
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public ActionResult<OperateResult<Auth_UserLoginHistory>> Logout()
        {
            var userId = new Guid(User.Identity.Name);
            return Json(_service.Logout(userId));
        }
        /// <summary>
        /// 获取员工类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<IEnumerable<KeyValuePair<int, string>>>> GetUserTypes()
        {
            return Json(new OperateResult<IEnumerable<KeyValuePair<int, string>>>(_service.GetUserTypes()));
        }
        /// <summary>
        /// 获取销售员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<IEnumerable<Auth_User>>> GetSalesman()
        {
            return Json(new OperateResult<IEnumerable<Auth_User>>(_service.GetSalesman()));
        }
        /// <summary>
        /// 添加 员工
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperateLogAttribute))]
        [HttpPost]
        public ActionResult<OperateResult<Auth_User>> Post([FromBody]UserParam param)
        {
            return Json(_service.Add(param));
        }
        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperateLogAttribute))]
        [HttpPut]
        public ActionResult<OperateResult<Auth_User>> Put([FromBody]UserParam param)
        {
            return Json(_service.Update(param));
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperateLogAttribute))]
        [HttpPut]
        public ActionResult<OperateResult<Auth_User>> UpdatePwd([FromBody]UserUpdatePassword param)
        {
            var userId = new Guid(User.Identity.Name);
            return Json(_service.UpdatePassword(param, userId));
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperateLogAttribute))]
        [HttpPut]
        public ActionResult<OperateResult<Auth_User>> ResetPwd([FromBody]UserResetPassword param)
        {
            return Json(_service.ResetPassword(param));
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public ActionResult<OperateResult<Auth_User>> FindPwd([FromBody]UserFindPassword param)
        {
            return Json(_service.FindPassword(param));
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceFilter(typeof(OperateLogAttribute))]
        [HttpDelete("{id}")]
        public ActionResult<OperateResult<Auth_User>> Delete(Guid id)
        {
            return Json(_service.Delete(id));
        }
        /// <summary>
        /// 根据Id查询员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<OperateResult<VUser>> Get(Guid id)
        {
            return Json(new OperateResult<VUser>(_service.GetById(id)));
        }
        /// <summary>
        /// 分页查询员工
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">页码：从0开始</param>
        /// <param name="userName">用户名</param>
        /// <param name="realName">姓名</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="jobId">职位id</param>
        /// <returns></returns>
        [HttpGet("{pageIndex}/{pageSize}")]
        public ActionResult<OperateResult<IPagedList<VUser>>> GetPage(int pageSize, int pageIndex, Guid? departmentId, Guid? jobId,
            string userName = null, string realName = null)
        {
            return Json(new OperateResult<IPagedList<VUser>>(_service.GetPage(pageSize, pageIndex, userName, realName, departmentId, jobId)));
        }

        /// <summary>
        /// 查询房务排班员工
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="realName">姓名</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="jobId">职位id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<List<VUsers>>> GetList(Guid? departmentId, Guid? jobId, string userName = null, string realName = null)
        {
            return Json(new OperateResult<List<VUsers>>(_service.GetList(userName, realName, departmentId, jobId)));
        }
        /// <summary>
        /// 用户绑定角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<OperateResult<IEnumerable<Auth_RoleUser>>> BindRoles([FromBody]UserBindRolesParam param)
        {
            return Json(_service.BindRoles(param));
        }
        /// <summary>
        /// 根据角色获取用户
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<IEnumerable<VUser>>> GetByRole(Guid roleId)
        {
            return Json(new OperateResult<IEnumerable<VUser>>(_service.GetByRole(roleId)));
        }
        /// <summary>
        /// 获取查房人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<IEnumerable<VUser>>> GetCheckRoom()
        {
            return Json(new OperateResult<IEnumerable<VUser>>(_service.GetCheckRoom()));
        }
        /// <summary>
        /// 获取复审人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OperateResult<IEnumerable<VUser>>> GetRecheckRoom()
        {
            return Json(new OperateResult<IEnumerable<VUser>>(_service.GetRecheckRoom()));
        }

    }
}
