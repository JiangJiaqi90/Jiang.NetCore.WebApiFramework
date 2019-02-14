using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jiang.NetCore.WebApiFramework
{
    public class JwtSettings
    {
        /// <summary>
        /// token是谁颁发的
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 给哪些客户端使用
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 加密的key
        /// </summary>
        public string SecretKey { get; set; }
    }
}
