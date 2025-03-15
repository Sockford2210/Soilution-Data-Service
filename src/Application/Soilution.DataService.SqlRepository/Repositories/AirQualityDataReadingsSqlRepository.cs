using Soilution.DataService.DataRepository.Models;
using Soilution.DataService.DataRepository.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Soilution.DataService.SqlRepository.Repositories
{
    /// <summary>
    /// SQL database repository for air quality data.
    /// </summary>
    internal class AirQualityDataReadingsSqlRepository : DatabaseAccessor, IAirQualityDataRepository
    {
        private const string DEVICE_ID_PARAMETER = "@DeviceId";
        private const string TIMESTAMP_PARAMETER = "@TimeStamp";
        private const string HUMIDITY_PARAMETER = "@Humidity";
        private const string TEMERATURE_PARAMETER = "@Temperature";
        private const string CO2_PARAMETER = "@CO2";
        private const string READING_COUNT_PARAMETER = "@ReadingCount";

        public AirQualityDataReadingsSqlRepository(ILogger<DatabaseAccessor> logger, IOptions<DatabaseAccessorSettings> options) 
            : base(logger, options) { }

        /// <inheritdoc/>
        public async Task<int> CreateNewAirQualityRecord(AirQualityDataRecord record)
        {
            string insertStatement = $"INSERT INTO AirQualityDataReadings ({nameof(AirQualityDataRecord.DeviceId)}" +
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
                {DEVICE_ID_PARAMETER, record.DeviceId},
                {TIMESTAMP_PARAMETER, record.Timestamp},
                {HUMIDITY_PARAMETER, record.HumidityPercentage},
                {TEMERATURE_PARAMETER, record.TemperatureCelcius},
                {CO2_PARAMETER, record.CO2PPM}
            };

            var dBCommand = new DatabaseCommand(insertStatement, parameters);

            return await ExecuteCommandWithSingularOutputValue<int>(dBCommand);
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
                    $" FROM AirQualityDataReadings" +
                    $" WHERE {nameof(AirQualityDataRecord.DeviceId)} = {DEVICE_ID_PARAMETER}" +
                    $" ORDER BY [{nameof(AirQualityDataRecord.Timestamp)}] DESC";

            var parameters = new Dictionary<string, object>
            {
                {DEVICE_ID_PARAMETER, deviceId},
                { READING_COUNT_PARAMETER, count }
            };

            var dBCommand = new DatabaseCommand(queryStatement, parameters);

            return await ExecuteQueryAndReturnData<AirQualityDataRecord>(dBCommand);
        }

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
                    $" FROM AirQualityDataReadings";

            var dBCommand = new DatabaseCommand(queryStatement);

            var results = await ExecuteQueryAndReturnData<AirQualityDataMaxMinAverage>(dBCommand);

            return results.FirstOrDefault() ?? new AirQualityDataMaxMinAverage();
        }

        public async Task<AirQualityDataMaxMinAverage> GetMinMaxAverageAirQualityDataSinceTimemstamp(DateTime fromTimestamp)
        {
            const string fromTimestampParameterName = "@FromTimestamp";
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
                    $" FROM AirQualityDataReadings" +
                    $" WHERE {nameof(AirQualityDataRecord.Timestamp)} >= {fromTimestampParameterName}";

            var parameters = new Dictionary<string, object>
            {
                { fromTimestampParameterName, fromTimestamp }
            };

            var dBCommand = new DatabaseCommand(queryStatement, parameters);

            var results = await ExecuteQueryAndReturnData<AirQualityDataMaxMinAverage>(dBCommand);

            return results.FirstOrDefault() ?? new AirQualityDataMaxMinAverage();
        }

        public async Task<int> GetNumberOfRecordsForDevice(int deviceId)
        {
            string queryStatement = $"SELECT" +
                $" COUNT({nameof(AirQualityDataRecord.Id)}) as NumberOfRecords" +
                $" WHERE {nameof(AirQualityDataRecord.DeviceId)} = {DEVICE_ID_PARAMETER}";

            var parameters = new Dictionary<string, object>
            {
                { DEVICE_ID_PARAMETER, deviceId }
            };

            var dbCommand = new DatabaseCommand(queryStatement, parameters);

            return await ExecuteCommandWithSingularOutputValue<int>(dbCommand);
        }
    }
}
