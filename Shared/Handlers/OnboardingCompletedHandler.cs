using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class FinalizeOnboardingHandler : IConsumer<FinalizeOnboarding>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<FinalizeOnboardingHandler> _logger;

        public FinalizeOnboardingHandler(
            IAppDbContext appDbContext, 
            ILogger<FinalizeOnboardingHandler> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<FinalizeOnboarding> context)
        {
            try
            {
                _logger.LogInformation("✅ OnboardingCompletedHandler");

                //throw new Exception();

                await context.Publish(new JobCompleted()
                {
                    SubscriberId = context.Message.SubsciberId,
                    Email = context.Message.Email
                });
            }
            catch
            {
                _logger.LogError("❌ Error encountered.........");
                _logger.LogError("❌ Reverting all changes");

                await context.Publish<Fault<OnboardingCompleted>>(new
                {
                    Message = new OnboardingCompleted()
                    {
                        SubscriberId = context.Message.SubsciberId,
                        Email = context.Message.Email
                    }
                });
            }
        }
    }
}
