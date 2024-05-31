using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class SendFollowUpEmailHandler : IConsumer<SendFollowUpEmail>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<SendFollowUpEmailHandler> _logger;

        public SendFollowUpEmailHandler(
            IAppDbContext appDbContext, 
            ILogger<SendFollowUpEmailHandler> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendFollowUpEmail> context)
        {
            try
            {
                _logger.LogInformation("✅ SendFollowUpEmailHandler");
                
                await context.Publish(new FollowUpEmailSent()
                {
                    SubscriberId = context.Message.SubsciberId,
                    Email = context.Message.Email
                });
            }
            catch
            {
                await context.Publish<Fault<FollowUpEmailSent>>(new
                {
                    Message = new FollowUpEmailSent()
                    {
                        SubscriberId = context.Message.SubsciberId,
                        Email = context.Message.Email
                    }
                });
            }
        }
    }
}
