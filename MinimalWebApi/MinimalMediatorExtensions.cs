using MediatR;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MinimalMediatorExtensions
    {
        public static void AddMediatorAssembly(this WebApplicationBuilder? builder) 
        {
            var executingAssembly = Assembly.GetEntryAssembly();
            builder.Services.AddMediatR(executingAssembly);
        }
    }
}
