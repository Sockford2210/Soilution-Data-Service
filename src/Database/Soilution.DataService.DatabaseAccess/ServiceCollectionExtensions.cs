using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soilution.DataService.DatabaseAccess.Accessor;
using Soilution.DataService.DatabaseAccess.CommandExecution;
using Soilution.DataService.DatabaseAccess.CommandRunner;
using Soilution.DataService.DatabaseAccess.Repositories;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.DatabaseAccess
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupSQLRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDatabaseAccessorFactory, SqlDatabaseAccessorFactory>();
            services.AddScoped<ICommandRunner, SqlDatabaseCommandRunner>();

            services.AddScoped<IAirQualityDataRepository, AirQualitySqlRepository>();
            services.AddScoped<IAirQualityDeviceRepository, AirQualityDeviceSqlRepository>();
            services.AddScoped<ISoilDataRepository, SoilSQLRepository>();
            services.AddScoped<ISoilQualityDeviceRepository, SoilQualityDeviceSqlRepository>();
            services.AddScoped<IDataHubRepository, DataHubSqlRepository>();

            services.Configure<DatabaseAccessorSettings>(configuration.GetSection("Database"));
        }
    }
}
