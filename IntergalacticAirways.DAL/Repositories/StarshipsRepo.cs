using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.Lib.Cache.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IntergalacticAirways.DAL.Repositories
{
    public class StarshipsRepo : IStarshipsRepo
    {
        private readonly AppSettings _appSettings;
        private readonly IMemoryCacheService _cacheService;
        private readonly IMapper _mapper;

        public StarshipsRepo(IOptions<AppSettings> appSettings, IMemoryCacheService cacheService, IMapper mapper)
        {
            _cacheService = cacheService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<List<Starship>> GetAll(int pageIndex)
        {
            var cacheKey = $"{Resource.Starships}/?page={pageIndex}".ToLowerInvariant();

            if (_cacheService.GetCachedByKey(cacheKey) != null)
            {
                var starShipCaches = _cacheService.GetCachedByKey(cacheKey) as List<Starship>;

                return starShipCaches;
            }

            var starships = new List<Starship>();

            var starShips = await RequestData(cacheKey);

            if (starShips != null)
            {
                if (starShips.Any())
                {
                    starships.AddRange(starShips);
                }
            }

            return starships;
        }

        private async Task<List<Starship>> RequestData(string cacheKey)
        {
            var apiEndpoint = $"{_appSettings.BaseUrl}{cacheKey}".ToLowerInvariant();

            using var client = new HttpClient();

            client.DefaultRequestHeaders
                .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var maxWaitToken =
                new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds));

            var response = await client.GetAsync(apiEndpoint, maxWaitToken.Token);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync(maxWaitToken.Token);

                var starShips = JsonConvert.DeserializeObject<StarshipResponse>(jsonString);

                var mappedResults = _mapper.Map<List<Starship>>(starShips.Results);

                await _cacheService.CacheResponseAsync(cacheKey, mappedResults);

                return mappedResults;
            }

            return null;
        }
    }
}