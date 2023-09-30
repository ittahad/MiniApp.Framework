namespace MiniApp.Core
{

    public class TestMessage : MinimalCommand
    {
        public string Name { get; set; }
    }

    public class TestMessage2 : MinimalCommand
    {
        public string Name { get; set; }
    }
    public class TestMessage3 : MinimalCommand
    {
        public string Name { get; set; }
    }
    public class TestMessage4 : MinimalCommand
    {
        public string Name { get; set; }
    }
    public class TestMessage5 : RedisMessage
    {
        public string Name { get; set; }
    }
}
