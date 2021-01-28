using System.Threading;
using System.Threading.Tasks;

namespace IntergalacticAirways.Lib.HttpClients
{
    public interface IApiHttpClient
    {
        Task<string> SendGetRequest(string apiEndpoint, CancellationToken maxWaitToken);
    }
}