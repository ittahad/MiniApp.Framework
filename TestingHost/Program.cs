using MinimalFramework;
using MinimalHost;
using TestingHost;

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
        }, 
        messageHandlerAssembly: typeof(TestMessageHandler).Assembly);

builder.Run();

