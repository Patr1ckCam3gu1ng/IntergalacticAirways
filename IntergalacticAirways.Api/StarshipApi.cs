using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IntergalacticAirways.Api.Apis;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.Lib.HttpClients;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IntergalacticAirways.Api
{
    public class StarshipApi : IStarshipApi
    {
        private readonly IApiHttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public StarshipApi(IOptions<AppSettings> appSettings, IApiHttpClient httpClient, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<List<StarshipModel>> GetByPageIndexAsync(int pageIndex)
        {
            var starShips = await RequestData(pageIndex);

            return starShips;
        }

        private async Task<List<StarshipModel>> RequestData(int pageIndex)
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