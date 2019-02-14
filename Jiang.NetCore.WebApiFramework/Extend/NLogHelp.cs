using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jiang.NetCore.WebApiFramework
{
    public class NLogHelp
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        public static void ErrorLog(Exception ex)
        {
            logger.Error(ex);
        }
        public static void InfoLog(string operateMsg)
        {
            logger.Info(operateMsg);
        }
    }
}
