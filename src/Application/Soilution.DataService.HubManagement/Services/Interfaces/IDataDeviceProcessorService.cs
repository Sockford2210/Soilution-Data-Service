
using Soilution.DataService.HubManagement.Models;

namespace Soilution.DataService.HubManagement.Processors
{
    public interface IDataDeviceProcessorService
    {
        Task CreateNewDevice(string deviceName);
        Task<DataHubDetails> GetDeviceDetailsByDeviceName(string deviceName);
    }
}