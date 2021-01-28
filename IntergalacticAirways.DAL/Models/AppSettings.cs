namespace IntergalacticAirways.DAL.Models
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }

        public int MaximumPagination { get; set; }

        public int RequestMaximumWaitSeconds { get; set; }
    }
}