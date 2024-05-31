using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class RevertSendWelcomeEmailHandler : IConsumer<RevertSendWelcomeEmail>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<RevertSendWelcomeEmail> _logger;

        public RevertSendWelcomeEmailHandler(
            IAppDbContext appDbContext,
            ILogger<RevertSendWelcomeEmail> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<RevertSendWelcomeEmail> context)
        {
            _logger.LogInformation("❌ Rolling back welcome email sending job");

            return Task.CompletedTask;
        }
    }
}
