using NLog;
using System;

namespace Rokolab.BitstampClient.Logging
{
    public class NLogging : ILog
    {
        private static Logger _log;
        private string _sourceName;

        public NLogging(string sourceName)
        {
            _sourceName = sourceName;
        }

        public void Trace(string message)
        {
            SetLogger();
            _log.Trace(message);
        }

        public void Trace(string message, params object[] args)
        {
            SetLogger();
            _log.Trace(message, args);
        }

        public void TraceDebug(string message)
        {
            SetLogger();
            _log.Debug(message);
        }

        public void TraceInfo(string message)
        {
            SetLogger();
            _log.Info(message);
        }

        public void TraceError(string message, Exception exception = null)
        {
            SetLogger();
            _log.ErrorException(message, exception);
        }

        public void TraceException(string message, Exception exception)
        {
            SetLogger();
            _log.ErrorException(message, exception);
        }

        public void TraceWarn(string message)
        {
            SetLogger();
            _log.Warn(message);
        }

        private void SetLogger()
        {
            _log = LogManager.GetLogger(_sourceName);
        }
    }
}