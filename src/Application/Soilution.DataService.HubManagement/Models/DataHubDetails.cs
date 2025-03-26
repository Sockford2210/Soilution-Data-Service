namespace Soilution.DataService.HubManagement.Models
{
    public class DataHubDetails
    {
        public string DeviceName { get; set; } = string.Empty;
        public int NumberOfRecords { get; set; } = 0;
        public DateTime DateRegistered { get; set; } = DateTime.MinValue;
    }
}
