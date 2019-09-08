namespace Rokolab.BitstampClient.Logging
{
    public interface ILogFactory
    {
        ILog CreateLog(string logger);
    }
}