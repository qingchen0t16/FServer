
using FServer;
using FServer.Core;
using System.Text;

Logger.I.Info("尝试打开数据库...");
if (DBHelper.I.Connection("115.238.228.36", 10003, "foodshellbeta", "qingchen20020216", "foodshellbeta", "utf8", "FSDB", true))
    Logger.I.Info("数据库打开成功");

FServer.Core.FServer.Instance.Init(new()
{
    Host = "192.168.31.84",
    Port = 7788,
});
Logger.I.Info("服务器启动成功");

new Thread(() =>
{
    string cmd;

    while ((cmd = Console.ReadLine()) != null)
    {
        string[] args = cmd.Split(' ');
        if (args.Length == 0)
            continue;
        switch (args[0])
        {
            case "sendAll":
                if (args.Length != 2)
                    Logger.I.Info("错误的指令： sendAll <string>");
                else
                    FServer.Core.FServer.Instance.SendAll(args[1]);
                break;
            case "exit":
                Logger.I.Info("服务器即将关闭");
                return;
            default:
                Logger.I.Info("未知的指令");
                break;
        }
    }

}).Start();