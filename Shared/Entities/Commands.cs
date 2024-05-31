namespace Shared.Entities
{
    public record SubscribeToNewsletter(string Email);

    public record SendWelcomeEmail(Guid SubsciberId, string Email);

    public record SendFollowUpEmail(Guid SubsciberId, string Email);

}
