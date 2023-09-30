namespace MiniApp.Core
{
    public abstract class RedisMessage : MinimalCommand
    {
        public string? MessageType { get; set; }
    }
}
