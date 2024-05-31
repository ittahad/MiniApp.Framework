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
        public Event<Fault<SendWelcomeEmail>> WelcomeEmailFailed { get; set; }

        public Event<FollowUpEmailSent> FollowUpEmailSent { get; set; }
        public Event<Fault<SendFollowUpEmail>> FollowUpEmailFailed { get; set; }

        public Event<OnboardingStrated> OnboardingStarted { get; set; }
        public Event<Fault<OnboardingStrated>> OnboardingFailed { get; set; }

        public NewsletterOnboardingSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => SubscriberCreated, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => SubscriberCreateFailed, e => e.CorrelateById(m => m.Message.Message.SubscriberId));
            
            Event(() => WelcomeEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => WelcomeEmailFailed, e => e.CorrelateById(m =>
            {
                return m.Message.Message.SubsciberId;
            }));
            
            Event(() => FollowUpEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => FollowUpEmailFailed, e => e.CorrelateById(m => m.Message.Message.SubsciberId));

            Event(() => OnboardingStarted, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => OnboardingFailed, e => e.CorrelateById(m => m.Message.Message.SubscriberId));

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
                    })
                    .TransitionTo(Onboarding)
                    .Publish(context => new OnboardingStrated()
                    {
                        Email = context.Message.Email,
                        SubscriberId = context.Message.SubscriberId,
                    }));

            During(Onboarding,
                When(OnboardingStarted)
                    .Then(context =>
                    {
                        context.Saga.OnboardingCompleted = true;
                    })
                    .TransitionTo(Finished)
                    .Publish(context => new OnboardingCompleted()
                    {
                        Email = context.Message.Email,
                        SubscriberId = context.Message.SubscriberId,
                    })
                    .Finalize());

            #endregion

            #region fault region

            DuringAny(
                When(FollowUpEmailFailed)
                    .TransitionTo(FollowingUpFaulted)
                    .Then(ctx =>
                    {
                        ctx.Publish<Fault<SendFollowUpEmail>>(new { ctx.Message });
                    }));

            DuringAny(
                When(WelcomeEmailFailed)
                    .TransitionTo(WelcomeFaulted)
                    .Then(x =>
                    {
                        int a = 10;
                    }));

            #endregion

        }
    }
}
