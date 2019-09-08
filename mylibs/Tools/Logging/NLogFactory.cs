using System;

namespace Tools.Logging
{
    public class NLogFactory : ILogFactory
    {
        public ILog CreateLog(string logger)
        {
            return new NLogging(logger);
        }
    }
} // end of namespace
