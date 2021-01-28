using System.Collections.Generic;

namespace IntergalacticAirways.DAL.Models
{
    public class StarshipResponse
    {
        public int Count { get; set; }

        public List<StarshipDetail> Results { get; set; }
    }
}