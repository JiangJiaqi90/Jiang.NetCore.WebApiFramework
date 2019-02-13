using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// 扩展工具类
    /// </summary>
    public static class CommonUtil
    {
        /// <summary>
        /// 32位md5加密
        /// UTF-8编码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string To32MD5(this string data)
        {
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            var sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                sb.Append(s[i].ToString("x2"));
            }
            return sb.ToString();

        }
        /// <summary>
        /// post提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string HttpPost(this string url, string body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            //request.ServicePoint.ConnectionLimit = 100;
            //request.Timeout = 600000;  //600秒=10分钟
            //ServicePointManager.DefaultConnectionLimit = 200;  //默认连接数
            //ServicePointManager.MaxServicePointIdleTime = 600000;  //600秒=10分钟
            //ServicePointManager.MaxServicePoints = 4;
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                    response.Dispose();
                }
                return ex.Message;
            }
        }
        /// <summary>
        /// 格式化decimal保留两位小数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static decimal Format(this decimal number)
        {
            return Math.Round(number * 100) / 100;
        }
    }
}
