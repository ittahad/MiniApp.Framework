
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniApp.Core;
using MiniApp.Mediator;
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
            Assembly?[] messageHandlerAssembly = null,
            Func<IBusRegistrationConfigurator, Type[]>? busConfigurator = null)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(_options.CommandLineArgs);
            
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            builder.UseEnvironment(env);
            
            builder.AddLogger(_options);

            builder.ConfigureServices((settings, services) =>
                {
                    services.AddMassTransit((IBusRegistrationConfigurator config) =>
                    {
                        var foundHandlers = new List<Type>();

                        if (messageHandlerAssembly != null)
                        {
                            foreach(var assembly in messageHandlerAssembly) 
                            {
                                var handlers = GetAllDescendantsOf(assembly!, typeof(MinimalCommandHandler<,>));

                                foreach (var foundHandler in handlers)
                                {
                                    config.AddConsumer(foundHandler);
                                    config.AddRequestClient(foundHandler);
                                }

                                foundHandlers.AddRange(handlers);
                            }
                        }

                        var externalHandlers = busConfigurator!.Invoke(config);
                        foreach(var _t in externalHandlers)
                        {
                            config.AddConsumer(_t);
                            config.AddRequestClient(_t);
                            foundHandlers.Add(_t);
                        }

                        config.UsingRabbitMq((ctx, conf) =>
                        {
                            conf.Host(settings.Configuration["RabbitMqServer"]);

                            foreach (var op in _consumerOptions)
                            {
                                AddQueueAndHandler(
                                        ctx: ctx,
                                        conf: conf,
                                        op: op,
                                        foundHandlers: foundHandlers);
                            }

                            //conf.UseInMemoryOutbox(ctx);

                        });
                     });
                    
                    services.AddHttpPolicy();
                    services.AddSingleton<IMinimalMediator, MinimalMediator>();
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

                foreach (var foundHandler in foundHandlers)
                {
                    e.ConfigureConsumer(ctx, foundHandler);
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