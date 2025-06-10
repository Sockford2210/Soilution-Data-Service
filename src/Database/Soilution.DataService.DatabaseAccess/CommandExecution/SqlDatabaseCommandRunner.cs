using Microsoft.Extensions.Logging;
using Soilution.DataService.DatabaseAccess.Accessor;
using Soilution.DataService.DatabaseAccess.CommandRunner;
using Soilution.DataService.DataRepository.Models.Base;

namespace Soilution.DataService.DatabaseAccess.CommandExecution
{
    public class SqlDatabaseCommandRunner : ICommandRunner
    {
        private readonly ILogger<SqlDatabaseCommandRunner> _logger;
        private readonly IDatabaseAccessorFactory _accessorFactory;

        public SqlDatabaseCommandRunner(ILogger<SqlDatabaseCommandRunner> logger,
            IDatabaseAccessorFactory accessorFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accessorFactory = accessorFactory ?? throw new ArgumentNullException(nameof(accessorFactory));
        }

        public async Task ExecuteCommandWithNoReturnData(DatabaseCommand command)
        {
            try
            {
                using var accessor = _accessorFactory.GetDatabaseAccessor();

                accessor.OpenConnection();
                await accessor.ExecuteNonQueryAsync(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DatabaseCommandRunner: Failed to execute query");
                throw;
            }
        }

        public async Task<TOutput?> ExecuteCommandWithSingularOutputValue<TOutput>(DatabaseCommand command)
        {
            try
            {
                using var accessor = _accessorFactory.GetDatabaseAccessor();
                accessor.OpenConnection();

                var output = await accessor.ExecuteScalarAsync(command);

                if (output is TOutput outputCast)
                {
                    return outputCast;
                }

                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DatabaseCommandRunner: Failed to execute query");
                throw;
            }
        }

        public async Task<IEnumerable<TOutput>> ExecuteQueryAndReturnDataEnumerator<TOutput>(DatabaseCommand command) where TOutput : QueryResultObject, new()
        {
            try
            {
                using var accessor = _accessorFactory.GetDatabaseAccessor();

                accessor.OpenConnection();

                var readings = new List<TOutput>();
                using var dataReader = await accessor.ExecuteReaderAsync(command);
                while (dataReader.Read())
                {
                    var reading = new TOutput();
                    reading.PopulateFromDataReader(dataReader);
                    readings.Add(reading);
                }

                return readings.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DatabaseCommandRunner: Failed to execute query");
                throw;
            }
        }

        public async Task<TOutput> ExecuteQueryAndReturnSingleRecord<TOutput>(DatabaseCommand command) where TOutput : QueryResultObject, new()
        {
            try
            {
                using var accessor = _accessorFactory.GetDatabaseAccessor();

                accessor.OpenConnection();

                using var dataReader = await accessor.ExecuteReaderAsync(command);
                var reading = new TOutput();
                while (dataReader.Read())
                {
                    reading.PopulateFromDataReader(dataReader);
                    return reading;
                }

                return reading;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DatabaseCommandRunner: Failed to execute query");
                throw;
            }
        }
    }
}
