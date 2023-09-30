using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using MiniApp.Api;
using MinimalHost;
using System.Reflection;
using System.Text.Json;

namespace MiniApp.Core
{
    public static class RedisMessageHandlerExtensions
    {
        public static void RegisterRedisMessages(this IServiceCollection serviceCollection, Assembly asm)
        {
            var type = typeof(RedisMessage);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            var handlerTypes = MinimalHostingBuilder.GetAllDescendantsOf(asm, typeof(MinimalCommandHandler<,>));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var redisClient = serviceProvider.GetRequiredService<IRedisClient>();

            foreach (var _type in types)
            {
                redisClient.Subscribe(_type.FullName!, (ch, valFromRedis) =>
                {
                    var msg = JsonSerializer.Deserialize<JsonElement>(valFromRedis!);

                    var _t = msg.GetString(nameof(RedisMessage.MessageType));

                    var targetHandlerType = handlerTypes.FirstOrDefault(x => x.BaseType!.GetGenericArguments().Any(y => y.FullName == _t));

                    var cHandler = serviceProvider.GetService(targetHandlerType!);

                    var __messageType = AppDomain.CurrentDomain.GetAssemblies()
                .       SelectMany(s => s.GetTypes()).FirstOrDefault(x => x.FullName == _t!);

                    var targetObj = JsonSerializer.Deserialize(valFromRedis!, __messageType!);

                    object? task = (Task?)cHandler!.GetType().GetMethod("Handle")!.Invoke(
                        cHandler!,
                        new[] { targetObj! });

                    _ = task!.GetType()!.GetProperty("Result")!.GetValue(task);
                });
            }
        }
    }
}
