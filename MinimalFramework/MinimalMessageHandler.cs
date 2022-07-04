using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Context.Propagation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingHost;

namespace MinimalFramework
{
    public abstract class MinimalMessageHandler<TMessage> : IConsumer<TMessage>
        where TMessage : class
    {
        private static readonly ActivitySource ActivitySource = new("MassTransit");

        public Task Consume(ConsumeContext<TMessage> context)
        {
            TryAddingObservabilityTrace(context);

            Handle(context.Message);

            return Task.CompletedTask;
        }

        private void TryAddingObservabilityTrace(ConsumeContext<TMessage> context)
        {
            try
            {
                var baseMessage = context.Message as MinimalMessage;

                if (baseMessage == null) return;
                var traceId = ActivityTraceId.CreateFromString(baseMessage.TraceId.AsSpan());
                var spanId = ActivitySpanId.CreateFromString(baseMessage.SpanId.AsSpan());

                ActivityContext activityContext = new ActivityContext(
                    traceId: traceId,
                    spanId: spanId,
                    traceFlags: ActivityTraceFlags.Recorded);
                var activity = ActivitySource.StartActivity(
                                $"{context.Message.GetType().Name}-Handler",
                                ActivityKind.Consumer,
                                activityContext);
            }
            catch (Exception) { 
            }
        }

        public abstract Task Handle(TMessage message);
    }
}
