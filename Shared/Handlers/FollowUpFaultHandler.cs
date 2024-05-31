using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class FollowUpFaultHandler : IConsumer<Fault<FollowUpEmailSent>>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<FollowUpFaultHandler> _logger;

        public FollowUpFaultHandler(
            IAppDbContext appDbContext,
            ILogger<FollowUpFaultHandler> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<FollowUpEmailSent>> context)
        {
            _logger.LogInformation("+Rolling back follow-up");

            return Task.CompletedTask;
        }
    }
}
