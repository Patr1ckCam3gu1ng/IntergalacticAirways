using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.Api.Apis
{
    public interface IPilotApi
    {
        Task<PilotModel> GetByUrl(string pilotUrl);
    }
}