using System;
using System.Threading;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.Lib.Caches;
using IntergalacticAirways.Lib.HttpClients;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IntergalacticAirways.DAL.Repositories
{
    public class PilotRepo : IPilotRepo
    {
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _cacheService;
        private readonly IApiHttpClient _httpClient;

        public PilotRepo(IOptions<AppSettings> appSettings, IMemoryCache cacheService, IApiHttpClient httpClient)
        {
            _cacheService = cacheService;
            _httpClient = httpClient;
            _appSettings = appSettings.Value;
        }

        public async Task<PilotDetail> SetPilotDetailByUrl(string pilotUrl)
        {
            var maxWaitToken =
                new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds));

            var jsonResponse = await _httpClient.SendGetRequest(pilotUrl, maxWaitToken.Token);

            if (jsonResponse == null)
            {
                return null;
            }

            var pilotDetail = JsonConvert.DeserializeObject<PilotDetail>(jsonResponse);

            await _cacheService.CacheResponseAsync(pilotUrl, pilotDetail);

            return pilotDetail;
        }

        public PilotDetail GetNameByPilotUrl(string pilot)
        {
            if (_cacheService.GetCachedByKey(pilot) == null)
            {
                return null;
            }

            var starShipCaches = _cacheService.GetCachedByKey(pilot) as PilotDetail;

            return starShipCaches;
        }
    }
}