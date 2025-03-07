using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SanjeshP.RDC.WebFramework.Middlewares
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        // ذخیره داده‌ها در Redis
        public async Task SetDataInCacheAsync(string key, object data, TimeSpan duration)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            };

            await _cache.SetStringAsync(key, serializedData, options);
        }

        // بازیابی داده‌ها از Redis
        public async Task<T> GetDataFromCacheAsync<T>(string key)
        {
            var cachedData = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<T>(cachedData);
            }
            return default;
        }
    }

    // استفاده از Redis Cache
    //var redisService = new RedisCacheService(distributedCache);
    //await redisService.SetDataInCacheAsync("UserAccess", userAccessData, TimeSpan.FromMinutes(30));

    //var cachedData = await redisService.GetDataFromCacheAsync<List<Menu>>("UserAccess");

}
