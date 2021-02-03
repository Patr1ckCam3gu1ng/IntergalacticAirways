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

        public async Task<List<Pilot>> Insert(List<PilotModel> pilotModels)
        {
            var pilots = new List<Pilot>();

            foreach (var pilot in pilotModels.Select(pilot => _mapper.Map<Pilot>(pilot)))
            {
                pilots.Add(pilot);

                await _dbContext.Pilot.AddAsync(pilot);
            }

            await _dbContext.SaveChangesAsync();

            return pilots;
        }

        public async Task<Pilot> GetByUrl(string pilotUrl)
        {
            return await _dbContext.Pilot.FirstOrDefaultAsync(c => c.Url == pilotUrl);
        }
    }
}