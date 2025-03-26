using Microsoft.Extensions.DependencyInjection;
using Soilution.DataService.SoilQualityProcessing.Services;

namespace Soilution.DataService.SoilQualityProcessing
{
    public static class ApplicationBuilderExtensions
    {
        public static void RegisterSoilQualityProcessingApps(this IServiceCollection services)
        {
            services.AddScoped<ISoilQualityProcessingService, SoilQualityProcessingService>();
        }
    }
}
