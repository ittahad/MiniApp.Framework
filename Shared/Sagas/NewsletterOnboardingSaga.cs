using MassTransit;
using Shared.Entities;

namespace Shared.Sagas
{
    public class NewsletterOnboardingSaga : MassTransitStateMachine<NewsletterOnboardingSagaData>
    {
        public State Welcoming { get; set; }
        public State WelcomeFaulted { get; set; }

        public State FollowingUp { get; set; }
        public State FollowingUpFaulted { get; set; }

        public State Onboarding { get; set; }
        public State OnboardingFaulted { get; set; }

        public State Finished { get; set; }

        public Event<SubscriberCreatedEvent> SubscriberCreated { get; set; }
        public Event<Fault<SubscriberCreatedEvent>> SubscriberCreateFailed { get; set; }

        public Event<WelcomeEmailSent> WelcomeEmailSent { get; set; }
        public Event<Fault<WelcomeEmailSent>> WelcomeEmailFailed { get; set; }

        public Event<FollowUpEmailSent> FollowUpEmailSent { get; set; }
        public Event<Fault<FollowUpEmailSent>> FollowUpEmailFailed { get; set; }

        public Event<OnboardingCompleted> OnboardingDone { get; set; }
        public Event<Fault<OnboardingCompleted>> OnboardingFailed { get; set; }
        
        public Event<JobCompleted> End { get; set; }

        public NewsletterOnboardingSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => SubscriberCreated, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => SubscriberCreateFailed, e => e.CorrelateById(m => {
                return m.Message.Message.SubscriberId; 
            }));
            
            Event(() => WelcomeEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => WelcomeEmailFailed, e => e.CorrelateById(m =>
            {
                return m.Message.Message.SubscriberId;
            }));
            
            Event(() => FollowUpEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => FollowUpEmailFailed, e => e.CorrelateById(m => {
                return m.Message.Message.SubscriberId; 
            }));

            Event(() => OnboardingDone, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => OnboardingFailed, e => e.CorrelateById(m => { 
                return m.Message.Message.SubscriberId; 
            }));

            Event(() => End, e => e.CorrelateById(m => m.Message.SubscriberId));

            #region Ok path

            Initially(
                When(SubscriberCreated)
                    .Then(context =>
                    {
                        context.Saga.SubscriberId = context.Message.SubscriberId;
                        context.Saga.Email = context.Message.Email;
                    })
                    .TransitionTo(Welcoming)
                    .Publish(context => new SendWelcomeEmail(context.Message.SubscriberId, context.Message.Email))
                );

            During(Welcoming,
                When(WelcomeEmailSent)
                    .Then(context =>
                    {
                        context.Saga.WelcomeEmailSent = true;
                    })
                    .TransitionTo(FollowingUp)
                    .Publish(context => new SendFollowUpEmail(context.Message.SubscriberId, context.Message.Email)));
            
            During(FollowingUp,
                When(FollowUpEmailSent)
                    .Then(context =>
                    {
                        context.Saga.FollowUpEmailSent = true;
                        context.Saga.OnboardingCompleted = true;
                    })
                    .TransitionTo(Onboarding)
                    .Publish(context => new FinalizeOnboarding(context.Message.SubscriberId, context.Message.Email)));

            During(Onboarding,
                When(End)
                    .Finalize());

            #endregion

            #region fault region
           
            DuringAny(
                When(OnboardingFailed)
                    .TransitionTo(OnboardingFaulted)
                    .Then(ctx =>
                    {
                        ctx.Saga.OnboardingCompleted = false;
                        ctx.Publish(new RevertOnboarding(ctx.Message.Message.SubscriberId, ctx.Message.Message.Email));
                    }));

            DuringAny(
                When(FollowUpEmailFailed)
                    .TransitionTo(FollowingUpFaulted)
                    .Then(ctx =>
                    {
                        ctx.Saga.FollowUpEmailSent = false;
                        ctx.Publish(new RevertSendFollowUpEmail(ctx.Message.Message.SubscriberId, ctx.Message.Message.Email));
                    }));

            DuringAny(
                When(WelcomeEmailFailed)
                    .TransitionTo(WelcomeFaulted)
                    .Then(ctx =>
                    {
                        ctx.Saga.WelcomeEmailSent = false;
                        ctx.Publish(new RevertSendWelcomeEmail(ctx.Message.Message.SubscriberId, ctx.Message.Message.Email));
                    }));

            #endregion

        }
    }
}
