using System.Collections.Generic;

namespace IntergalacticAirways.DAL.Models
{
    public class StarshipModel
    {
        public string Name { get; set; }

        public List<PilotModel> Pilots { get; set; }

        public int? PassengerCapacity { get; set; }
    }
}