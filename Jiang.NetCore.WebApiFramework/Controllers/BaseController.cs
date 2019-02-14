using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jiang.NetCore.WebApiFramework
{
    [Route("api/[controller]/[action]")]
    #if DEBUG
        //[Authorize(Policy = "hotelManage")]//--仅发布版本身份验证
    #else
        [Authorize(Policy = "hotelManage")]
    #endif
    public abstract class BaseController : Controller
    {
        
    }
}
