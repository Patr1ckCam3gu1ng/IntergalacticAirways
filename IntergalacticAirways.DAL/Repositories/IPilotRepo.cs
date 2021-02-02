using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.DAL.Repositories
{
    public interface IPilotRepo
    {
        Task Insert(List<PilotModel> pilots);

        Task<Pilot> GetByUrl(string pilotUrl);
    }
}