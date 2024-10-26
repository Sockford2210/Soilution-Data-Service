using Microsoft.Extensions.DependencyInjection;
using Soilution.DataService.Analytics.Services.Interfaces;
using Soilution.DataService.Analytics.Services;

namespace Soilution.DataService.Analytics
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAnalyticsSuite(this IServiceCollection services)
        {
            services.AddScoped<IAnalyticsService, AnalyticsService>();
        }
    }
}