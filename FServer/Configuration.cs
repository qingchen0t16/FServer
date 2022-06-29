using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FServer
{
    public class ServerConf
    {
        public string Host = "127.0.0.1";
        public int Port = 7788;
    }

    public class MySQLConf { 
        public string Host = "127.0.0.1";
        public int Port = 3306;
        public string User = "root",
                            Passwd = "123456789";
    }
}
