using MinimalFramework;
using MinimalWebApi;

var options = new MinimalWebAppOptions
{
    UseSwagger = true,
    ConsoleLogging = true,
    FileLogging = true,
    LoggingProvider = LoggingProviders.Serilog,
    SeqLoggerOptions = new SeqLoggerOptions() {
        UseSeq = true,
        SeqServerUrl = "http://localhost:5341"
    }
};

var minimalAppBuilder = new MinimalWebAppBuilder(options);

var minimalWebApp = minimalAppBuilder?.Build(builder => {
    builder.AddMediatorAssembly();
});

minimalWebApp?.Start();
