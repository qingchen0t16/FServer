using FServer.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FServer.Core.Data
{
    partial class FSData
    {
        /// <summary>
        /// 获取服务器状态
        /// </summary>
        /// <returns></returns>
        public static int GetServerStatu()
        {
            return Convert.ToInt32(DBHelper.I.Operate("FSDB").ExecuteData("SELECT sc.Value FROM ServerConf sc WHERE sc.`Key` = 'ServerType'").ToString());
        }
    }
}
