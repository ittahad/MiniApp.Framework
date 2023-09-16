using Microsoft.Extensions.Hosting;
using Serilog;

namespace MiniApp.Core
{
    public interface ILoggerProvider
    {
        public void ConfigureLogger(
                IHostBuilder? builder,
                MinimalHostOptions options,
                Action<LoggerConfiguration>? config);
    }
}
