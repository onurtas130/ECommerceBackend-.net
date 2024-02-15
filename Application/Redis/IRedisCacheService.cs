using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Redis
{
    public interface IRedisCacheService
    {
        /// <summary>
        /// gets cached data and deserializes it
        /// </summary>
        /// <typeparam name="T">data type to deserialize</typeparam>
        /// <param name="key">key of value</param>
        /// <returns></returns>
        public T GetCachedData<T>(string key);

        /// <summary>
        /// caches data
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="key">key of value</param>
        /// <param name="data">data to set</param>
        /// <param name="cacheDuration">duration time for caching</param>
        public void SetCachedData<T>(string key, T data, TimeSpan cacheDuration);

        /// <summary>
        /// gets data from cache by specified key and deserializes it. if data is not available, caches and returns data.
        /// </summary>
        /// <typeparam name="T">data type to deserialize</typeparam>
        /// <param name="method">must be return data. (make request to database or something like that)</param>
        /// <param name="key">key of value</param>
        /// <param name="cacheDuration"></param>
        /// <returns></returns>
        public Task<T> GetAndSetDataAsync<T>(Func<Task<T>>method, string key, TimeSpan cacheDuration);

        /// <summary>
        /// gets data from cache by specified key and deserializes it. if data is not available, caches and returns data.
        /// </summary>
        /// <typeparam name="T">data type to deserialize</typeparam>
        /// <param name="method">must be return data. (make request to database or something like that)</param>
        /// <param name="key">key of value</param>
        /// <param name="cacheDuration"></param>
        /// <returns></returns>
        public T GetAndSetData<T>(Func<T> method, string key, TimeSpan cacheDuration);

    }
}
