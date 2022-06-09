using MediatR;

namespace MinimalWebApi
{
    public class HealthCheckQuery : IRequest<string>
    {
    }
}
