using MediatR;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MinimalMediatorExtensions
    {
        public static void AddMediatorAssembly(this WebApplicationBuilder? builder) 
        {
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
