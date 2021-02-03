using System;
using System.Linq;
using AutoMapper;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace IntergalacticAirways.Infrastructure.ConfigServices
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<StarshipDetail, StarshipModel>()
                .ForMember(c => c.Pilots, f =>
                    f.MapFrom(c => c.Pilots.Select(url => new PilotModel { Url = url })))
                .ForMember(dest => dest.PassengerCapacity,
                    opt =>
                        opt.MapFrom(src =>
                            src.Passengers.All(char.IsNumber) ? int.Parse(src.Passengers) : (int?) null));

            CreateMap<PilotModel, Pilot>();

            CreateMap<StarshipModel, Starship>();
        }
    }

    public static class MapperConfigService
    {
        public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
        {
            //INFO: Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}