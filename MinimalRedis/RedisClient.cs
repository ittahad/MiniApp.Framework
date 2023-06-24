﻿using Microsoft.Extensions.Configuration;
using MinimalFramework;
using StackExchange.Redis;
using System.Text.Json;

namespace MinimalRedis
{
    public class RedisClient
    {
        private readonly IDatabase _database;
        private readonly ISubscriber _subscriber;

        public RedisClient(IConfiguration configuration)
        {
            string connectionString = configuration["RedisServer"];

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);
            _database = redis.GetDatabase();
            _subscriber = redis.GetSubscriber();
        }

        public long Publish(string channelName, RedisValue message)
        {
            return _subscriber.Publish(channelName, message);
        }

        public void Subscribe(string channelName, Action<string, RedisValue> action)
        {
            _subscriber.Subscribe(channelName, (ch, val) =>
            {
                action.Invoke(ch, val);
            });
        }

        public void AddString(string key, string value)
        {
            _database.StringSet(key, value);
        }

        public RedisValue GetString(string key, string value)
        {
            return _database.StringGet(key);
        }

        public void AddToSet(string key, string value)
        {
            _database.SetAdd(key, value);
        }

        public RedisValue[] GetFromSet(string key)
        {
            return _database.SetMembers(key);
        }

        public bool KeyExists(string key)
        {
            return _database.KeyExists(key);
        }

    }
    public class TestComm : MinimalCommand
    { 
        public string Data { get; set; }
    }
    public class RedisMessage
    { 
        public MinimalCommand Message {get; set; }
        public string ClassType {get; set; }
    }
}