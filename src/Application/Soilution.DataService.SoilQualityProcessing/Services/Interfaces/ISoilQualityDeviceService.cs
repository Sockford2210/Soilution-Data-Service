using Soilution.DataService.SoilQualityProcessing.Models;

namespace Soilution.DataService.SoilQualityProcessing.Services.Interfaces
{
    internal interface ISoilQualityDeviceService
    {
        void CreateNewDevice(SoilQualityDeviceDto device);
    }
}