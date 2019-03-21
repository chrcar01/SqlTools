using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace SqlTools.SqlServer.Tests
{
    
    public class SqlDbDbHelperTests : BaseDbHelperTest
    {
        public override IDataParameter CreateParameter(string name, object value)
        {
            var @param = new SqlParameter();
            @param.ParameterName = name;
            @param.Value = value;
            return @param;
        }

        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public override IDbHelper CreateHelper()
        {
            return new SqlDbHelper("server=.;database=scratch;trusted_connection=true;");
        }
    }
}
