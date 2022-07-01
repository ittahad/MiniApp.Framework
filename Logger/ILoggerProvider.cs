using Microsoft.Extensions.Hosting;
using MinimalFramework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILoggerProvider
    {
        public void ConfigureLogger(
                IHostBuilder? builder,
                MinimalHostOptions options,
                Action<LoggerConfiguration>? config);
    }
}
