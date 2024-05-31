using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class RevertOnboardingHandler : IConsumer<RevertOnboarding>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<RevertOnboardingHandler> _logger;

        public RevertOnboardingHandler(
            IAppDbContext appDbContext,
            ILogger<RevertOnboardingHandler> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RevertOnboarding> context)
        {
            _logger.LogInformation("❌ Rolling back onboarding tasks");

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
