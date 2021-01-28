namespace IntergalacticAirways.DAL.Models
{
    public class StarshipResponse
    {
        public int Count { get; set; }

        public StarshipDetail[] Results { get; set; }
    }

    public class StarshipDetail
    {
        public string Name { get; set; }

        public string Passengers { get; set; }

        public string Url { get; set; }
    }
}