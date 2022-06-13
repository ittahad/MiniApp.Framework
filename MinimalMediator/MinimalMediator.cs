using MediatR;
using MinimalFramework;

namespace MinimalMediator
{
    public class MinimalMediator : IMinimalMediator
    {
        private readonly IMediator _mediator;

        public MinimalMediator(IMediator mediator) {
            _mediator = mediator;
        }

        public Task<TResponse> PublishAsync<TEvent, TResponse>(TEvent command) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> SendAsync<TCommand, TResponse>(TCommand command) where TCommand : class
        {
            var response = await _mediator.Send(command);

            return (TResponse)response;
        }
    }
}