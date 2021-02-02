using System;
using System.Collections.Generic;
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

            var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>();

            while (true)
            {
                var pageIndex = 1;

                Console.Clear();
                Console.Write("Enter number of passengers: ");

                var passengerCount = Console.ReadLine();

                if (IsInputInvalid(passengerCount))
                {
                    continue;
                }

                Console.Clear();

                {
                    var hasAnyCapableStarship = false;

                    while (pageIndex < appSettings.Value.MaximumPagination)
                    {
                        var starships = await GetStarStarships(serviceProvider, pageIndex, passengerCount);

                        foreach (var starship in starships)
                        {
                            foreach (var pilot in starship.Pilots)
                            {
                                hasAnyCapableStarship = true;
                            
                                Console.WriteLine($"{starship.Name} -- {pilot.Name}");
                            }
                        }

                        pageIndex++;
                    }

                    if (!hasAnyCapableStarship)
                    {
                        Console.WriteLine("No Starship capable to transport this number of passengers\n");

                        PrintReturnKey();

                        continue;
                    }

                    Console.WriteLine();

                    PrintReturnKey();
                }
            }
        }

        #region Private methods

        private static void PrintReturnKey()
        {
            Console.Write("Press return key to begin again...");
            Console.Read();
        }

        private static bool IsInputInvalid(string passengerReadLine)
        {
            if (string.IsNullOrWhiteSpace(passengerReadLine) || string.IsNullOrEmpty(passengerReadLine))
            {
                return true;
            }

            if (!int.TryParse(passengerReadLine, out _))
            {
                return true;
            }

            var numberPassengers = Convert.ToInt32(passengerReadLine);

            return numberPassengers < 1;
        }

        private static async Task<List<StarshipDto>> GetStarStarships(IServiceProvider serviceProvider, int pageIndex,
            string numberOfPassengers)
        {
            var starshipService = serviceProvider.GetRequiredService<IStarshipService>();
            var pilotService = serviceProvider.GetRequiredService<IPilotService>();

            var starshipByPageIndex = await starshipService.GetByIndexPage(pageIndex);

            var starshipsByCapacity =
                starshipService.FilterByPassengerCapacity(starshipByPageIndex, Convert.ToInt32(numberOfPassengers));

            var starshipWithPilotName = await pilotService.GetStarshipPilot(starshipsByCapacity);

            return starshipWithPilotName;
        }

        #endregion
    }
}