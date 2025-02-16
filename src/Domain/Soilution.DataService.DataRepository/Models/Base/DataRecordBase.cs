using System.Data;

namespace Soilution.DataService.DataRepository.Models.Base
{
    /// <summary>
    /// Base class for database records.
    /// </summary>
    public abstract class DataRecordBase
    {
        /// <summary>
        /// Populates properties from a data reader.
        /// </summary>
        public abstract void PopulateFromDataReader(IDataReader reader);
    }
}
