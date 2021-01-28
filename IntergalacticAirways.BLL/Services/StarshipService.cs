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

        public async Task<List<Starship>> GetAllWithinCapacity(int numberOfPassengers, int pageIndex)
        {
            var starships = await _starshipsRepo.GetAll(pageIndex);

            return starships?.Where(starship => starship.PassengerCapacity != null)
                .Where(starship => starship.PassengerCapacity >= numberOfPassengers &&
                                   numberOfPassengers <= starship.PassengerCapacity)
                .ToList();
        }
    }
}