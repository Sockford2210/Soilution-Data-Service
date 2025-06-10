using Soilution.DataService.DataRepository.Models.Base;
using System.Data;

namespace Soilution.DataService.UnitTests.DatabaseAccess
{
    internal class TestQueryResultObject : QueryResultObject
    {
        private const string STRING_VALUE_DEFAULT = "NOT SET";
        private const int INT_VALUE_DEFAULT = -1;
        public int IntValue { get; set; } = INT_VALUE_DEFAULT;

        public string StringValue { get; set; } = STRING_VALUE_DEFAULT;

        public bool StringValueSet => StringValue != STRING_VALUE_DEFAULT;
        public bool IntValueSet => IntValue != INT_VALUE_DEFAULT;

        public override void PopulateFromDataReader(IDataReader reader)
        {
            this.IntValue = (int)reader["IntValue"];
            this.StringValue = (string)reader["StringValue"];
        }
    }
}
