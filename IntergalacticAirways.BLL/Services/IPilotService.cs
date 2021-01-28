using System.Collections.Generic;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.BLL.Services
{
    public interface IPilotService
    {
        List<Starship> AssignStarshipPilot(List<Starship> starships);
    }
}