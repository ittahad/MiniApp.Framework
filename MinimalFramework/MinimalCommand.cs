using MediatR;

namespace MiniApp.Core
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
