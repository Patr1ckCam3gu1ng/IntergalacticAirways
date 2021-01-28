using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace IntergalacticAirways.Lib.HttpClients
{
    public class ApiHttpClient : IApiHttpClient
    {
        public async Task<string> SendGetRequest(string apiEndpoint, CancellationToken maxWaitToken)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders
                .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(apiEndpoint, maxWaitToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync(maxWaitToken);

            return jsonString;
        }
    }
}