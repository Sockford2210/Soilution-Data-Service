namespace Soilution.DataService.SqlRepository
{
    /// <summary>
    /// Base class for paramterised commands to be sent to the database.
    /// </summary>
    public class DatabaseCommand
    {
        public DatabaseCommand(string queryString)
        {
            QueryString = queryString;
        }

        public DatabaseCommand(string queryString, Dictionary<string, object> parameters)
        {
            QueryString = queryString;
            Parameters = parameters;
        }

        public string QueryString { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = [];
    }
}
