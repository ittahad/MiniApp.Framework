namespace MiniApp.Core
{
    public class MinimalHostOptions
    {
        public string[]? CommandLineArgs { get; set; }
        public bool? ConsoleLogging { get; set; }
        public bool? FileLogging { get; set; }
        public LoggingProviders LoggingProvider { get; set; } = LoggingProviders.Default;
        public SeqLoggerOptions SeqLoggerOptions { get; set; }
        public OpenTelemetrySettings? OpenTelemetryOptions { get; set; }
        public MessageDeliveryBackend? DeliverySystem { get; set; } = MessageDeliveryBackend.RabbitMq;
    }

    public class SeqLoggerOptions
    {
        public bool? UseSeq { get; set; }
        public string? SeqServerUrl { get; set; }
    }

    public class OpenTelemetrySettings
    {
        public bool EnableTracing { get; set; }
        public string? TracingHost { get; set; }
        public bool EnableMetrics { get; set; }
    }

    public enum LoggingProviders
    {
        Serilog,
        Default,
    }

}
