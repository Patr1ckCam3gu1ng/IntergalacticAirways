using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.BLL.Services
{
    public interface IStarshipPilotService
    {
        Task<List<StarshipPilot>> GetByStarshipId(int starshipId);
    }
}