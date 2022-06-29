using System.Net;
using System.Net.Sockets;
using ToastIO.API;
using ToastIO.Enum;
using ToastIO.Model;
using ToastIO.Package;

namespace FServer.Core
{
    public class FSUser
    {
        public bool IsLogin { get; private set; } = false;   // 是否登录

        public FSRequest FSR = new FSRequest();  // 创建FSR

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="server"></param>
        public FSUser(Socket server)
        {
            FSR.Request = server;   // 绑定Request
            FSR.StartListenerData();    // 启动数据监听

            // 增加数据进入异常监听
            FSR.DataEnterExListener += (request, e) => Logger.I.Info($"{request.IP}:{request.Port} : 引发异常 > {e.Message}");

            // 增加数据包解析异常监听
            FSR.PagePushFailedListener += (request, buff) => Logger.I.Info($"{request.IP}:{request.Port} : 数据包解析出现问题: " + $"数据长度:{buff.Length}");

            // 增加请求关闭监听
            FSR.RequestCloseListener += (request, str) =>
            {
                Logger.I.Info($"{request.IP}:{request.Port} : 引发异常导致客户退出 > {str}");
                FServer.Instance.RemoveClient(this);   // 用户删除
            };

            // 增加包体接收完毕监听
            FSR.EndReceiveListener += EndReceive;

            new Thread(() =>
            {
                FSR.Beat();
            }).Start();
        }

        /// <summary>
        /// 全局用户事件
        /// </summary>
        public void UserListener(string head, string content)
        {
            switch (head) {
                case "SendContent":
                    FSR.Respones.Send(FSR.Request, SendType.Object,new SendMessage {
                        Id = -1,
                        Message = content,
                        SendType = "系统"
                    },"Message");
                    break;
            }
            return;
        }

        /// <summary>
        /// 数据到达
        /// </summary>
        /// <param name="sp"></param>
        public void EndReceive(SourcePackage sp)
        {
            EndPoint? point = sp.RequestSocket.LocalEndPoint;
            Logger.I.Info($"{(point is null ? "未知客户" : (IPEndPoint)point)} : 请求 > {sp.Header}");

            switch (sp.SendType)
            {
                case SendType.Text:
                    break;
                case SendType.Object:
                    break;
            }
        }
    }
}
