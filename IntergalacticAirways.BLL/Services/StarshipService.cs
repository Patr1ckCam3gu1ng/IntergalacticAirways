using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntergalacticAirways.Api.Apis;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.DAL.Repositories;
using IntergalacticAirways.Lib.Caches;

namespace IntergalacticAirways.BLL.Services
{
    public class StarshipService : IStarshipService
    {
        private readonly IMemoryCache _cacheService;
        private readonly IStarshipApi _starshipApi;
        private readonly IStarshipsRepo _starshipsRepo;

        public StarshipService(IStarshipsRepo starshipsRepo, IStarshipApi starshipApi, IMemoryCache cacheService)
        {
            _starshipsRepo = starshipsRepo;
            _starshipApi = starshipApi;
            _cacheService = cacheService;
        }

        public async Task<List<Starship>> GetByIndexPage(int pageIndex)
        {
            var cacheKey = $"{Resource.Starships}_{pageIndex}";

            var starshipCache = _cacheService.GetCachedByKey(cacheKey);

            if (starshipCache != null)
            {
                var starships = starshipCache as List<Starship>;

                return starships;
            }

            {
                var starshipsApiResponse = await _starshipApi.GetByPageIndexAsync(pageIndex);

                await _starshipsRepo.Insert(starshipsApiResponse, pageIndex);

                var starships = await _starshipsRepo.GetByPageIndex(pageIndex);

                await _cacheService.CacheResponseAsync(cacheKey, starships);

                return starships;
            }
        }

        public List<Starship> FilterByPassengerCapacity(IEnumerable<Starship> starships, int numberOfPassengers)
        {
            return starships.Where(starship => starship.PassengerCapacity != null)
                .Where(starship =>
                    starship.PassengerCapacity >= numberOfPassengers &&
                    numberOfPassengers <= starship.PassengerCapacity
                ).ToList();
        }
    }
}