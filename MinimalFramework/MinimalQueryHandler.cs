using MediatR;

namespace MiniApp.Core
{
    public abstract class MinimalQueryHandler<TMessage, TResponse>
        : IRequestHandler<TMessage, TResponse>
        where TMessage : MinimalQuery<TResponse>
    {
        public abstract Task<TResponse> Handle(TMessage request, CancellationToken cancellationToken);
    }
}
