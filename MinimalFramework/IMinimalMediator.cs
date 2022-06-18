using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public interface IMinimalMediator
    {
        public Task<TResponse> SendAsync<TCommand, TResponse>(TCommand command) 
            where TCommand : class;

        public Task SendToQueue<TCommand>(TCommand command, string queueName)
            where TCommand : class;

        public Task SendToExchange<TEvent>(TEvent @event, string exchangeName)
            where TEvent : class;

        public Task<TResponse> PublishAsync<TEvent, TResponse>(TEvent command) 
            where TEvent : class;
    }
}
