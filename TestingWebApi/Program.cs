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

var options = new MinimalWebAppOptions
{
    UseSwagger = true,
    ConsoleLogging = true,
    FileLogging = true,
    LoggingProvider = LoggingProviders.Serilog,
    SeqLoggerOptions = new() {
        UseSeq = true,
        SeqServerUrl = "http://localhost:5341"
    },
    OpenTelemetryOptions = new() { 
        EnableTracing = true,
        TracingHost = "http://localhost:9411/api/v2/spans"
    }
};

var minimalAppBuilder = new MinimalWebAppBuilder(options);

var minimalWebApp = minimalAppBuilder?.Build(builder => {
    builder.AddMediatorAssembly();

    #region Metrics pending
    //Metrics
    /* builder.Services.AddOpenTelemetryMetrics(options =>
     {
         options
            .AddMeter(meter.Name)
            .AddRuntimeInstrumentation()
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation();

         options.AddPrometheusExporter(options =>
         {
             options.StartHttpListener = true;
             options.HttpListenerPrefixes = new string[] { $"http://localhost:{9090}/" };
             options.ScrapeResponseCacheDurationMilliseconds = 0;
         });
     });

     var process = Process.GetCurrentProcess();
     meter.CreateObservableCounter("thread.cpu_time", () => GetThreadCpuTime(process), "ms");


     Counter<double> Counter = meter.CreateCounter<double>("myCounter", description: "A counter for demonstration purpose.");

     using var token = new CancellationTokenSource();
     Task.Run(() =>
     {
         while (!token.IsCancellationRequested)
         {
             Counter.Add(9.9, new("name", "apple"), new("color", "red"));
             Counter.Add(99.9, new("name", "lemon"), new("color", "yellow"));
             Task.Delay(10).Wait();
         }
     });*/

    //var requestCounter = meter.CreateCounter<int>("compute_requests");
    //builder.Services.AddSingleton(requestCounter);

    //builder.Services.AddSingleton(meter);
    #endregion

});

#region Metrics Pending
/*minimalWebApp?.Application?.MapGet("/hello", (ActivitySource MyActivitySource) =>
{
// Track work inside of the request
    using var activity = MyActivitySource.StartActivity("SayHello");    
    activity?.SetTag("foo", 1);
    activity?.SetTag("bar", "Hello, World!");
    activity?.SetTag("baz", new int[] { 1, 2, 3 });

    return "Hello, World!";
});*/
#endregion

minimalWebApp?.Start(app => { 
    //app.UseOpenTelemetryPrometheusScrapingEndpoint();
});

#region Metrics Pending
/*IEnumerable<Measurement<double>> GetThreadCpuTime(Process process)
{
    foreach (ProcessThread thread in process.Threads)
    {
        yield return new(thread.TotalProcessorTime.TotalMilliseconds, new("ProcessId", process.Id), new("ThreadId", thread.Id));
    }
}*/
#endregion