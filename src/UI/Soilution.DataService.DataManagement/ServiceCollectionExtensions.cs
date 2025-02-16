using Soilution.DataService.DataManagement.Air.Processors;
using Soilution.DataService.DataManagement.Soil.Processors;

namespace Soilution.DataService.DataManagement
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDataManagementApps(this IServiceCollection services)
        {
            services.AddScoped<IAirQualityRecordProcessor, AirQualityRecordProcessor>();
            services.AddScoped<ISoilQualityRecordProcessor, SoilQualityRecordProcessor>();
        }
    }
}
