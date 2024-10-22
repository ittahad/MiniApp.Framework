﻿using Microsoft.Extensions.Hosting;
using MiniApp.Core;
using MiniApp.Logger;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoggerExtension
    {
        public static void AddLogger(
            this IHostBuilder? builder, 
            MinimalHostOptions options,
            Action<LoggerConfiguration>? config = null) {

            var loggerFactory = new LoggerFactory(options);
            var strategy = loggerFactory.CreateLogger(options.LoggingProvider);

            if (strategy == null)
                return;

            strategy.ConfigureLogger(builder, options, config);

            builder.ConfigureServices(services => {
                services.AddSingleton(loggerFactory);
                services.AddSingleton(strategy);
            });
        }

    }
}