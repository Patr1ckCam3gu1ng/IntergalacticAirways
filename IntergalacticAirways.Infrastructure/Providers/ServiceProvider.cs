using System;
using System.IO;
using IntergalacticAirways.Api;
using IntergalacticAirways.Api.Apis;
using IntergalacticAirways.BLL.Services;
using IntergalacticAirways.DAL.Entities;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.DAL.Repositories;
using IntergalacticAirways.Infrastructure.ConfigServices;
using IntergalacticAirways.Lib.Caches;
using IntergalacticAirways.Lib.HttpClients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntergalacticAirways.Infrastructure.Providers
{
    public static class HostBuilder
    {
        public static IServiceProvider GetServiceProvider(string[] args)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            services.Configure<AppSettings>(configuration.GetSection("App"));

            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IApiHttpClient, ApiHttpClient>();

            services.AddScoped<IStarshipsRepo, StarshipsRepo>();
            services.AddScoped<IStarshipPilotRepo, StarshipPilotRepo>();
            services.AddScoped<IPilotRepo, PilotRepo>();

            services.AddScoped<IStarshipService, StarshipService>();
            services.AddScoped<IPilotService, PilotService>();
            services.AddScoped<IStarshipPilotService, StarshipPilotService>();

            services.AddScoped<IStarshipApi, StarshipApi>();
            services.AddScoped<IPilotApi, PilotApi>();


            services.RegisterAutoMapper();
            services.AddMemoryCache();

            services.AddDbContext<IntergalacticAirwaysDbContext>(
                opt => opt.UseInMemoryDatabase("IntergalacticAirways"));
        }
    }
}