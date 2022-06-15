
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MinimalHost
{
    public class MinimalHosting
    {
        public void Build(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddMassTransit(config =>
                    {
                        config.UsingRabbitMq((r, c) =>
                        {
                            c.Host("amqps://jvxyncsn:4oLJUAtdt8McmhhdPsjW4AnpqjexG5sQ@sparrow.rmq.cloudamqp.com/jvxyncsn");
                            c.PrefetchCount = 5;

                            c.ConfigureEndpoints(r);
                        });
                    });


                    services.AddSingleton<IBus>(p => p.GetRequiredService<IBusControl>());
                    services.AddHostedService<Worker>();
                })
                .Build().Run();
        }
    }
}