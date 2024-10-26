using Soilution.DataService.DataManagement.Soil.Models;

namespace Soilution.DataService.DataManagement.Soil.Processors
{
    public interface ISoilQualityRecordProcessor
    {
        Task SubmitSoilDataReading(IncomingSoilQualityReading soilData);
    }
}