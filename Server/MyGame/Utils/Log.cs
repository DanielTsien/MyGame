using log4net;

namespace MyGame
{
    internal static class Log
    {
        private static ILog m_log;

        public static void Init(string name)
        {
            m_log = LogManager.GetLogger(name);
        }

        public static void Info(object message)
        {
            m_log.Info(message);
        }
        public static void InfoFormat(string format, object arg0)
        {
            m_log.InfoFormat(format, arg0);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            m_log.InfoFormat(format, arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            m_log.InfoFormat(format, arg0, arg1, arg2);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            m_log.InfoFormat(format, args);
        }


        public static void Warn(object message)
        {
            m_log.Warn(message);
        }

        public static void WarnFormat(string format, object arg0)
        {
            m_log.WarnFormat(format, arg0);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            m_log.WarnFormat(format, arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            m_log.WarnFormat(format, arg0, arg1, arg2);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            m_log.WarnFormat(format, args);
        }

        public static void Error(object message)
        {
            m_log.Error(message);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            m_log.ErrorFormat(format, arg0);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            m_log.ErrorFormat(format, arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            m_log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            m_log.ErrorFormat(format, args);
        }

        public static void Fatal(object message)
        {
            m_log.Fatal(message);
        }

        public static void FatalFormat(string format, object arg0)
        {
            m_log.FatalFormat(format, arg0);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            m_log.FatalFormat(format, arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            m_log.FatalFormat(format, arg0, arg1, arg2);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            m_log.FatalFormat(format, args);
        }
    }
}
