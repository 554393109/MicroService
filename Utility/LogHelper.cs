using System;
using System.Collections.Concurrent;
using log4net;

namespace Utility
{
    public class LogHelper
    {
        private static readonly ConcurrentDictionary<Type, log4net.ILog> Loggers = new ConcurrentDictionary<Type, log4net.ILog>();

        /// <summary>
        /// 获取记录器
        /// </summary>
        /// <param name="source">soruce</param>
        /// <returns></returns>
        private static ILog GetLogger(Type source)
        {
            if (Loggers.ContainsKey(source))
            {
                return Loggers[source];
            }
            else
            {
                //ILog logger = LogManager.GetLogger("logerror", source);
                ILog logger = LogManager.GetLogger("log4net-default-repository", source);

                Loggers.TryAdd(source, logger);
                return logger;
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void Error(dynamic source, string message)
        {
            var logger = GetLogger(source.GetType());
            if (logger.IsErrorEnabled)
                logger.Error(message);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(dynamic source, string message)
        {
            var logger = GetLogger(source.GetType());
            if (logger.IsInfoEnabled)
                logger.Info(message);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(dynamic source, string message)
        {
            var logger = GetLogger(source.GetType());
            if (logger.IsDebugEnabled)
                logger.Debug(message);
        }
    }
}