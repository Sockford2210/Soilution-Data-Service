using Soilution.DataService.DataRepository.Models;

namespace Soilution.DataService.DataRepository.Repositories
{
    public interface IAirQualityDeviceRepository
    {
        Task<int> CreateNewDevice(AirQualityDeviceRecord newDevice);

        Task<AirQualityDeviceRecord> GetDeviceByName(string deviceName);
    }
}
