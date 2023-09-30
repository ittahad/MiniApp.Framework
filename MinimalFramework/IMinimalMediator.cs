namespace MiniApp.Core
{
    public interface IMinimalMediator
    {
        public Task<TResponse> SendAsync<TMessage, TResponse>(TMessage message)
            where TMessage : MinimalQuery<TResponse>;

        public Task SendAsync<TMessage>(TMessage message, string queueName)
            where TMessage : MinimalCommand;

        public Task SendViaRedisAsync<TMessage>(TMessage message)
            where TMessage : RedisMessage;

        public Task SendToExchange<TMessage>(TMessage @event, string exchangeName)
            where TMessage : MinimalCommand;

        public Task PublishAsync<TMessage>(TMessage @event)
            where TMessage : MinimalCommand;
    }
}
