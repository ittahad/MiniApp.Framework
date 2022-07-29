using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Context.Propagation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingHost;

namespace MinimalFramework
{
    public abstract class MinimalQueryHandler<TMessage, TResponse> 
        : IRequestHandler<TMessage, TResponse>
        where TMessage : MinimalQuery<TResponse>
    {
        public abstract Task<TResponse> Handle(TMessage request, CancellationToken cancellationToken);
    }
}
