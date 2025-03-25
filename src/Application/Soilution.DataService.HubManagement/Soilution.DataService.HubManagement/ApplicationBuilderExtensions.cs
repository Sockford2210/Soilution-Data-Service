using Microsoft.Extensions.DependencyInjection;
using Soilution.DataService.DeviceManagement.Processors;
using Soilution.DataService.HubManagement.Processors;

namespace Soilution.DataService.HubManagement
{
    public static class ApplicationBuilderExtensions
    {
        public static void RegisterHubManagementApps(this IServiceCollection services)
        {
            services.AddScoped<IDataDeviceProcessorService, DataDeviceProcessorService>();
        }
    }
}
