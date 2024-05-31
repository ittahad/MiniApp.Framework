using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class SubscribeToNewsletterHandler : IConsumer<SubscribeToNewsletter>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<SubscribeToNewsletterHandler> _logger;

        public SubscribeToNewsletterHandler(IAppDbContext appDbContext, ILogger<SubscribeToNewsletterHandler> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubscribeToNewsletter> context)
        {
            _logger.LogInformation("+SubscribeToNewsletterHandler");

            var subscriber = new Subscriber()
            {
                Id = Guid.NewGuid(),
                Email = context.Message.Email,
                SubscribedOnUtc = DateTime.UtcNow
            };

            await _appDbContext.SaveItem(subscriber, "OrderDb");

            await context.Publish(new SubscriberCreatedEvent()
            {
                SubscriberId = subscriber.Id,
                Email = context.Message.Email
            });
        }
    }
}
