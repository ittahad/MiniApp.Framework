﻿using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using System.Diagnostics;

namespace MiniApp.Mediator
{
    public class MinimalMediator : IMinimalMediator
    {
        private readonly IMediator _mediator;
        private readonly IBus _bus;
        private readonly ILogger<MinimalMediator> _logger;

        public MinimalMediator(
            IMediator mediator,
            IBus bus,
            ILogger<MinimalMediator> logger)
        {
            _mediator = mediator;
            _bus = bus;
            _logger = logger;
        }

        public Task PublishAsync<TMessage>(TMessage command)
            where TMessage : MinimalCommand
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> SendAsync<TMessage, TResponse>(TMessage command)
            where TMessage : MinimalQuery<TResponse>
        {
            var response = await _mediator.Send(command);
            return response;
        }

        public async Task SendToExchange<TMessage>(TMessage @event, string exchangeName)
            where TMessage : MinimalCommand
        {
            try
            {
                if (string.IsNullOrEmpty(exchangeName)) throw new ArgumentException("(exchangeName) can not be null or empty");

                TryAddingObservabilityTrace<TMessage, bool>(@event);

                var endpoint = await _bus.GetSendEndpoint(new Uri($"exchange:{exchangeName}"));

                await endpoint.Send(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task SendAsync<TMessage>(TMessage command, string queueName)
            where TMessage : MinimalCommand
        {
            try
            {
                if (string.IsNullOrEmpty(queueName)) throw new ArgumentException("(queueName) can not be null or empty");

                TryAddingObservabilityTrace<TMessage, bool>(command);

                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));

                await endpoint.Send(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private void TryAddingObservabilityTrace<TMessage, TResponse>(TMessage command)
            where TMessage : MinimalCommand
        {
            try
            {
                if (command is MinimalCommand)
                {
                    using var activity = Activity.Current;

                    var baseMessage = command as MinimalCommand;

                    baseMessage.SpanId = activity.SpanId.ToString();
                    baseMessage.TraceId = activity.TraceId.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}