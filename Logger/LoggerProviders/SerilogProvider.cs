using Microsoft.Extensions.Hosting;
using MiniApp.Core;
using Serilog;

namespace Logger.LoggerProviders
{
    public class SerilogProvider : ILoggerProvider
    {
        public void ConfigureLogger(
              IHostBuilder? builder,
              MinimalHostOptions options,
              Action<LoggerConfiguration>? config)
        {
            builder?.UseSerilog((ctx, loggerConfig) =>
            {
                string seriviceName = ctx.Configuration.GetSection("ServiceName").Value;
                string filePath = $"logs/" + $"{seriviceName}-{DateTime.Now.ToString("MM-dd-yy")}.log";

                if (options.ConsoleLogging.HasValue && options.ConsoleLogging.Value)
                    loggerConfig.WriteTo.Console(
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);

                if (options.ConsoleLogging.HasValue && options.ConsoleLogging.Value)
                {
                    loggerConfig.WriteTo.File(
                        path: filePath,
                        fileSizeLimitBytes: (10 * 1024 * 1024),
                        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                        retainedFileCountLimit: 30);
                }

                if (options.SeqLoggerOptions != null
                    && options.SeqLoggerOptions.UseSeq.HasValue
                    && options.SeqLoggerOptions.UseSeq.Value)
                {
                    loggerConfig.WriteTo.Seq(options.SeqLoggerOptions.SeqServerUrl ?? throw new Exception("Invalid seq server url"));
                }

                loggerConfig.ReadFrom.Configuration(ctx.Configuration);

                if (config != null)
                    config.Invoke(loggerConfig);
            });
        }
    }
}
