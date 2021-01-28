using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace IntergalacticAirways.Lib.Cache.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {

        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache

            
            )
        {
            _memoryCache = memoryCache;
        }

        public async Task<object> CacheResponseAsync(string cacheKey, object data)
        {
            if (data != null)
            {
                return await _memoryCache.GetOrCreateAsync(cacheKey,
                    cache =>
                    {
                        cache.SlidingExpiration = TimeSpan.FromMinutes(120);
          

                        return Task.FromResult(data);
                    });
            }

            return Task.CompletedTask;
        }

        public object GetCachedByKey(string cacheKey)
        {
            return _memoryCache.Get(cacheKey);
        }
    }
}