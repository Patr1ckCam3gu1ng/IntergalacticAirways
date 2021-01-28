using System.Threading.Tasks;

namespace IntergalacticAirways.Lib.Caches
{
    public interface IMemoryCache
    {
        Task<object> CacheResponseAsync(string cacheKey, object data);

        object GetCachedByKey(string cacheKey);
    }
}