using Soilution.DataService.DeviceManagement.Devices.Processors;

namespace Soilution.DataService.DeviceManagement
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDeviceManagementApps(this IServiceCollection services)
        {
            services.AddScoped<IDataDeviceProcessor, DataDeviceProcessor>();
        }
    }
}