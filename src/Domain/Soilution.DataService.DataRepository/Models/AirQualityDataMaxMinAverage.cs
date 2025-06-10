using Soilution.DataService.DataRepository.Models.Base;
using System.Data;

namespace Soilution.DataService.DataRepository.Models
{
    public class AirQualityDataMaxMinAverage : QueryResultObject
    {
        public double MaximumHumidityPercentage { get; set; }
        public double MinimumHumidityPercentage { get; set; }
        public double AverageHumidityPercentage { get; set; }
        public double MaximumTemperatureCelcius { get; set; }
        public double MinimumTemperatureCelcius { get; set; }
        public double AverageTemperatureCelcius { get; set; }
        public double MaximumCO2PPM { get; set; }
        public double MinimumCO2PPM { get; set; }
        public double AverageCO2PPM { get; set; }

        public override void PopulateFromDataReader(IDataReader reader)
        {
            if (double.TryParse(reader[nameof(MaximumHumidityPercentage)]?.ToString(), out double maxHumidity)) { MaximumHumidityPercentage = maxHumidity; }
            if (double.TryParse(reader[nameof(MinimumHumidityPercentage)]?.ToString(), out double minHumidity)) { MinimumHumidityPercentage = minHumidity; }
            if (double.TryParse(reader[nameof(AverageHumidityPercentage)]?.ToString(), out double avgHumidity)) { AverageHumidityPercentage = avgHumidity; }
            if (double.TryParse(reader[nameof(MaximumTemperatureCelcius)]?.ToString(), out double maxTemperature)) { MaximumTemperatureCelcius = maxTemperature; }
            if (double.TryParse(reader[nameof(MinimumTemperatureCelcius)]?.ToString(), out double minTemperature)) { MinimumTemperatureCelcius = minTemperature; }
            if (double.TryParse(reader[nameof(AverageTemperatureCelcius)]?.ToString(), out double avgTemperature)) { AverageTemperatureCelcius = avgTemperature; }
            if (double.TryParse(reader[nameof(MaximumCO2PPM)]?.ToString(), out double maxCo2)) { MaximumCO2PPM = maxCo2; }
            if (double.TryParse(reader[nameof(MinimumCO2PPM)]?.ToString(), out double minCo2)) { MinimumCO2PPM = minCo2; }
            if (double.TryParse(reader[nameof(AverageCO2PPM)]?.ToString(), out double avgCo2)) { AverageCO2PPM = avgCo2; }
        }
    }
}
