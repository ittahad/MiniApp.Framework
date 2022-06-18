using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public abstract class MinimalMessageHandler<TMessage> : IConsumer<TMessage>
        where TMessage : class
    {
        public Task Consume(ConsumeContext<TMessage> context) {
            Handle(context.Message);
            return Task.CompletedTask;
        }
        public abstract Task Handle(TMessage message);
    }
}
