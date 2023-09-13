using StackExchange.Redis;

namespace MiniApp.Api
{
    public interface IRedisClient
    {
        public long Publish(string channelName, RedisValue message);

        public void Subscribe(string channelName, Action<string, RedisValue> action);

        public void AddString(string key, string value);

        public RedisValue GetString(string key, string value);

        public void AddToSet(string key, string value);

        public RedisValue[] GetFromSet(string key);

        public bool KeyExists(string key);
    }
}
