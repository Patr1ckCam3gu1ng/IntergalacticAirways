using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace IntergalacticAirways.DAL.Repositories
{
    public class PilotRepo : IPilotRepo
    {
        private readonly IntergalacticAirwaysDbContext _dbContext;
        private readonly IMapper _mapper;

        public PilotRepo(IntergalacticAirwaysDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Insert(List<PilotModel> pilots)
        {
            foreach (var mappedPilot in pilots.Select(pilot => _mapper.Map<Pilot>(pilot)))
            {
                await _dbContext.Pilot.AddAsync(mappedPilot);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Pilot> GetByUrl(string pilotUrl)
        {
            return await _dbContext.Pilot.FirstOrDefaultAsync(c => c.Url == pilotUrl);
        }
    }
}