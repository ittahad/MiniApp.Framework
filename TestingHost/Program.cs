﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniApp.Core;
using MinimalHost;
using OpenTelemetry.Metrics;
using System.Diagnostics;
using System.Reflection;
using TestingHost;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var options = new MinimalHostOptions
{
    ConsoleLogging = true,
    FileLogging = true,
    LoggingProvider = LoggingProviders.Serilog,
    SeqLoggerOptions = new SeqLoggerOptions()
    {
        UseSeq = true,
        SeqServerUrl = "http://localhost:5341"
    },
    OpenTelemetryOptions = new()
    {
        EnableTracing = true,
        TracingHost = "http://localhost:9411/api/v2/spans"
    }
};

var builder = new MinimalHostingBuilder(options)
    .ListenOn("TestQueue1")
    .ListenOn("TestQueue2")
    .Build(
        hostBuilder: ConfigureBuilder,
        messageHandlerAssembly: typeof(TestMessageHandler).Assembly);

builder.Run();

static void ConfigureBuilder(IHostBuilder hostBuilder)
{
    hostBuilder.ConfigureServices((ctx, services) =>
    {
        services.AddMediatR(Assembly.GetEntryAssembly()!);
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddOpenTelemetryMetrics(options =>
           {
               options.AddPrometheusExporter(options =>
               {
                   options.StartHttpListener = true;
                   options.HttpListenerPrefixes = new string[] { $"http://localhost:9090/" };
               });
           });
    });
}

