using StackExchange.Redis;
using System;
using TryRedis.API.Extensions;

namespace TryRedis.API.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _client;

        public RedisCacheService(IConfiguration configuration)
        {
           // Redisin bağlanacağı url appsettings.json "localhost:1453" olarak ayarlandı.
            var connectionString = configuration.GetSection("RedisConfiguration:ConnectionString")?.Value;

            ConfigurationOptions options = new()
            {
                EndPoints =
                {
                    connectionString
                },
                AbortOnConnectFail = false, //redise bağlanamazsa
                AsyncTimeout = 10000, //redis'e async isteklerde 10 saniyeden geç yanıt verirse timeouta düşmesi
                ConnectTimeout = 10000, //redis'e normal isteklerde 10 sn den geç yanıt verirse timeout'a düşmesi

               //TODO: bu konfigurasyonları detaylı araştır neler varmış keşfet
            };
            _client=ConnectionMultiplexer.Connect(options); //redis'e bağlanmak için yaptım
        }

        public T Get<T>(string key) where T : class
        {
            string value = _client.GetDatabase().StringGet(key);

            return value.ToObject<T>();
        }

        public string Get(string key)
        {
            return _client.GetDatabase().StringGet(key);
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            string value = await _client.GetDatabase().StringGetAsync(key);

            return value.ToObject<T>();
        }

        public void Remove(string key)
        {
            _client.GetDatabase().KeyDelete(key);
        }

        public void Set(string key, string value)
        {
            _client.GetDatabase().StringSet(key, value);

        }

        public void Set<T>(string key, T value) where T : class
        {
            _client.GetDatabase().StringSet(key, value.ToJsON());

        }

        public void Set(string key, object value, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string key, object value)
        {
            return _client.GetDatabase().StringSetAsync(key, value.ToJsON());
        }

        public Task SetAsync(string key, object value, TimeSpan expiration)
        {
            return _client.GetDatabase().StringSetAsync(key, value.ToJsON(), expiration);
        }
    }
}
