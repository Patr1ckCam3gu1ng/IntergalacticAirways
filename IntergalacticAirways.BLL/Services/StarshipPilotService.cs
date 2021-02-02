using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.DAL.Repositories;
using IntergalacticAirways.Lib.Caches;

namespace IntergalacticAirways.BLL.Services
{
    public class StarshipPilotService : IStarshipPilotService
    {
        private readonly IMemoryCache _cacheService;
        private readonly IStarshipPilotRepo _starshipPilotRepo;

        public StarshipPilotService(IMemoryCache cacheService, IStarshipPilotRepo starshipPilotRepo)
        {
            _cacheService = cacheService;
            _starshipPilotRepo = starshipPilotRepo;
        }

        public async Task<List<StarshipPilot>> GetByStarshipId(int starshipId)
        {
            var cacheKey = $"{Resource.StarshipPilots}_{starshipId}";

            var starshipCache = _cacheService.GetCachedByKey(cacheKey);

            if (starshipCache != null)
            {
                var starships = starshipCache as List<StarshipPilot>;

                return starships;
            }

            var starshipPilots = await _starshipPilotRepo.GetByStarshipId(starshipId);

            await _cacheService.CacheResponseAsync(cacheKey, starshipPilots);

            return starshipPilots;
        }
    }
}