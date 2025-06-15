using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.SoilQualityProcessing.Models;
using Soilution.DataService.SoilQualityProcessing.Services.Interfaces;

namespace Soilution.DataService.SoilQualityProcessing.Services
{
    internal class SoilQualityDeviceService : ISoilQualityDeviceService
    {
        private readonly ISoilQualityDeviceRepository _repository;

        public void CreateNewDevice(SoilQualityDeviceDto device)
        {
            throw new NotImplementedException();
        }
    }
}
