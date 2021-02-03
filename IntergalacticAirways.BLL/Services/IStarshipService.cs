using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Entities;

namespace IntergalacticAirways.BLL.Services
{
    public interface IStarshipService
    {
        Task<List<Starship>> GetByIndexPage(int pageIndex);

        List<Starship> FilterByPassengerCapacity(IEnumerable<Starship> starships, int numberOfPassengers);
    }
}