using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Soilution.DataService.DataRepository.Models.Base;

namespace Soilution.DataService.SqlRepository
{
    /// <summary>
    /// SQL Database accessor, exposed inherited properties for interfacing with the underlying database.
    /// </summary>
    public class DatabaseAccessor
    {
        private readonly ILogger<DatabaseAccessor> _logger;
        private readonly DatabaseAccessorSettings _settings;

        public DatabaseAccessor(ILogger<DatabaseAccessor> logger, IOptions<DatabaseAccessorSettings> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Execute a single command that does not return and results.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        protected async Task ExecuteCommandWithNoReturnData(DatabaseCommand command)
        {
            try
            {
                using var connection = new SqlConnection(_settings.ConnectionString);

                var sqlCommand = new SqlCommand(command.QueryString, connection);

                foreach (var parameter in command.Parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                sqlCommand.Connection.Open();
                await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to exeucte query");
                throw;
            }
        }

        /// <summary>
        /// Execute a single command that returns an output value
        /// </summary>
        /// <param name="command">Command to execute.</param>
        protected async Task<int> ExecuteCommandWithSingularOutputValue(DatabaseCommand command)
        {
            try
            {
                using var connection = new SqlConnection(_settings.ConnectionString);

                var sqlCommand = new SqlCommand(command.QueryString, connection);

                foreach (var parameter in command.Parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                sqlCommand.Connection.Open();
                return (int)await sqlCommand.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to exeucte query");
                throw;
            }
        }

        /// <summary>
        /// Execute a single command and maps the results onto a <see cref="DataRecordBase"/>
        /// </summary>
        /// <typeparam name="T"><see cref="DataRecordBase"/> to map results onto.</typeparam>
        /// <param name="command">Command to execute.</param>
        protected async Task<IEnumerable<T>> ExecuteQueryAndReturnData<T>(DatabaseCommand command) where T : DataRecordBase, new()
        {
            try
            {
                using var connection = new SqlConnection(_settings.ConnectionString);

                var sqlCommand = new SqlCommand(command.QueryString, connection);

                foreach (var parameter in command.Parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                sqlCommand.Connection.Open();

                var readings = new List<T>();
                using var dataReader = await sqlCommand.ExecuteReaderAsync();
                while (dataReader.Read())
                {
                    var reading = new T();
                    reading.PopulateFromDataReader(dataReader);
                    readings.Add(reading);
                }

                return readings.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute query");
                throw;
            }
        }
    }
}
