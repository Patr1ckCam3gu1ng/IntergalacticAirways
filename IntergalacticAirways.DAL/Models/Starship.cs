using System.ComponentModel.DataAnnotations;

namespace IntergalacticAirways.DAL.Models
{
    public class Starship
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int? PassengerCapacity { get; set; }

        public int PageIndex { get; set; }
    }
}