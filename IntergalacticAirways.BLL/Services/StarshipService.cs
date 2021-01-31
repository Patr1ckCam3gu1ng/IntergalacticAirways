using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.DAL.Repositories;

namespace IntergalacticAirways.BLL.Services
{
    public class StarshipService : IStarshipService
    {
        private readonly IStarshipsRepo _starshipsRepo;

        public StarshipService(IStarshipsRepo starshipsRepo)
        {
            _starshipsRepo = starshipsRepo;
        }

        public async Task<List<Starship>> GetByPageIndexAsync(int pageIndex)
        {
            var starships = await _starshipsRepo.GetByPageIndexAsync(pageIndex);

            return starships;
        }

        public IEnumerable<Starship> FilterByCapacity(IEnumerable<Starship> starships, int numberOfPassengers)
        {
            return starships.Where(starship => starship.PassengerCapacity != null)
                .Where(starship =>
                    starship.PassengerCapacity >= numberOfPassengers &&
                    numberOfPassengers <= starship.PassengerCapacity
                );
        }
    }
}