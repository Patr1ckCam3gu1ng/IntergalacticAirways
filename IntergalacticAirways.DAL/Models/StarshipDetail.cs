using System.Collections.Generic;

namespace IntergalacticAirways.DAL.Models
{
    public class StarshipDetail
    {
        public string Name { get; set; }

        public string Passengers { get; set; }

        public string Url { get; set; }

        public List<string> Pilots { get; set; }
    }
}