using Microsoft.Extensions.Options;

namespace Soilution.DataService.DatabaseAccess.Accessor
{
    public class SqlDatabaseAccessorFactory : IDatabaseAccessorFactory
    {
        private readonly DatabaseAccessorSettings _settings;

        public SqlDatabaseAccessorFactory(IOptions<DatabaseAccessorSettings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public IDatabaseAccessor GetDatabaseAccessor()
        {
            return new SqlDatabaseAccessor(_settings);
        }
    }
}
