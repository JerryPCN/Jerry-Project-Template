using log4net;
using System;
using System.Reflection;

namespace JerryPlat.Utils.Helpers
{
    public static class LogHelper
    {
        private static ILog _Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Debug(object message)
        {
            _Log.Debug(message);
        }

        public static void Debug(object message, Exception exception)
        {
            _Log.Debug(message, exception);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            _Log.DebugFormat(format, args);
        }

        public static void DebugFormat(string format, object arg0)
        {
            _Log.DebugFormat(format, arg0);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.DebugFormat(provider, format, args);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            _Log.DebugFormat(format, arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            _Log.DebugFormat(format, arg0, arg1, arg2);
        }

        public static void Error(object message)
        {
            _Log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            _Log.Error(message, exception);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            _Log.ErrorFormat(format, args);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            _Log.ErrorFormat(format, arg0);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.ErrorFormat(provider, format, args);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            _Log.ErrorFormat(format, arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            _Log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public static void Fatal(object message)
        {
            _Log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            _Log.Fatal(message, exception);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            _Log.FatalFormat(format, args);
        }

        public static void FatalFormat(string format, object arg0)
        {
            _Log.FatalFormat(format, arg0);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.FatalFormat(provider, format, args);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            _Log.FatalFormat(format, arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            _Log.FatalFormat(format, arg0, arg1, arg2);
        }

        public static void Info(object message)
        {
            _Log.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
            _Log.Info(message, exception);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            _Log.InfoFormat(format, args);
        }

        public static void InfoFormat(string format, object arg0)
        {
            _Log.InfoFormat(format, arg0);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.InfoFormat(provider, format, args);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            _Log.InfoFormat(format, arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            _Log.InfoFormat(format, arg0, arg1, arg2);
        }

        public static void Warn(object message)
        {
            _Log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            _Log.Warn(message, exception);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            _Log.WarnFormat(format, args);
        }

        public static void WarnFormat(string format, object arg0)
        {
            _Log.WarnFormat(format, arg0);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.WarnFormat(provider, format, args);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            _Log.WarnFormat(format, arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            _Log.WarnFormat(format, arg0, arg1, arg2);
        }

        public static bool IsDebugEnabled { get { return _Log.IsDebugEnabled; } }

        public static bool IsErrorEnabled { get { return _Log.IsErrorEnabled; } }

        public static bool IsFatalEnabled { get { return _Log.IsFatalEnabled; } }

        public static bool IsInfoEnabled { get { return _Log.IsInfoEnabled; } }

        public static bool IsWarnEnabled { get { return _Log.IsWarnEnabled; } }
    }
}