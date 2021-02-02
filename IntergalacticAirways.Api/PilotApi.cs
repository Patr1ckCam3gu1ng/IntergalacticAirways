using System;
using System.Threading;
using System.Threading.Tasks;
using IntergalacticAirways.Api.Apis;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.Lib.HttpClients;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IntergalacticAirways.Api
{
    public class PilotApi : IPilotApi
    {
        private readonly IApiHttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public PilotApi(IOptions<AppSettings> appSettings, IApiHttpClient httpClient)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
        }

        public async Task<PilotModel> GetByUrl(string pilotUrl)
        {
            var maxWaitToken =
                new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds));

            var jsonResponse = await _httpClient.SendGetRequest(pilotUrl, maxWaitToken.Token);

            if (jsonResponse == null)
            {
                return null;
            }

            var pilotDetail = JsonConvert.DeserializeObject<PilotModel>(jsonResponse);

            return pilotDetail;
        }
    }
}