using Logger.LoggerProviders;
using MiniApp.Core;

namespace MiniApp.Logger
{
    public class LoggerFactory
    {
        MinimalHostOptions _options;

        public LoggerFactory(MinimalHostOptions options)
        {
            _options = options;
        }

        public ILoggerProvider? CreateLogger(LoggingProviders logger)
        {
            if (logger == LoggingProviders.Serilog)
                return new SerilogProvider();
            else if (logger == LoggingProviders.Default)
                return null;
            else
                throw new Exception("Invalid logger strategy");
        }
    }
}
