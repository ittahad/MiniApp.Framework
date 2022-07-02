using Microsoft.Extensions.DependencyInjection;
using MinimalFramework;
using MinimalHost;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
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
};

var builder = new MinimalHostingBuilder(options)
    .ListenOn("TestQueue1")
    .ListenOn("TestQueue2")
    .Build(
        hostBuilder: (conf) => {
            
            conf.ConfigureServices((ctx, services) =>
            {
                var serviceName = ctx.Configuration.GetSection("ServiceName").Value;
                var serviceVersion = "1.0.0";
                
                services.AddOpenTelemetryTracing(tracerProviderBuilder =>
                {
                    tracerProviderBuilder
                        .SetSampler(new AlwaysOnSampler())
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddMassTransitInstrumentation();

                    tracerProviderBuilder
                        .AddSource("MassTransit")
                        .SetResourceBuilder(
                            ResourceBuilder.CreateDefault()
                                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                        .AddZipkinExporter(o =>
                        {
                            o.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                        });
                });
            });
        }, 
        messageHandlerAssembly: typeof(TestMessageHandler).Assembly);

builder.Run();

