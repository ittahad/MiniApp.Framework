namespace MiniApp.Core
{
    public enum MessageDeliveryBackend
    {
        /// <summary>
        /// Uses mass-transit with rabbitMq
        /// </summary>
        RabbitMq,
        /// <summary>
        /// Uses redis for mediator backend
        /// </summary>
        Redis
    }
}
