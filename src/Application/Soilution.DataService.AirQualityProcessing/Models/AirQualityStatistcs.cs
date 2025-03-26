namespace Soilution.DataService.AirQualityProcessing.Models
{
    public class AirQualityStatistcs
    {
        public double MaximumHumidityPercentage { get; set; }
        public double MinimumHumidityPercentage { get; set; }
        public double AverageHumidityPercentage { get; set; }
        public double MaximumTemperatureCelcius { get; set; }
        public double MinimumTemperatureCelcius { get; set; }
        public double AverageTemperatureCelcius { get; set; }
        public double MaximumCo2ppm { get; set; }
        public double MinimumCo2ppm { get; set; }
        public double AverageCo2ppm { get; set; }
    }
}
