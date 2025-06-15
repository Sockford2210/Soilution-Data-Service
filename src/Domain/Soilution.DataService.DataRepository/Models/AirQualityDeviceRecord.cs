using Soilution.DataService.DataRepository.Models.Base;
using System.Data;

namespace Soilution.DataService.DataRepository.Models
{
    public class AirQualityDeviceRecord : QueryResultObject
    {
        public int Id { get; set; } = -1;
        public int HubId { get; set; } = -1;
        public DateTime DateCreated { get; set; }

        public override void PopulateFromDataReader(IDataReader reader)
        {
            if (int.TryParse(reader[nameof(Id)]?.ToString(), out int id)) { Id = id; }
            if (int.TryParse(reader[nameof(HubId)]?.ToString(), out int hubId)) { HubId = hubId; }
            if (DateTime.TryParse(reader[nameof(DateCreated)]?.ToString(), out DateTime dateCreated)) { DateCreated = dateCreated; }
        }
    }
}
