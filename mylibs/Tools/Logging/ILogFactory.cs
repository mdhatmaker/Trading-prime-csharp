using System;

namespace Tools.Logging
{
    public interface ILogFactory
    {
        ILog CreateLog(string logger);
    }
} // end of namespace
