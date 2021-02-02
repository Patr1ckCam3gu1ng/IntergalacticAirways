using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace IntergalacticAirways.DAL.Repositories
{
    public class StarshipPilotRepo : IStarshipPilotRepo
    {
        private readonly IntergalacticAirwaysDbContext _dbContext;

        public StarshipPilotRepo(IntergalacticAirwaysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<StarshipPilot>> GetByStarshipId(int starshipId)
        {
            return await _dbContext.StarshipPilot.Where(c => c.StarshipId == starshipId).ToListAsync();
        }
    }
}