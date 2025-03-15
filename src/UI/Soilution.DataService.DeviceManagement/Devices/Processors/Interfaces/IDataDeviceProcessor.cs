
using Soilution.DataService.DeviceManagement.Devices.Models;

namespace Soilution.DataService.DeviceManagement.Devices.Processors
{
    public interface IDataDeviceProcessor
    {
        Task CreateNewDevice(string deviceName);
        Task<DataHubDetails> GetDeviceDetailsByDeviceName(string deviceName);
    }
}