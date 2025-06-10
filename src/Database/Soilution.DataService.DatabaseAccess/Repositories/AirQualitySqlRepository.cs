using Soilution.DataService.DatabaseAccess.CommandExecution;
using Soilution.DataService.DatabaseAccess.CommandRunner;
using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;

namespace Soilution.DataService.DatabaseAccess.Repositories
{
    /// <summary>
    /// SQL database repository for air quality data.
    /// </summary>
    internal class AirQualitySqlRepository : IAirQualityDataRepository
    {
        private readonly ICommandRunner _commandRunner;

        private const string AIR_QUALITY_TABLE_NAME = "AirQualityDataReadings";
        private const string DEVICE_ID_PARAMETER = "@DeviceId";
        private const string TIMESTAMP_PARAMETER = "@TimeStamp";
        private const string HUMIDITY_PARAMETER = "@Humidity";
        private const string TEMERATURE_PARAMETER = "@Temperature";
        private const string CO2_PARAMETER = "@CO2";
        private const string READING_COUNT_PARAMETER = "@ReadingCount";

        public AirQualitySqlRepository(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner ?? throw new ArgumentNullException(nameof(commandRunner));
        }

        /// <inheritdoc/>
        public async Task<int> CreateNewAirQualityRecord(AirQualityDataRecord record)
        {
            string insertStatement = $"INSERT INTO {AIR_QUALITY_TABLE_NAME} ({nameof(AirQualityDataRecord.DeviceId)}" +
                $" ,{nameof(AirQualityDataRecord.Timestamp)}" +
                $" ,{nameof(AirQualityDataRecord.HumidityPercentage)}" +
                $" ,{nameof(AirQualityDataRecord.TemperatureCelcius)}" +
                $" ,{nameof(AirQualityDataRecord.CO2PPM)}) " +
                $"OUTPUT INSERTED.{nameof(AirQualityDataRecord.Id)} " +
                $"VALUES ({DEVICE_ID_PARAMETER}" +
                $" ,{TIMESTAMP_PARAMETER}" +
                $" ,{HUMIDITY_PARAMETER}" +
                $" ,{TEMERATURE_PARAMETER}" +
                $" ,{CO2_PARAMETER})";

            var parameters = new Dictionary<string, object>
            {
                { DEVICE_ID_PARAMETER, record.DeviceId },
                { TIMESTAMP_PARAMETER, record.Timestamp },
                { HUMIDITY_PARAMETER, record.HumidityPercentage },
                { TEMERATURE_PARAMETER, record.TemperatureCelcius },
                { CO2_PARAMETER, record.CO2PPM }
            };

            var dBCommand = new DatabaseCommand(insertStatement, parameters);

            return await _commandRunner.ExecuteCommandWithSingularOutputValue<int>(dBCommand);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AirQualityDataRecord>> GetLatestAirQualityRecords(int deviceId, int count)
        {
            string queryStatement = $"SELECT TOP ({READING_COUNT_PARAMETER}) [{nameof(AirQualityDataRecord.Id)}]" +
                    $" ,[{nameof(AirQualityDataRecord.Id)}]" +
                    $" ,[{nameof(AirQualityDataRecord.Timestamp)}]" +
                    $" ,[{nameof(AirQualityDataRecord.HumidityPercentage)}]" +
                    $" ,[{nameof(AirQualityDataRecord.DeviceId)}]" +
                    $" ,[{nameof(AirQualityDataRecord.CO2PPM)}]" +
                    $" FROM {AIR_QUALITY_TABLE_NAME}" +
                    $" WHERE {nameof(AirQualityDataRecord.DeviceId)} = {DEVICE_ID_PARAMETER}" +
                    $" ORDER BY [{nameof(AirQualityDataRecord.Timestamp)}] DESC";

            var parameters = new Dictionary<string, object>
            {
                { DEVICE_ID_PARAMETER, deviceId },
                { READING_COUNT_PARAMETER, count }
            };

            var dBCommand = new DatabaseCommand(queryStatement, parameters);

            return await _commandRunner.ExecuteQueryAndReturnDataEnumerator<AirQualityDataRecord>(dBCommand);
        }

        /// <inheritdoc/>
        public async Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityData()
        {
            string queryStatement = $"SELECT" +
                    $" MAX({nameof(AirQualityDataRecord.HumidityPercentage)}) AS {nameof(AirQualityDataMaxMinAverage.MaximumHumidityPercentage)}" +
                    $", MIN({nameof(AirQualityDataRecord.HumidityPercentage)}) AS {nameof(AirQualityDataMaxMinAverage.MinimumHumidityPercentage)}" +
                    $", AVG({nameof(AirQualityDataRecord.HumidityPercentage)}) AS {nameof(AirQualityDataMaxMinAverage.AverageHumidityPercentage)}" +
                    $", MAX({nameof(AirQualityDataRecord.TemperatureCelcius)}) AS {nameof(AirQualityDataMaxMinAverage.MaximumTemperatureCelcius)}" +
                    $", MIN({nameof(AirQualityDataRecord.TemperatureCelcius)}) AS {nameof(AirQualityDataMaxMinAverage.MinimumTemperatureCelcius)}" +
                    $", AVG({nameof(AirQualityDataRecord.TemperatureCelcius)}) AS {nameof(AirQualityDataMaxMinAverage.AverageTemperatureCelcius)}" +
                    $", MAX({nameof(AirQualityDataRecord.CO2PPM)}) AS {nameof(AirQualityDataMaxMinAverage.MaximumCO2PPM)}" +
                    $", MIN({nameof(AirQualityDataRecord.CO2PPM)}) AS {nameof(AirQualityDataMaxMinAverage.MinimumCO2PPM)}" +
                    $", AVG({nameof(AirQualityDataRecord.CO2PPM)}) AS {nameof(AirQualityDataMaxMinAverage.AverageCO2PPM)}" +
                    $" FROM {AIR_QUALITY_TABLE_NAME}";

            var dBCommand = new DatabaseCommand(queryStatement);

            return await _commandRunner.ExecuteQueryAndReturnSingleRecord<AirQualityDataMaxMinAverage>(dBCommand);
        }

        /// <inheritdoc/>
        public async Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityDataSinceTimemstamp(DateTime fromTimestamp)
        {
            string queryStatement = $"SELECT" +
                    $" MAX({nameof(AirQualityDataRecord.HumidityPercentage)}) AS {nameof(AirQualityDataMaxMinAverage.MaximumHumidityPercentage)}" +
                    $", MIN({nameof(AirQualityDataRecord.HumidityPercentage)}) AS {nameof(AirQualityDataMaxMinAverage.MinimumHumidityPercentage)}" +
                    $", AVG({nameof(AirQualityDataRecord.HumidityPercentage)}) AS {nameof(AirQualityDataMaxMinAverage.AverageHumidityPercentage)}" +
                    $", MAX({nameof(AirQualityDataRecord.TemperatureCelcius)}) AS {nameof(AirQualityDataMaxMinAverage.MaximumTemperatureCelcius)}" +
                    $", MIN({nameof(AirQualityDataRecord.TemperatureCelcius)}) AS {nameof(AirQualityDataMaxMinAverage.MinimumTemperatureCelcius)}" +
                    $", AVG({nameof(AirQualityDataRecord.TemperatureCelcius)}) AS {nameof(AirQualityDataMaxMinAverage.AverageTemperatureCelcius)}" +
                    $", MAX({nameof(AirQualityDataRecord.CO2PPM)}) AS {nameof(AirQualityDataMaxMinAverage.MaximumCO2PPM)}" +
                    $", MIN({nameof(AirQualityDataRecord.CO2PPM)}) AS {nameof(AirQualityDataMaxMinAverage.MinimumCO2PPM)}" +
                    $", AVG({nameof(AirQualityDataRecord.CO2PPM)}) AS {nameof(AirQualityDataMaxMinAverage.AverageCO2PPM)}" +
                    $" FROM  {AIR_QUALITY_TABLE_NAME}" +
                    $" WHERE {nameof(AirQualityDataRecord.Timestamp)} >= {TIMESTAMP_PARAMETER}";

            var parameters = new Dictionary<string, object>
            {
                { TIMESTAMP_PARAMETER, fromTimestamp }
            };

            var dBCommand = new DatabaseCommand(queryStatement, parameters);

            return await _commandRunner.ExecuteQueryAndReturnSingleRecord<AirQualityDataMaxMinAverage>(dBCommand);
        }

        /// <inheritdoc/>
        public async Task<int> GetNumberOfRecordsForDevice(int deviceId)
        {
            string queryStatement = $"SELECT COUNT({nameof(AirQualityDataRecord.Id)}) as NumberOfRecords" +
                $" FROM {AIR_QUALITY_TABLE_NAME}" +
                $" WHERE {nameof(AirQualityDataRecord.DeviceId)} = {DEVICE_ID_PARAMETER}";

            var parameters = new Dictionary<string, object>
            {
                { DEVICE_ID_PARAMETER, deviceId }
            };

            var dbCommand = new DatabaseCommand(queryStatement, parameters);

            return await _commandRunner.ExecuteCommandWithSingularOutputValue<int>(dbCommand);
        }
    }
}
