using Application.Redis;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /// <inheritdoc cref="GetCachedData{T}(string)" />
        public T GetCachedData<T>(string key) 
        {
            var jsonData = _distributedCache.GetString(key);

            if (jsonData == null)
                return default(T);

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        /// <inheritdoc cref="SetCachedData{T}(string, T, TimeSpan)" />
        public void SetCachedData<T>(string key, T data, TimeSpan cacheDuration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            };

            var jsonData = JsonSerializer.Serialize(data);

            _distributedCache.SetString(key, jsonData, options);
        }

        /// <inheritdoc cref="GetAndSetDataAsync{T}(Func{Task{T}}, string, TimeSpan)" />
        public async Task<T> GetAndSetDataAsync<T>(Func<Task<T>> func, string key, TimeSpan cacheDuration)
        {
            string jsonData;

            jsonData = _distributedCache.GetString(key);

            if(jsonData is null)
            {
                var data = await func();

                jsonData = JsonSerializer.Serialize(data);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheDuration
                };

                _distributedCache.SetString(key, jsonData, options);

                return data;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        /// <inheritdoc cref="GetAndSetData{T}(Func{T}, string, TimeSpan)" />
        public T GetAndSetData<T>(Func<T> method, string key, TimeSpan cacheDuration)
        {
            string jsonData;

            jsonData = _distributedCache.GetString(key);

            if (jsonData is null)
            {
                var data = method();

                jsonData = JsonSerializer.Serialize(data);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cacheDuration
                };

                _distributedCache.SetString(key, jsonData, options);

                return data;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
