using System.Data;

namespace Soilution.DataService.SqlDatabase
{
    /// <summary>
    /// Base class for database records.
    /// </summary>
    public abstract class DatabaseRecord
    {
        /// <summary>
        /// Populates properties from a data reader.
        /// </summary>
        public abstract void PopulateFromDataReader(IDataReader reader);
    }
}
