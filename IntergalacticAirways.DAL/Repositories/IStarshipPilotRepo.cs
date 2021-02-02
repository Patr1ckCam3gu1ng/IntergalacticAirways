using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.DAL.Repositories
{
    public interface IStarshipPilotRepo
    {
        Task<List<StarshipPilot>> GetByStarshipId(int starshipId);
    }
}