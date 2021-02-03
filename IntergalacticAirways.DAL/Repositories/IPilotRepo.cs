using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.DAL.Repositories
{
    public interface IPilotRepo
    {
        Task<List<Pilot>> Insert(List<PilotModel> pilotModels);

        Task<Pilot> GetByUrl(string pilotUrl);
    }
}