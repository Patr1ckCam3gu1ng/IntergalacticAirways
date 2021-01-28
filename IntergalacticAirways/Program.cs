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

            var starshipService = serviceProvider.GetRequiredService<IStarshipService>();
            var pilotService = serviceProvider.GetRequiredService<IPilotService>();
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
                    while (pageIndex < appSettings.Value.MaximumPagination)
                    {
                        var index = pageIndex;

                        var starships = await GetStartStarships(starshipService, index,
                            passengerCount, pilotService);

                        foreach (var starship in starships)
                        {
                            foreach (var pilot in starship.Pilots)
                            {
                                Console.WriteLine($"{starship.Name} -- {pilot.Name}");
                            }
                        }

                        pageIndex++;
                    }

                    Console.WriteLine();
                    Console.Write("Press return key to begin again...");
                    Console.Read();
                }
            }
        }

        #region Private methods

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

            return numberPassengers < 0;
        }

        private static async Task<List<Starship>> GetStartStarships(IStarshipService starshipService, int pageIndex,
            string numberOfPassengers,
            IPilotService pilotService)
        {
            var starshipByPageIndex = await starshipService.GetByPageIndexAsync(pageIndex);

            var starshipsByCapacity =
                starshipService.FilterByCapacity(starshipByPageIndex, Convert.ToInt32(numberOfPassengers));

            var starshipWithPilotName = pilotService.AssignStarshipPilot(starshipsByCapacity);

            return starshipWithPilotName;
        }

        #endregion
    }
}