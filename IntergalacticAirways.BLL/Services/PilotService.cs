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
            SetPilotDetailsCache(starships);

            return GetPilotDetailsFromCache(starships);
        }

        #region Private Functions

        private List<Starship> GetPilotDetailsFromCache(IEnumerable<Starship> starships)
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

        private void SetPilotDetailsCache(IEnumerable<Starship> starships)
        {
            var nonCachedPilots = GetNonCachedPilotByUrl(starships);

            if (!nonCachedPilots.Any())
            {
                return;
            }

            var taskList = new List<Task>();

            taskList.AddRange(
                nonCachedPilots.Select(pilotUrl =>
                    Task.Run(async () => { await _pilotRepo.SetPilotDetailByUrl(pilotUrl); },
                        new CancellationTokenSource(
                            TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds)).Token)));

            if (!taskList.Any())
            {
                return;
            }

            Task.WaitAll(taskList.ToArray(),
                new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds)).Token);
        }

        private List<string> GetNonCachedPilotByUrl(IEnumerable<Starship> starships)
        {
            var nonCachedPilot = from starship in starships
                from starshipPilot in starship.Pilots
                let pilotDetails = _pilotRepo.GetNameByPilotUrl(starshipPilot.Url)
                where pilotDetails == null
                select starshipPilot.Url;

            return nonCachedPilot.ToList();
        }

        #endregion
    }
}