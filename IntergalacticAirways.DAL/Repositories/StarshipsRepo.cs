using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.Lib.Caches;
using IntergalacticAirways.Lib.HttpClients;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IntergalacticAirways.DAL.Repositories
{
    public class StarshipsRepo : IStarshipsRepo
    {
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _cacheService;
        private readonly IApiHttpClient _httpClient;
        private readonly IMapper _mapper;

        public StarshipsRepo(IMemoryCache cacheService, IApiHttpClient httpClient, IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            _cacheService = cacheService;
            _httpClient = httpClient;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<List<Starship>> GetByPageIndexAsync(int pageIndex)
        {
            var cacheKey = $"{Resource.Starships}/?page={pageIndex}".ToLowerInvariant();

            if (_cacheService.GetCachedByKey(cacheKey) != null)
            {
                var starShipCaches = _cacheService.GetCachedByKey(cacheKey) as List<Starship>;

                return starShipCaches;
            }

            var starships = new List<Starship>();

            var starShips = await RequestData(cacheKey);

            if (starShips == null)
            {
                return starships;
            }

            if (starShips.Any())
            {
                starships.AddRange(starShips);
            }

            return starships;
        }

        private async Task<List<Starship>> RequestData(string cacheKey)
        {
            var maxWaitToken =
                new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds));

            var jsonResponse = await _httpClient.SendGetRequest($"{_appSettings.BaseUrl}{cacheKey}", maxWaitToken.Token);

            if (jsonResponse == null)
            {
                return null;
            }

            var starShips = JsonConvert.DeserializeObject<StarshipResponse>(jsonResponse);

            var mappedResults = _mapper.Map<List<Starship>>(starShips.Results);

            await _cacheService.CacheResponseAsync(cacheKey, mappedResults);

            return mappedResults;
        }
    }
}