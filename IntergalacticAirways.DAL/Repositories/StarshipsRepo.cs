using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace IntergalacticAirways.DAL.Repositories
{
    public class StarshipsRepo : IStarshipsRepo
    {
        private readonly IntergalacticAirwaysDbContext _dbContext;
        private readonly IMapper _mapper;

        public StarshipsRepo(IntergalacticAirwaysDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Insert(List<StarshipModel> starships, int pageIndex)
        {
            foreach (var starship in starships)
            {
                var mappedStarship = _mapper.Map<Starship>(starship);

                mappedStarship.PageIndex = pageIndex;

                await _dbContext.Starship.AddAsync(mappedStarship);

                foreach (var starshipPilot in starship.Pilots)
                {
                    await _dbContext.StarshipPilot.AddAsync(new StarshipPilot
                    {
                        StarshipId = mappedStarship.Id,
                        PilotUrl = starshipPilot.Url
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Starship>> GetByPageIndex(int pageIndex)
        {
            return await _dbContext.Starship.Where(c => c.PageIndex == pageIndex).ToListAsync();
        }
    }
}