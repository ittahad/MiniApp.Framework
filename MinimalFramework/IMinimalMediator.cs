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
            where TMessage : MinimalQuery<TResponse>;

        public Task SendAsync<TMessage>(TMessage message, string queueName)
            where TMessage : MinimalCommand;

        public Task SendToExchange<TMessage>(TMessage @event, string exchangeName)
            where TMessage : MinimalCommand;

        public Task PublishAsync<TMessage>(TMessage @event) 
            where TMessage : MinimalCommand;
    }
}
