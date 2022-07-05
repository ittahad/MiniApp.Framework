using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;
using MinimalFramework;
using MinimalWebApi;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Meter meter = new Meter("HatCo.HatStore", "1.0.0");
Counter<int> s_hatsSold = meter.CreateCounter<int>(name: "hats-sold",
                                                            unit: "Hats",
                                                            description: "The number of hats sold in our store");

var options = new MinimalWebAppOptions
{
    UseSwagger = true,
    ConsoleLogging = true,
    FileLogging = true,
    LoggingProvider = LoggingProviders.Serilog,
    SeqLoggerOptions = new()
    {
        UseSeq = true,
        SeqServerUrl = "http://localhost:5341"
    },
    OpenTelemetryOptions = new()
    {
        EnableTracing = false,
        TracingHost = "http://localhost:9411/api/v2/spans"
    }
};

var minimalAppBuilder = new MinimalWebAppBuilder(options);

var minimalWebApp = minimalAppBuilder?.Build(builder =>
{
    builder.AddMediatorAssembly();

    //Metrics
    builder.Services.AddOpenTelemetryMetrics(options =>
    {
        options
           .AddMeter("HatCo.HatStore");

        options.AddPrometheusExporter(options =>
        {
            options.StartHttpListener = true;
            options.HttpListenerPrefixes = new string[] { $"http://localhost:9184/" };
        });
    });

    Task.Factory.StartNew(() =>
    {

        Console.WriteLine("metrics thread...");
        while (true)
        {
            // Pretend our store has a transaction each second that sells 4 hats
            Thread.Sleep(1000);
            s_hatsSold.Add(4);
        }
    });

});

minimalWebApp?.Start(app =>
{
    app.UseOpenTelemetryPrometheusScrapingEndpoint();
});

