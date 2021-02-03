using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.DAL.Repositories
{
    public interface IStarshipsRepo
    {
        Task Insert(List<StarshipModel> starships, int pageIndex);

        Task<List<Starship>> GetByPageIndex(int pageIndex);
    }
}