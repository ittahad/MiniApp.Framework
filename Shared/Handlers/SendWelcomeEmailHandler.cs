using MassTransit;
using Microsoft.Extensions.Logging;
using MiniApp.Core;
using Shared.Entities;

namespace Shared.Handlers
{
    public class SendWelcomeEmailHandler : IConsumer<SendWelcomeEmail>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly ILogger<SendWelcomeEmailHandler> _logger;

        public SendWelcomeEmailHandler(
            IAppDbContext appDbContext, 
            ILogger<SendWelcomeEmailHandler> logger
            )
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
        {
            try
            {
                _logger.LogInformation("✅ SendWelcomeEmailHandler");

                await context.Publish(new WelcomeEmailSent()
                {
                    SubscriberId = context.Message.SubsciberId,
                    Email = context.Message.Email
                });
            }
            catch
            {
                
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
}
