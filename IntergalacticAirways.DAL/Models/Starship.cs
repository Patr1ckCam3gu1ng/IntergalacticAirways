using System.Collections.Generic;

namespace IntergalacticAirways.DAL.Models
{
    public class Starship
    {
        public string Name { get; set; }

        public int? PassengerCapacity { get; set; }

        public List<PilotDetail> Pilots { get; set; }
    }
}