using Soilution.DataService.SoilQualityProcessing.Models;

namespace Soilution.DataService.SoilQualityProcessing.Services
{
    public interface ISoilQualityProcessingService
    {
        Task SubmitSoilDataReading(IncomingSoilQualityReading soilData);
    }
}