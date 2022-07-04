using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public interface IMinimalMediator
    {
        public Task<TResponse> SendAsync<TMessage, TResponse>(TMessage message) 
            where TMessage : MinimalMessage;

        public Task SendToQueue<TMessage>(TMessage message, string queueName)
            where TMessage : MinimalMessage;

        public Task SendToExchange<TMessage>(TMessage @event, string exchangeName)
            where TMessage : MinimalMessage;

        public Task<TResponse> PublishAsync<TMessage, TResponse>(TMessage @event) 
            where TMessage : MinimalMessage;
    }
}
