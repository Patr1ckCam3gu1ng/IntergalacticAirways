using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.DAL.Repositories
{
    public interface IPilotRepo
    {
        Task<PilotDetail> RequestPilotDetailByUrl(string pilotUrl);

        PilotDetail GetNameByPilotUrl(string pilot);
    }
}