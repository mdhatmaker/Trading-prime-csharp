using System;

namespace Rokolab.BitstampClient.Logging
{
    public interface ILog
    {
        void Trace(string message);
        void Trace(string message, params object[] args);
        void TraceDebug(string message);
        void TraceInfo(string message);
        void TraceError(string message, Exception exception = null);
        void TraceException(string message, Exception exception);
        void TraceWarn(string message);
    }
}