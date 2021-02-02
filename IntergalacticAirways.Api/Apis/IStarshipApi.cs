using System.Collections.Generic;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;

namespace IntergalacticAirways.Api.Apis
{
    public interface IStarshipApi
    {
        Task<List<StarshipModel>> GetByPageIndexAsync(int pageIndex);
    }
}