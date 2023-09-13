using MassTransit;
using System.Diagnostics;
using TestingHost;

namespace MinimalFramework
{
    public abstract class MinimalCommandHandler<TMessage, TResponse> : IConsumer<TMessage>
        where TMessage : class
    {
        public async Task Consume(ConsumeContext<TMessage> context)
        {
            var activity = TryAddingObservabilityTrace(context);

            activity?.Start();

            _ = await Handle(context.Message);

            activity?.Stop();
        }

        private Activity? TryAddingObservabilityTrace(ConsumeContext<TMessage> context)
        {
            try
            {
                var baseMessage = context.Message as MinimalCommand;

                if (baseMessage == null) return null;

                var traceId = ActivityTraceId.CreateFromString(baseMessage.TraceId.AsSpan());
                var spanId = ActivitySpanId.CreateFromString(baseMessage.SpanId.AsSpan());

                ActivityContext activityContext = new ActivityContext(
                    traceId: traceId,
                    spanId: spanId,
                    traceFlags: ActivityTraceFlags.Recorded);

                var activity = TracingProvider.GetActivitySource().StartActivity(
                                $"{context.Message.GetType().Name}-Handler",
                                ActivityKind.Consumer,
                                activityContext);
                return activity;
            }
            catch (Exception) { 
                return null;
            }
        }

        public abstract Task<TResponse> Handle(TMessage message);
    }
}
