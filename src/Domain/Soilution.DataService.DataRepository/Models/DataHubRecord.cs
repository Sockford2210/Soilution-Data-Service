using Soilution.DataService.DataRepository.Models.Base;
using System.Data;

namespace Soilution.DataService.DataRepository.Models
{
    public class DataHubRecord : DataRecordBase
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.MinValue;
        public bool Exists => Id != -1;

        public override void PopulateFromDataReader(IDataReader reader)
        {
            if (int.TryParse(reader[nameof(Id)]?.ToString(), out int id)) { Id = id; }
            Name = reader[nameof(Name)]?.ToString();
        }
    }
}
