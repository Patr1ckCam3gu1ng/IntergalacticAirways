using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.DAL.Repositories
{
    public interface IStarshipsRepo
    {
        Task<List<Starship>> GetAll(int pageIndex);
    }
}