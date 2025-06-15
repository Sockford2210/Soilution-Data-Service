using Soilution.DataService.AirQualityProcessing.Models;

namespace Soilution.DataService.AirQualityProcessing.Services.Interfaces
{
    public interface IAirQualityDeviceService
    {
        Task CreateNewDevice(AirQualityDeviceDto newDevice);
    }
}