using MinimalFramework;
using MinimalHost;
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
    OpenTelemetryOptions = new()
    {
        EnableTracing = true,
        TracingHost = "http://localhost:9411/api/v2/spans"
    }
};

var builder = new MinimalHostingBuilder(options)
    .ListenOn("TestQueue1")
    .ListenOn("TestQueue2")
    .Build(messageHandlerAssembly: typeof(TestMessageHandler).Assembly);

builder.Run();

