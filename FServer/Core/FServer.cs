using System.Net;
using System.Net.Sockets;

namespace FServer.Core
{
    public class FServer
    {
        public static FServer Instance = new FServer();

        public bool Open { get; protected set; } = false;   // 服务器是否已开启
        public Socket? server;   // 服务端Socket
        public Dictionary<Socket, FSUser> Clients = new Dictionary<Socket, FSUser>();   // 用户

        // 委托
        public Action<string,string>? ClientListener;  // 全局用户事件

        public void Init(ServerConf conf) {
            try
            {
                Open = true;
                Logger.I.Info("正在启动服务端...");
                Logger.I.Info("创建Socket...");
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Logger.I.Info($"监听IP {conf.Host}:{conf.Port}");
                server.Bind(new IPEndPoint(IPAddress.Parse(conf.Host), Convert.ToInt32(conf.Port)));
                server.Listen(Convert.ToInt32(100));    // 同时处理最大请求数
                Logger.I.Info("开始监听...");
                server.BeginAccept(new AsyncCallback(ClientAccepted), server);
            }
            catch (Exception)
            {
                Open = false;
                throw;
            }
        }

        /// <summary>
        /// 用户请求
        /// </summary>
        /// <param name="ar"></param>
        private void ClientAccepted(IAsyncResult ar)
        {
            try
            {
                Socket? socket = ar.AsyncState as Socket,    // 拿回服务端socket
                   client = socket.EndAccept(ar);       // 拿回客户端socket
                if (!Clients.ContainsKey(client))
                {
                    FSUser user = new FSUser(client);
                    if (ClientListener != null)
                        ClientListener("UserEnter",user.IsLogin.ToString());
                    Clients.Add(client, user);

                    user.FSR.DoBeat();

                    ClientListener += user.UserListener;   // 增加用户进入全局用户事件

                    Logger.I.Info($"用户{client.RemoteEndPoint}连入服务器");
                }
                socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);  // 等待下一个Client
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 群发数据
        /// </summary>
        /// <param name="head"></param>
        /// <param name="content"></param>
        public void SendAll(string content) {
            if (ClientListener != null)
                ClientListener("SendContent",content);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public bool RemoveClient(FSUser user)
        {
            ClientListener -= user.UserListener;
            return Clients.Remove(user.FSR.Request);
        }
    }
}
