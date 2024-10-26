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
        public AirQualityDataReadingsSqlRepository(ILogger<DatabaseAccessor> logger, IOptions<DatabaseAccessorSettings> options) : base(logger, options) { }

        /// <inheritdoc/>
        public async Task<int> CreateNewAirQualityRecord(AirQualityDataRecord record)
        {
            string insertStatement = $"INSERT INTO AirQualityDataReadings ({nameof(AirQualityDataRecord.DeviceId)}" +
                $" ,{nameof(AirQualityDataRecord.Timestamp)}" +
                $" ,{nameof(AirQualityDataRecord.HumidityPercentage)}" +
                $" ,{nameof(AirQualityDataRecord.TemperatureCelcius)}" +
                $" ,{nameof(AirQualityDataRecord.CO2PPM)}) " +
                $"OUTPUT INSERTED.{nameof(AirQualityDataRecord.Id)} " +
                $"VALUES (@DeviceId" +
                $" ,@TimeStamp" +
                $" ,@Humidity" +
                $" ,@Temperature" +
                $" ,@CO2)";

            var parameters = new Dictionary<string, object>
            {
                {"@DeviceId", record.DeviceId},
                {"@TimeStamp", record.Timestamp},
                {"@Humidity", record.HumidityPercentage},
                {"@Temperature", record.TemperatureCelcius},
                {"@CO2", record.CO2PPM}
            };

            var dBCommand = new DatabaseCommand(insertStatement, parameters);

            return await ExecuteCommandWithSingularOutputValue(dBCommand);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AirQualityDataRecord>> GetLatestAirQualityRecords(int count)
        {
            string queryStatement = $"SELECT TOP (@readingCount) [{nameof(AirQualityDataRecord.Id)}]" +
                    $" ,[{nameof(AirQualityDataRecord.DeviceId)}]" +
                    $" ,[{nameof(AirQualityDataRecord.Timestamp)}]" +
                    $" ,[{nameof(AirQualityDataRecord.HumidityPercentage)}]" +
                    $" ,[{nameof(AirQualityDataRecord.DeviceId)}]" +
                    $" ,[{nameof(AirQualityDataRecord.CO2PPM)}]" +
                    $" FROM AirQualityDataReadings" +
                    $" ORDER BY [{nameof(AirQualityDataRecord.Timestamp)}] DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@readingCount", count }
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
                    $" WHERE {nameof(AirQualityDataRecord.Timestamp)} >= @fromTimestamp";

            var parameters = new Dictionary<string, object>
            {
                { "@fromTimestamp", fromTimestamp }
            };

            var dBCommand = new DatabaseCommand(queryStatement, parameters);

            var results = await ExecuteQueryAndReturnData<AirQualityDataMaxMinAverage>(dBCommand);

            return results.FirstOrDefault() ?? new AirQualityDataMaxMinAverage();
        }
    }
}
