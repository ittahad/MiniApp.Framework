using MediatR;
using MinimalFramework;
using MinimalHost;
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
        b => { 
            b.ConfigureServices((ctx, services) => { 
                services.AddMediatR(Assembly.GetEntryAssembly());
                services.AddMediatR(Assembly.GetExecutingAssembly());
            });
        },
        messageHandlerAssembly: typeof(TestMessageHandler).Assembly);

  //Metrics
 //builder.Host.Services.AddOpenTelemetryMetrics(options =>
 //   {
 //       options
 //          .AddMeter("HatCo.HatStore");

 //       options.AddPrometheusExporter(options =>
 //       {
 //           options.StartHttpListener = true;
 //           options.HttpListenerPrefixes = new string[] { $"http://localhost:9090/" };
 //       });
 //   });

builder.Run();

