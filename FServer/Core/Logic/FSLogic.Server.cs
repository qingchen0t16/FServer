using FServer.Core.Enum;
using FServer.Core.Data;

namespace FServer.Core.Logic
{
    partial class FSLogic
    {
        /// <summary>
        /// 获取服务器状态
        /// </summary>
        /// <returns></returns>
        public static int GetServerStatu()
            => FSData.GetServerStatu();
    }
}
