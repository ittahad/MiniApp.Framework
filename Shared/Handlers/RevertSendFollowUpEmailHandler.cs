using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class RevertSendFollowUpEmailHandler : IConsumer<RevertSendFollowUpEmail>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<RevertSendFollowUpEmailHandler> _logger;

        public RevertSendFollowUpEmailHandler(
            IAppDbContext appDbContext,
            ILogger<RevertSendFollowUpEmailHandler> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RevertSendFollowUpEmail> context)
        {
            _logger.LogInformation("❌ Rolling back follow-up tasks");

            await context.Publish<Fault<WelcomeEmailSent>>(new
            {
                Message = new WelcomeEmailSent()
                {
                    SubscriberId = context.Message.SubsciberId,
                    Email = context.Message.Email
                }
            });
        }
    }
}
