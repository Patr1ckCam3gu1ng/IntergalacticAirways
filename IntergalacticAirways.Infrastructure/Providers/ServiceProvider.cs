using System;
using System.IO;
using IntergalacticAirways.BLL.Services;
using IntergalacticAirways.DAL.Models;
using IntergalacticAirways.DAL.Repositories;
using IntergalacticAirways.Infrastructure.ConfigServices;
using IntergalacticAirways.Lib.Cache.Services;
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

            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddScoped<IStarshipsRepo, StarshipsRepo>();
            services.AddScoped<IStarshipService, StarshipService>();

            services.RegisterAutoMapper();
            services.AddMemoryCache();
        }
    }
}