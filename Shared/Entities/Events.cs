using MassTransit;
using System.Runtime.CompilerServices;

namespace Shared.Entities
{
    public class SubscriberCreatedEvent
    {  

        public Guid SubscriberId { get; init; }
        public string Email { get; init; }
        
        [ModuleInitializer]
        internal static void Init()
        {
            GlobalTopology.Send.UseCorrelationId<SubscriberCreatedEvent>(x => x.SubscriberId);
        }
    }

    public class WelcomeEmailSent
    {
        public Guid SubscriberId { get; init; }
        public string Email { get; init; }
    }

    public class FollowUpEmailSent
    {
        public Guid SubscriberId { get; init; }
        public string Email { get; init; }
    }
    
    public class OnboardingCompleted
    {
        public Guid SubscriberId { get; init; }
        public string Email { get; init; }
    }

    public class JobCompleted
    {
        public Guid SubscriberId { get; init; }
        public string Email { get; init; }
    }
}
