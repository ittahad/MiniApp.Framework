using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public abstract class MinimalCommand
    {
        public string? SpanId { get; set; }
        public string? TraceId { get; set; }
    }

    public abstract class MinimalQuery<TRes> : IRequest<TRes>
    {
        public string? SpanId { get; set; }
        public string? TraceId { get; set; }
    }
}
