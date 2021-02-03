using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.Lib.HttpClients;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IntergalacticAirways.Api.Apis
{
    public class StarshipApi : IStarshipApi
    {
        private readonly AppSettings _appSettings;
        private readonly IApiHttpClient _httpClient;
        private readonly IMapper _mapper;

        public StarshipApi(IOptions<AppSettings> appSettings, IApiHttpClient httpClient, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<StarshipModel>> GetByPageIndexAsync(int pageIndex)
        {
            var maxWaitToken =
                new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds));

            var url = $"{Resource.Starships}/?page={pageIndex}".ToLowerInvariant();

            var jsonResponse =
                await _httpClient.SendGetRequest($"{_appSettings.BaseUrl}{url}", maxWaitToken.Token);

            if (jsonResponse == null)
            {
                return null;
            }

            var starShips = JsonConvert.DeserializeObject<StarshipResponse>(jsonResponse);

            var mappedResults = _mapper.Map<List<StarshipModel>>(starShips.Results);

            return mappedResults;
        }
    }
}