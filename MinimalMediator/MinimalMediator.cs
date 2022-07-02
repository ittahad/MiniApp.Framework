using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using MinimalFramework;
using System.Diagnostics;

namespace MinimalMediator
{
    public class MinimalMediator : IMinimalMediator
    {
        private readonly IMediator _mediator;
        private readonly IBus _bus;
        private readonly ILogger<MinimalMediator> _logger;
        private readonly ActivitySource _activitySource; 

        public MinimalMediator(
            IMediator mediator,
            IBus bus,
            ILogger<MinimalMediator> logger,
            ActivitySource activitySource) {
            _mediator = mediator;
            _bus = bus;
            _logger = logger;
            _activitySource = activitySource;
        }

        public Task<TResponse> PublishAsync<TEvent, TResponse>(TEvent command) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand command) where TCommand : class
        {
            var response = await _mediator.Send(command);

            return (TResponse)response;
        }

        public async Task SendToExchange<TEvent>(TEvent @event, string exchangeName) where TEvent : class
        {
            try
            {
                if (string.IsNullOrEmpty(exchangeName)) throw new ArgumentException("(exchangeName) can not be null or empty");

                var endpoint = await _bus.GetSendEndpoint(new Uri($"exchange:{exchangeName}"));

                await endpoint.Send(@event);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
            }
        }

        public async Task SendToQueue<TCommand>(TCommand command, string queueName) where TCommand : class
        {
            try
            {
                if (string.IsNullOrEmpty(queueName)) throw new ArgumentException("(queueName) can not be null or empty");

                if (typeof(MessageBase).IsAssignableFrom(command.GetType())) {
                    using var activity = Activity.Current;
                    var baseMessage = command as MessageBase;
                    baseMessage.SpanId = activity.SpanId.ToString();
                    baseMessage.TraceId = activity.TraceId.ToString();
                }

                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));

                await endpoint.Send(command);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
            }
        }
    }
}