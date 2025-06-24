using Soilution.DataService.SoilQualityProcessing.Models;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.SoilQualityProcessing.Services
{
    internal class SoilQualityProcessingService : ISoilQualityProcessingService
    {
        private readonly ISoilQualityDataRepository _dataRepository;

        public SoilQualityProcessingService(ISoilQualityDataRepository dataRepository)
        {
            _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
        }

        public async Task SubmitSoilDataReading(SoilQualityReadingDto soilData)
        {
            var record = new SoilQualityDataRecord
            {
                DeviceId = soilData.DeviceId,
                Timestamp = soilData.Timestamp,
                MoisturePercentage = soilData.MoisturePercentage,
                TemperatureCelsius = soilData.TemperatureCelcius,
                SunlightLumens = soilData.SunlightLumens,
                PHLevel = soilData.PHLevel,
            };

            await _dataRepository.CreateNewSoilRecord(record);
        }
    }
}
