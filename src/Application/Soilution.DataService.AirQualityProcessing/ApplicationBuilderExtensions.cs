using Microsoft.Extensions.DependencyInjection;
using Soilution.DataService.AirQualityProcessing.Services.Interfaces;
using Soilution.DataService.AirQualityProcessing.Services;

namespace Soilution.DataService.AirQualityProcessing
{
    public static class ApplicationBuilderExtensions
    {
        public static void RegisterAirQualityProcessingApps(this IServiceCollection services)
        {
            services.AddScoped<IAirQualityProcessorService, AirQualityProcessorService>();
            services.AddScoped<IAirQualityAnalyticsService, AirQualityAnalyticsService>();
        }
    }
}
