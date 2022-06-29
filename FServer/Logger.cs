using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FServer
{
    public class Logger
    {
        public static Logger I = new Logger(log4net.LogManager.GetLogger("FServer"));

        public Logger(ILog log)
        {
            this._log = log;
        }

        private readonly log4net.ILog _log;

        // 静态代码块
        static Logger()
        {
            Console.WriteLine("Log4Net Loaded.");
            XmlConfigurator.Configure();
        }

        // 静态工具方法
        public static Logger GetLogger(Type? type)
        {
            // log4net.LogManager.GetLogger(Type type)
            // - type：System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType
            // - 含义是自动获取使用该日志的类名，然后打印显示或者存在文件内，比如名为Test.cs的类使用了日志功能，那么获取到的就是Test
            //   2022-04-21 17:09:52,836 [1] ERROR Test - a
            log4net.ILog log = log4net.LogManager.GetLogger(type);
            Logger logger = new Logger(log);
            return logger;
        }


        /// <summary>
        /// 输出信息日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Info(string message)
        {
            if (_log.IsInfoEnabled)
            {
                _log.Info(message);
            }
        }

        /// <summary>
        /// 输出调试日志
        /// </summary>
        /// <param name="message">调试信息</param>
        public void Debug(string message)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(message);
            }
        }

        /// <summary>
        /// 输出调试日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        public void Debug(Exception ex)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(ex.Message + "/r/n" + ex.Source + "/r/n" +
                          ex.TargetSite + "/r/n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="ex">错误信息</param>
        public void Error(string message)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(message);
            }
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="ex">错误信息</param>
        public void Error(Exception ex)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="ex">错误信息</param>
        public void Error(string message, Exception ex)
        {
            if (_log.IsErrorEnabled)
            {
                _log.Error(message, ex);
            }
        }
    }
}
