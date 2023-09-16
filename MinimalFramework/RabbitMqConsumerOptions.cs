namespace MiniApp.Core
{
    public sealed class RabbitMqConsumerOptions
    {
        public string? ListenOnQueue { get; set; }
        public string? ListenViaExchange { get; set; }
        public int? PrefetchCount { get; set; }
    }
}
