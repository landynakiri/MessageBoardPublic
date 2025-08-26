using Test.Server.Common.Application;

namespace Test.Server.Redis
{
    public class RedisConsole : INoSql
    {
        private ConnectionMultiplexer redis;
        private IDatabase db;

        internal async Task ConnectAsync()
        {
            string redisHost = "localhost";
            int redisPort = 777;
            string redisPassword = "aaa";

            ConfigurationOptions configOptions = new ConfigurationOptions
            {
                EndPoints = { $"{redisHost}:{redisPort}" },
                Password = redisPassword
            };

            redis = await ConnectionMultiplexer.ConnectAsync(configOptions);
            db = redis.GetDatabase();
        }

        internal void Disconnect()
        {
            redis?.Close();
            redis?.Dispose();
        }

        public void Add<T>(string key, T data) where T : class
        {
            db.ListRightPush(key, JsonConvert.SerializeObject(data));
        }

        public T[] Get<T>(string key)
        {
            var member = db.ListRange(key);
            T[] ts = new T[member.Length];
            for (int i = 0; i < member.Length; i++)
            {
                ts[i] = JsonConvert.DeserializeObject<T>(member[i]);
            }
            return ts;
        }
    }
}
