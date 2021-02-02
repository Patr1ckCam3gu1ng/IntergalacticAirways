using System.Collections.Generic;

namespace IntergalacticAirways.DAL.Models
{
    public class StarshipDto
    {
        public string Name { get; set; }

        public List<PilotDto> Pilots { get; set; }
    }
}