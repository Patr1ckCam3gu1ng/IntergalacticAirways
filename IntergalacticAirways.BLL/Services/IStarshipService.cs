using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.BLL.Services
{
    public interface IStarshipService
    {
        Task<List<Starship>> GetByPageIndexAsync(int pageIndex);

        List<Starship> FilterByCapacity(IEnumerable<Starship> starships, int numberOfPassengers);
    }
}