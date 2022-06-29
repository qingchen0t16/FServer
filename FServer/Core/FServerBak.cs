using FServer.Core.Logic;
using HPSocket;
using HPSocket.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FServer.Core
{
    public class FServerBak
    {
        public static FServer I = new FServer();

        public TcpServer Server = new TcpServer();

        public void Init(ServerConf conf)
        {
            Logger.I.Info($"监听{conf.Host}:{conf.Port}...");
            Server.Address = conf.Host;// 设置服务端IP
            Server.Port = Convert.ToUInt16(conf.Port);// 设置端口
            Server.SendPolicy = SendPolicy.Direct;

            Server.OnPrepareListen += OnPrepareListen;
            Server.OnAccept += OnAccept; //连接事件
            Server.OnClose += OnClose;   //断开连接事件
            Server.OnReceive += OnReceive;// 接收数据
            Server.OnSend += OnSend;// 发送数据

            Server.Start();
        }

        /// <summary>
        /// 发送全部数据
        /// </summary>
        public void SendAll(byte[] buf)
        {
            foreach (IntPtr ptr in Server.GetAllConnectionIds())
            {
                Server.GetLocalAddress(ptr, out var ip, out var port);
                Logger.I.Info($"向 {ip}:{port} 发送信息");
                Logger.I.Info(Server.Send(ptr, buf, buf.Length) ? $"发送成功" : $"发送失败");
            }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listen"></param>
        /// <returns></returns>
        HandleResult OnPrepareListen(IServer sender, IntPtr listen)
        {
            return HandleResult.Ok;
        }

        /// <summary>
        /// 客户进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        HandleResult OnAccept(IServer sender, IntPtr connId, IntPtr client)
        {
            // 获取客户端地址
            if (!sender.GetRemoteAddress(connId, out var ip, out var port))
            {
                return HandleResult.Error;
            }

            Logger.I.Debug($"{ip}:{port} 客户进入");

            return HandleResult.Ok;
        }

        //服务器收到数据
        HandleResult OnReceive(IServer sender, IntPtr connId, byte[] data)
        {
            try
            {
                // 获取客户端地址
                if (!sender.GetRemoteAddress(connId, out var ip, out var port))
                {
                    return HandleResult.Error;
                }

                string str = Encoding.UTF8.GetString(data);
                Logger.I.Debug($"[{ip}:{port}]:{str}");
                
                string[] cmd = str.Split(' ');
                byte[] buf;
                Logger.I.Debug((cmd[0] == "GetServerStatu").ToString());
                switch (cmd[0]) {
                    case "GetServerStatu":  // 获取服务器状态
                        buf = Encoding.Default.GetBytes("ServerStatu " + FSLogic.GetServerStatu().ToString());
                        break;
                    default:
                        buf = Encoding.Default.GetBytes("Error Command");
                        break;
                }

                sender.Send(connId, buf, buf.Length);
                return HandleResult.Ok;
            }
            catch (Exception)
            {
                return HandleResult.Ignore;
            }
        }

        HandleResult OnSend(IServer sender, IntPtr connId, byte[] data)
        {
            try
            {
                return HandleResult.Ok;
            }
            catch (Exception)
            {
                return HandleResult.Ignore;
            }
        }

        /// <summary>
        /// 客户端关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connId"></param>
        /// <param name="socketOperation"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        HandleResult OnClose(IServer sender, IntPtr connId, SocketOperation socketOperation, int errorCode)
        {
            // 获取客户端地址
            if (!sender.GetRemoteAddress(connId, out var ip, out var port))
            {
                return HandleResult.Error;
            }

            Logger.I.Debug($"{ip}:{port} 客户离开");

            return HandleResult.Ok;
        }

        //服务器关闭
        HandleResult OnShutdown(IServer sender)
        {

            return HandleResult.Ok;
        }
    }
}
