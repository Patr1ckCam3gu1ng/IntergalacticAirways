﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IntergalacticAirways.Api.Apis;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.DAL.Repositories;
using IntergalacticAirways.Lib.Caches;
using Microsoft.Extensions.Options;

namespace IntergalacticAirways.BLL.Services
{
    public class PilotService : IPilotService
    {
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _cacheService;
        private readonly IPilotApi _pilotApi;
        private readonly IPilotRepo _pilotRepo;
        private readonly IStarshipPilotService _starshipPilotService;
        private readonly IMapper _mapper;

        public PilotService(IPilotRepo pilotRepo, IOptions<AppSettings> appSettings, IMemoryCache cacheService,
            IPilotApi pilotApi, IStarshipPilotService starshipPilotService, IMapper mapper)
        {
            _pilotRepo = pilotRepo;
            _cacheService = cacheService;
            _pilotApi = pilotApi;
            _starshipPilotService = starshipPilotService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<List<StarshipDto>> GetStarshipPilot(List<Starship> starships)
        {
            var pilotApi = await GetFromPilotApi(starships);

            if (pilotApi != null)
            {
                var pilots = await _pilotRepo.Insert(pilotApi);

                foreach (var pilot in pilots)
                {
                    await _cacheService.CacheResponseAsync($"{Resource.Pilots}_{pilot.Url}", pilot);
                }
            }

            return await GetFromPilots(starships);
        }

        #region Private Functions

        private async Task<List<StarshipDto>> GetFromPilots(IEnumerable<Starship> starships)
        {
            var starshipList = new List<StarshipDto>();

            foreach (var starship in starships)
            {
                var starshipDto = new StarshipDto();

                var starshipPilots = await _starshipPilotService.GetByStarshipId(starship.Id);

                var pilots = new List<PilotDto>();

                foreach (var starshipPilot in starshipPilots)
                {
                    var cacheKey = $"{Resource.Pilots}_{starshipPilot.PilotUrl}";

                    var pilotCache = _cacheService.GetCachedByKey(cacheKey);

                    if (pilotCache is Pilot pilot)
                    {
                        pilots.Add(new PilotDto
                        {
                            Name = pilot.Name
                        });
                    }
                }

                starshipDto.Name = starship.Name;
                starshipDto.Pilots = pilots;

                starshipList.Add(starshipDto);
            }

            return starshipList;
        }

        private async Task<List<PilotModel>> GetFromPilotApi(IEnumerable<Starship> starships)
        {
            var nonCachedPilots = await GetPilotsNotCached(starships);

            if (nonCachedPilots.Any() == false)
            {
                return null;
            }

            var taskList = new List<Task>();

            var pilots = new List<PilotModel>();

            taskList.AddRange(
                nonCachedPilots.Select(pilot =>
                    Task.Run(async () =>
                        {
                            var pilotApiResponse = await _pilotApi.GetByUrl(pilot.PilotUrl);

                            pilots.Add(pilotApiResponse);
                        },
                        new CancellationTokenSource(
                            TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds)).Token)));


            Task.WaitAll(taskList.ToArray(),
                new CancellationTokenSource(TimeSpan.FromSeconds(_appSettings.RequestMaximumWaitSeconds)).Token);

            return pilots;
        }

        private async Task<List<StarshipPilot>> GetPilotsNotCached(IEnumerable<Starship> starships)
        {
            var nonCachedPilots = new List<StarshipPilot>();

            foreach (var starship in starships)
            {
                var starshipPilots = await _starshipPilotService.GetByStarshipId(starship.Id);

                foreach (var pilot in starshipPilots)
                {
                    var cacheKey = $"{Resource.Pilots}_{pilot.PilotUrl}";

                    if (_cacheService.GetCachedByKey(cacheKey) == null)
                    {
                        nonCachedPilots.Add(pilot);
                    }
                }
            }

            return nonCachedPilots;
        }

        #endregion
    }
}