
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalFramework;
using Serilog;
using System.Reflection;

namespace MinimalHost
{
    public class MinimalHostingBuilder
    {
        private readonly MinimalHostOptions _options;

        public MinimalHostingBuilder(MinimalHostOptions options = null) 
        {
            _options = options == null ? new MinimalHostOptions()
            {
                CommandLineArgs = new string[] { },
            } : options;
        }

        public MinimalHostingApp Build(
            Action<IHostBuilder> hostBuilder,
            Assembly messageHandlerAssembly = null)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(_options.CommandLineArgs);

            hostBuilder.Invoke(builder);

            builder.AddSerilog(_options);
            var host = builder.ConfigureServices(services =>
                {
                    services.AddMassTransit(config =>
                    {
                        IEnumerable<Type?> foundHandlers = null;

                        if (messageHandlerAssembly != null)
                        {
                            foundHandlers = GetAllDescendantsOf(
                                        messageHandlerAssembly,
                                        typeof(MinimalMessageHandler<>));

                            foreach (var foundHandler in foundHandlers)
                            {
                                config.AddConsumer(foundHandler);
                            }
                        }

                        config.UsingRabbitMq((ctx, conf) =>
                        {
                            conf.Host("amqps://jvxyncsn:4oLJUAtdt8McmhhdPsjW4AnpqjexG5sQ@sparrow.rmq.cloudamqp.com/jvxyncsn");
                            conf.PrefetchCount = 5;
                            
                            conf.ReceiveEndpoint("TestQueue1", e =>
                            {
                                if (messageHandlerAssembly != null)
                                {
                                    foreach (var foundHandler in foundHandlers)
                                    {
                                        e.ConfigureConsumer(ctx, foundHandler);
                                    }
                                }
                            });
                        });
                    });
                    services.AddSingleton<IBus>(p => p.GetRequiredService<IBusControl>());
                })
                .Build();

            return new MinimalHostingApp(_options) { 
                Host = host
            };
        }

        public static IEnumerable<Type> GetAllDescendantsOf(
            Assembly assembly,
            Type genericTypeDefinition)
        {
            IEnumerable<Type> GetAllAscendants(Type t)
            {
                var current = t;

                while (current.BaseType != typeof(object))
                {
                    yield return current.BaseType;
                    current = current.BaseType;
                }
            }

            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));

            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new ArgumentException(
                    "Specified type is not a valid generic type definition.",
                    nameof(genericTypeDefinition));

            return assembly.GetTypes()
                           .Where(t => GetAllAscendants(t).Any(d =>
                               d.IsGenericType &&
                               d.GetGenericTypeDefinition()
                                .Equals(genericTypeDefinition)));
        }
    }
}