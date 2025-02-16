using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Soilution.DataService.SqlRepository.Repositories
{
    /// <summary>
    /// SQL database repository for soil quality data.
    /// </summary>
    internal class SoilSQLRepository : DatabaseAccessor, ISoilDataRepository
    {
        public SoilSQLRepository(ILogger<DatabaseAccessor> logger, IOptions<DatabaseAccessorSettings> options) : base(logger, options) { }

        /// <inheritdoc/>
        public async Task<int> CreateNewSoilRecord(SoilQualityDataRecord record)
        {
            string insertStatement = $"INSERT INTO SoilDataReadings ({nameof(SoilQualityDataRecord.DeviceId)}" +
                $", {nameof(SoilQualityDataRecord.Timestamp)}" +
                $", {nameof(SoilQualityDataRecord.MoisturePercentage)}" +
                $", {nameof(SoilQualityDataRecord.PHLevel)}" +
                $", {nameof(SoilQualityDataRecord.TemperatureCelcius)}" +
                $", {nameof(SoilQualityDataRecord.SunlightLumens)})" +
                $"OUTPUT INSERTED.{nameof(SoilQualityDataRecord.Id)} " +
                $" VALUES (@DeviceId" +
                $", @TimeStamp" +
                $", @Moisture" +
                $", @PH" +
                $", @Temperature" +
                $", @Sunlight)";

            var parameters = new Dictionary<string, object>
            {
                {"@DeviceId", record.DeviceId},
                {"@TimeStamp", record.Timestamp},
                {"@Moisture", record.MoisturePercentage},
                {"@PH", record.PHLevel},
                {"@Temp", record.TemperatureCelcius},
                {"@Sunlight", record.SunlightLumens}
            };

            var dBCommand = new DatabaseCommand(insertStatement, parameters);

            return await ExecuteCommandWithSingularOutputValue(dBCommand);
        }
    }
}
