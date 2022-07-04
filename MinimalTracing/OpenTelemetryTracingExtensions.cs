using Microsoft.Extensions.DependencyInjection;
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
            string serviceName)
        {
            serviceCollection.AddOpenTelemetryTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .SetSampler(new AlwaysOnSampler())
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddMassTransitInstrumentation();

                tracerProviderBuilder
                    .AddSource(serviceName)
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName: serviceName))
                    .AddZipkinExporter(o =>
                    {
                        o.Endpoint = new Uri(options.OpenTelemetryOptions?.TracingHost 
                            ?? throw new Exception("options.OpenTelemetryOptions is required"));
                    });
            });
        }

    }
}