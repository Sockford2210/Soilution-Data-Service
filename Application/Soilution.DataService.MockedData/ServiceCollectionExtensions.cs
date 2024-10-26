using Microsoft.Extensions.DependencyInjection;
using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.MockedData.Repositories;

namespace Soilution.DataService.MockedData
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupMockedRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAirQualityDataRepository, AirQualityMockedRepository>();
            services.AddScoped<ISoilDataRepository, SoilMockedRepository>();
        }
    }
}
