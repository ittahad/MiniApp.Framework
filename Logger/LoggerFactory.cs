using Logger.LoggerProviders;
using Microsoft.Extensions.DependencyInjection;
using MinimalFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class LoggerFactory
    {
        MinimalHostOptions _options;

        public LoggerFactory(MinimalHostOptions options) {
            _options = options;
        }

        public ILoggerProvider CreateLogger(LoggingProviders logger)
        {
            if (logger == LoggingProviders.Serilog)
                return new SerilogProvider();

            throw new Exception("Invalid logger strategy");
        }
    }
}
