
namespace Soilution.DataService.DeviceManagement.Devices.Processors
{
    public interface IDataDeviceProcessor
    {
        Task CreateNewDevice(string deviceName);
    }
}