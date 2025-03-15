using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.SqlRepository.Repositories;

namespace Soilution.DataService.SqlRepository
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupSQLRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAirQualityDataRepository, AirQualityDataReadingsSqlRepository>();
            services.AddScoped<ISoilDataRepository, SoilSQLRepository>();
            services.AddScoped<IDataHubRepository, DataHubSqlRepository>();

            services.Configure<DatabaseAccessorSettings>(configuration.GetSection("Database"));
        }
    }
}
