
using MassTransit;
using MassTransit.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalFramework;
using MinimalMediator;
using Serilog;
using System.Diagnostics;
using System.Reflection;

namespace MinimalHost
{
    public class MinimalHostingBuilder
    {
        private readonly MinimalHostOptions _options;
        private readonly List<RabbitMqConsumerOptions> _consumerOptions;

        public MinimalHostingBuilder(MinimalHostOptions? options = null) 
        {
            _options = options == null ? new MinimalHostOptions()
            {
                CommandLineArgs = new string[] { },
            } : options;

            _consumerOptions = new List<RabbitMqConsumerOptions>();
        }

        public MinimalHostingApp Build(
            Action<IHostBuilder>? hostBuilder = null,
            Assembly? messageHandlerAssembly = null)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(_options.CommandLineArgs);

            builder.AddLogger(_options);

            builder.ConfigureServices((settings, services) =>
                {
                    services.AddMassTransit(config =>
                    {
                        IEnumerable<Type?>? foundHandlers = null;

                        if (messageHandlerAssembly != null)
                        {
                            foundHandlers = GetAllDescendantsOf(
                                        messageHandlerAssembly,
                                        typeof(MinimalCommandHandler<,>));

                            foreach (var foundHandler in foundHandlers)
                            {
                                config.AddConsumer(foundHandler);
                            }
                        }

                        config.UsingRabbitMq((ctx, conf) =>
                        {
                            conf.Host(settings.Configuration["RabbitMqServer"]);
                            //conf.PrefetchCount = 5;

                            foreach (var op in _consumerOptions)
                            {
                                AddQueueAndHandler(
                                    messageHandlerAssembly: messageHandlerAssembly,
                                    ctx: ctx,
                                    conf: conf,
                                    op: op,
                                    foundHandlers: foundHandlers);
                            }

                        });
                    });
                    
                    services.AddSingleton<IMinimalMediator, MinimalMediator.MinimalMediator>();
                    services.AddSingleton<IBus>(p => p.GetRequiredService<IBusControl>());
                    
                });

            if (hostBuilder != null)
                hostBuilder.Invoke(builder);

            // Tracing
            if (_options.OpenTelemetryOptions?.EnableTracing ?? false)
            {
                builder.ConfigureServices((settings, services) => {

                    var serviceName = settings.Configuration.GetSection("ServiceName").Value;

                    services.AddMinimalOpenTelemetryTracing(_options, serviceName, true);
                });
            }

            var host = builder.Build();

            return new MinimalHostingApp(_options) { 
                Host = host
            };
        }

        private void AddQueueAndHandler(
            Assembly? messageHandlerAssembly,
            IBusRegistrationContext ctx,
            IRabbitMqBusFactoryConfigurator conf,
            RabbitMqConsumerOptions op,
            IEnumerable<Type>? foundHandlers)
        {
            conf.ReceiveEndpoint(op.ListenOnQueue, e =>
            {
                if (!string.IsNullOrEmpty(op.ListenViaExchange))
                    e.Bind(op.ListenViaExchange);

                e.PrefetchCount = op.PrefetchCount.Value;

                if (messageHandlerAssembly != null)
                {
                    foreach (var foundHandler in foundHandlers)
                    {
                        e.ConfigureConsumer(ctx, foundHandler);
                    }
                }
            });
        }

        public MinimalHostingBuilder ListenOn(
            string queueName, 
            string exchangeName = null,
            int prefetch = 5) 
        {
            _consumerOptions.Add(new RabbitMqConsumerOptions {
                ListenOnQueue = queueName,
                ListenViaExchange = exchangeName,
                PrefetchCount = prefetch
            });

            return this;
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