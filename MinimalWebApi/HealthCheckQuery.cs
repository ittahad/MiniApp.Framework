using MediatR;
using MinimalFramework;

namespace MinimalWebApi
{
    public class HealthCheckQuery : IRequest<string>
    {
    }
}
