using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.DAL.Repositories;
using Microsoft.Extensions.Options;

namespace IntergalacticAirways.BLL.Services
{
    public class PilotService : IPilotService
    {
        private readonly AppSettings _appSettings;
        private readonly IPilotRepo _pilotRepo;

        public PilotService(IPilotRepo pilotRepo, IOptions<AppSettings> appSettings)
        {
            _pilotRepo = pilotRepo;
            _appSettings = appSettings.Value;
        }

        public List<Starship> AssignStarshipPilot(List<Starship> starships)
        {
            RequestPilotsDetail(starships);

            return AssignStarshipPilotName(starships);
        }

        private List<Starship> AssignStarshipPilotName(IEnumerable<Starship> starships)
        {
            var starshipList = new List<Starship>();

            foreach (var starship in starships)
            {
                var pilotDetails = new List<PilotDetail>();

                foreach (var pilot in starship.Pilots)
                {
                    var detail = _pilotRepo.GetNameByPilotUrl(pilot.Url);

                    pilot.Name = detail.Name;

                    pilotDetails.Add(pilot);
                }

                starship.Pilots = pilotDetails;

                starshipList.Add(starship);
            }

            return starshipList;
        }

        private void RequestPilotsDetail(IEnumerable<Starship> starships)
        {
            var taskList = new List<Task>();

            foreach (var starship in starships.Where(c => c.Pilots.Any()))
            {
                taskList.AddRange(starship.Pilots.Select(pilot =>
                    Task.Run(async () => { await _pilotRepo.RequestPilotDetailByUrl(pilot.Url); },
                        new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds))
                            .Token)));
            }

            if (!taskList.Any())
            {
                return;
            }

            Task.WaitAll(taskList.ToArray(),
                new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds)).Token);
        }
    }
}