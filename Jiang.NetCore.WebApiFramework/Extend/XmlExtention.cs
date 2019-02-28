using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Jiang.NetCore.WebApiFramework
{
    /// <summary>
    /// xml扩展
    /// </summary>
    public static class XmlExtention
    {
        /// <summary>
        /// 从控制器方法xml文档中获取方法的xml注释，构建字典key:controler_action,value:description
        /// 注意：该xml文档必须是自动生成的控制器方法的标准格式
        /// </summary>
        /// <param name="xmlFile">xml文件路径</param>
        /// <returns></returns>

        public static Dictionary<string, string> GetActionDescription(this string xmlFile)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var xmlpath = Path.Combine(basePath, xmlFile);
            if (!File.Exists(xmlpath)) //检查xml注释文件是否存在
                return null;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlpath);
            var dic = new Dictionary<string,string>();
            string memberName = string.Empty; //xml三级节点的name属性值
            string controllerName = string.Empty; //控制器完整名称
            var actionName = string.Empty;//方法名称
            string key = string.Empty; //控制器去Controller名称
            string value = string.Empty; //控制器注释
            var index = 0;//下标
            foreach (XmlNode node in xmlDoc.SelectNodes("//member")) //循环三级节点member
            {
                memberName = node.Attributes["name"].Value;
                if (memberName.StartsWith("M:")) //T:开头的代表类;M表示方法
                {
                    //M:Netson.SafeSchool.WebApi.AuthJobController.Put(Netson.SafeSchool.WebApi.AuthJobParam)
                    //有参数的情况会有干扰，截取括号前面的
                    if (memberName.Contains("("))
                    {
                        index = memberName.IndexOf("(");
                        memberName = memberName.Substring(0, index);
                    }
                    string[] arrPath = memberName.Split('.');
                    controllerName = arrPath[arrPath.Length - 2];
                    actionName = arrPath[arrPath.Length - 1];
                    if (controllerName.EndsWith("Controller")) //Controller结尾的代表控制器
                    {
                        XmlNode summaryNode = node.SelectSingleNode("summary"); //注释节点
                        key = controllerName.Remove(controllerName.Length - "Controller".Length, "Controller".Length)+"_"+actionName;
                        if (summaryNode != null && !string.IsNullOrEmpty(summaryNode.InnerText) &&!dic.ContainsKey(key))
                        {
                            value = summaryNode.InnerText.Trim();
                            dic.Add(key,value);
                        }
                    }
                }
            }

            return dic;
        }
    }
}

