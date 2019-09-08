namespace Rokolab.BitstampClient.Logging
{
    public class NLogFactory : ILogFactory
    {
        public ILog CreateLog(string logger)
        {
            return new NLogging(logger);
        }
    }
}