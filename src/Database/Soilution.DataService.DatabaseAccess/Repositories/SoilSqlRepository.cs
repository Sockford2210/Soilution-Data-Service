using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;
using Soilution.DataService.DatabaseAccess.CommandRunner;
using Soilution.DataService.DatabaseAccess.CommandExecution;

namespace Soilution.DataService.DatabaseAccess.Repositories
{
    /// <summary>
    /// SQL database repository for soil quality data.
    /// </summary>
    internal class SoilSQLRepository : ISoilDataRepository
    {
        private readonly ICommandRunner _commandRunner;

        private const string DEVICE_ID_PARAMETER = "@DeviceId";
        private const string TIMESTAMP_PARAMETER = "@TimeStamp";
        private const string MOISTURE_PARAMETER = "@Moisture";
        private const string PH_PARAMETER = "@PH";
        private const string TEMPERATURE_PARAMETER = "@Temperature";
        private const string SUNLIGHT_PARAMETER = "@Sunlight";

        public SoilSQLRepository(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner ?? throw new ArgumentNullException(nameof(commandRunner));
        }

        /// <inheritdoc/>
        public async Task<int> CreateNewSoilRecord(SoilQualityDataRecord record)
        {
            string insertStatement = $"INSERT INTO SoilDataReadings ({nameof(SoilQualityDataRecord.DeviceId)}" +
                $", {nameof(SoilQualityDataRecord.Timestamp)}" +
                $", {nameof(SoilQualityDataRecord.MoisturePercentage)}" +
                $", {nameof(SoilQualityDataRecord.PHLevel)}" +
                $", {nameof(SoilQualityDataRecord.TemperatureCelsius)}" +
                $", {nameof(SoilQualityDataRecord.SunlightLumens)})" +
                $"OUTPUT INSERTED.{nameof(SoilQualityDataRecord.Id)} " +
                $" VALUES ({DEVICE_ID_PARAMETER}" +
                $", {TIMESTAMP_PARAMETER}" +
                $", {MOISTURE_PARAMETER}" +
                $", {PH_PARAMETER}" +
                $", {TEMPERATURE_PARAMETER}" +
                $", {SUNLIGHT_PARAMETER})";

            var parameters = new Dictionary<string, object>
            {
                { DEVICE_ID_PARAMETER, record.DeviceId },
                { TIMESTAMP_PARAMETER, record.Timestamp },
                { MOISTURE_PARAMETER, record.MoisturePercentage },
                { PH_PARAMETER, record.PHLevel },
                { TEMPERATURE_PARAMETER, record.TemperatureCelsius },
                { SUNLIGHT_PARAMETER, record.SunlightLumens }
            };

            var dBCommand = new DatabaseCommand(insertStatement, parameters);

            return await _commandRunner.ExecuteCommandWithSingularOutputValue<int>(dBCommand);
        }
    }
}
