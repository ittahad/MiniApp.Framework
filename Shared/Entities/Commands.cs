namespace Shared.Entities
{
    public record SubscribeToNewsletter(string Email);

    public record SendWelcomeEmail(Guid SubsciberId, string Email);

    public record SendFollowUpEmail(Guid SubsciberId, string Email);

    public record FinalizeOnboarding(Guid SubsciberId, string Email);

    public record RevertSendWelcomeEmail(Guid SubsciberId, string Email);

    public record RevertSendFollowUpEmail(Guid SubsciberId, string Email);

    public record RevertOnboarding(Guid SubsciberId, string Email);

}
