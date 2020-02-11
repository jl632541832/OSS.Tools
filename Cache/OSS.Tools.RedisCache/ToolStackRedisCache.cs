using OSS.Tools.Cache;
using System.Text;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace OSS.Tools.RedisCache
{
    public class ToolStackRedisCache : IToolCache
    {
        private readonly Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache _cache;

        public ToolStackRedisCache(RedisCacheOptions optionsAccessor)
        {
            _cache = new Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache(optionsAccessor);
        }

        public async Task<bool> SetAsync<T>(string key, T obj, CacheTimeOptions cacheOpt)
        {
            var text = JsonConvert.SerializeObject(obj);
            var bytes = Encoding.UTF8.GetBytes(text);

            await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = cacheOpt.AbsoluteExpirationRelativeToNow,
                SlidingExpiration               = cacheOpt.SlidingExpiration
            });
            return true;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var bytes = await _cache.GetAsync(key);
            var text  = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<T>(text);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
            return true;
        }

  
    }
}
