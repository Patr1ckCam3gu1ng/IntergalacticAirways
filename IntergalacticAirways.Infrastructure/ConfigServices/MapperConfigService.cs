using System;
using System.Linq;
using AutoMapper;
using IntergalacticAirways.DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace IntergalacticAirways.Infrastructure.ConfigServices
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<StarshipDetail, Starship>()
                .ForMember(dest => dest.PassengerCapacity,
                    opt =>
                        opt.MapFrom(src =>
                            src.Passengers.All(char.IsNumber) ? int.Parse(src.Passengers) : (int?) null));
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