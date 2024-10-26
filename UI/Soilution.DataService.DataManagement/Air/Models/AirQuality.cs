namespace Soilution.DataService.DataManagement.Air.Models
{
    public class AirQuality
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public double HumidityPercentage { get; set; }
        public double TemperatureCelcius { get; set; }
        public double Co2ppm { get; set; }
    }
}
