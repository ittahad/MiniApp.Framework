using MinimalFramework;
using MinimalWebApi;

var options = new MinimalAppOptions
{
    UseSwagger = true,
    ConsoleLogging = true,
    FileLogging = true,
    SeqLoggerOptions = new SeqLoggerOptions() {
        UseSeq = true,
        SeqServerUrl = "http://localhost:5341"
    }
};

var minimalAppBuilder = new MinimalWebAppBuilder(options);

var minimalWebApp = minimalAppBuilder?.Build(builder => {
    builder.AddMediatorAssembly();
    builder.AddSerilog(options);
});

minimalWebApp?.Application?.MapGet("/", () =>
{
    return "Testing";
});

minimalWebApp?.Start();
