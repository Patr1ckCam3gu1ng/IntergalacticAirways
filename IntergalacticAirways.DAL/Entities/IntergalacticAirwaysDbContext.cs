using IntergalacticAirways.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace IntergalacticAirways.DAL.Entities
{
    public class IntergalacticAirwaysDbContext : DbContext
    {
        public IntergalacticAirwaysDbContext(DbContextOptions<IntergalacticAirwaysDbContext> options)
            : base(options)
        {
        }

        public DbSet<StarshipPilot> StarshipPilot { get; set; }

        public DbSet<Starship> Starship { get; set; }

        public DbSet<Pilot> Pilot { get; set; }
    }
}