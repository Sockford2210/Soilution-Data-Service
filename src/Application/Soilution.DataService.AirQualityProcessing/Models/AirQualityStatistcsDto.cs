namespace Soilution.DataService.AirQualityProcessing.Models
{
    public class AirQualityStatistcsDto
    {
        public double MaximumHumidityPercentage { get; set; } = -1;
        public double MinimumHumidityPercentage { get; set; } = -1;
        public double AverageHumidityPercentage { get; set; } = -1;
        public double MaximumTemperatureCelcius { get; set; } = -1;
        public double MinimumTemperatureCelcius { get; set; } = -1;
        public double AverageTemperatureCelcius { get; set; } = -1;
        public double MaximumCo2ppm { get; set; } = -1;
        public double MinimumCo2ppm { get; set; } = -1;
        public double AverageCo2ppm { get; set; } = -1;
    }
}
