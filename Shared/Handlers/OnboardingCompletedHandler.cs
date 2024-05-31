using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class OnboardingCompletedHandler : IConsumer<OnboardingCompleted>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<OnboardingCompletedHandler> _logger;

        public OnboardingCompletedHandler(
            IAppDbContext appDbContext, 
            ILogger<OnboardingCompletedHandler> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OnboardingCompleted> context)
        {
            _logger.LogInformation("+OnboardingCompletedHandler");
            
            return Task.CompletedTask;
        }
    }
}
