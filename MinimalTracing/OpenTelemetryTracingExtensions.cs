using MassTransit.Logging;
using MinimalFramework;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpentelemetryTracingExtensions
    {
        public static void AddMinimalOpenTelemetryTracing(
            this IServiceCollection serviceCollection,
            MinimalHostOptions options,
            string serviceName,
            bool isConsumer = false)
        {
            serviceCollection.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .SetSampler(new AlwaysOnSampler())
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddMassTransitInstrumentation();

                string source = isConsumer ? DiagnosticHeaders.DefaultListenerName : serviceName;

                tracerProviderBuilder
                    .AddSource(source)
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: serviceName))
                    .AddZipkinExporter(o =>
                    {
                        o.Endpoint = new Uri(options.OpenTelemetryOptions?.TracingHost 
                            ?? throw new Exception("options.OpenTelemetryOptions is required"));
                    });
            });
        }

    }
}