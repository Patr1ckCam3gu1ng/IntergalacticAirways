using System.Threading.Tasks;

namespace IntergalacticAirways.Lib.Cache.Services
{
    public interface IMemoryCacheService
    {
        Task<object> CacheResponseAsync(string cacheKey, object data);

        object GetCachedByKey(string cacheKey);
    }
}