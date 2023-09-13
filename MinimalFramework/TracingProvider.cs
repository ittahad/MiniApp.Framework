using System.Diagnostics;

namespace TestingHost
{
    public static class TracingProvider
    {
        private readonly static ActivitySource MyActivitySource = new ActivitySource("MassTransit");

        public static ActivitySource GetActivitySource() => MyActivitySource;
    }
}
