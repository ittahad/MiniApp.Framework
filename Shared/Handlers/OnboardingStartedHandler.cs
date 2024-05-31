using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class OnboardingStartedHandler : IConsumer<OnboardingStrated>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<OnboardingStartedHandler> _logger;

        public OnboardingStartedHandler(
            IAppDbContext appDbContext, 
            ILogger<OnboardingStartedHandler> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OnboardingStrated> context)
        {
            _logger.LogInformation("+OnboardingStartedHandler");
            
            await context.Publish(new OnboardingCompleted()
            {
                SubscriberId = context.Message.SubscriberId,
                Email = context.Message.Email
            });
        }
    }
}
