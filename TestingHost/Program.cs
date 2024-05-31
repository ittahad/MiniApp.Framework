using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniApp.Core;
using MiniApp.MongoDB;
using MinimalHost;
using OpenTelemetry.Metrics;
using Shared.Handlers;
using Shared.Sagas;
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
    //.ListenOn("TestQueue2")
    .Build(ConfigureBuilder, new Assembly?[]{ typeof(TestMessageHandler).Assembly, typeof(SubscribeToNewsletterHandler).Assembly },
        (cfg) =>
        {
            return new Type[] {
                typeof(SubscribeToNewsletterHandler),
                typeof(FinalizeOnboardingHandler),
                typeof(RevertOnboardingHandler),
                typeof(SendWelcomeEmailHandler),
                typeof(SendFollowUpEmailHandler),
                typeof(RevertSendFollowUpEmailHandler),
                typeof(RevertSendWelcomeEmailHandler),
            };
        });

builder.Run();

static void ConfigureBuilder(IHostBuilder hostBuilder)
{
    hostBuilder.ConfigureServices((ctx, services) =>
    {
        services.AddMongoDB();

        services.AddMediatR(cfg =>
        {
               cfg.RegisterServicesFromAssembly(Assembly.GetEntryAssembly()!);
        });
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(SubscribeToNewsletterHandler).Assembly);
        });

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

