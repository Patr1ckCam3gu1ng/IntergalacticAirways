using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.DAL.Repositories
{
    public interface IPilotRepo
    {
        Task<PilotDetail> SetPilotDetailByUrl(string pilotUrl);

        PilotDetail GetNameByPilotUrl(string pilot);
    }
}