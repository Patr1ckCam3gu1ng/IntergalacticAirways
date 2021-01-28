using System;
using System.Threading.Tasks;
using IntergalacticAirways.BLL.Services;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IntergalacticAirways
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var serviceProvider = HostBuilder.GetServiceProvider(args);

            var starshipService = serviceProvider.GetRequiredService<IStarshipService>();

            var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>();

            while (true)
            {
                Console.WriteLine("Enter number of passengers:");

                var numberOfPassengers = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(numberOfPassengers) || string.IsNullOrEmpty(numberOfPassengers))
                {
                    continue;
                }

                if (!int.TryParse(numberOfPassengers, out _))
                {
                    continue;
                }

                var pageIndex = 1;

                Console.Clear();

                while (pageIndex <= appSettings.Value.MaximumPagination)
                {
                    var starships = await starshipService.GetAllWithinCapacity(Convert.ToInt32(numberOfPassengers), pageIndex);

                    pageIndex++;

                    Parallel.ForEach(starships, starship => { Console.WriteLine($"{starship.Name} -- "); });
                }

                break;
            }
        }
    }
}