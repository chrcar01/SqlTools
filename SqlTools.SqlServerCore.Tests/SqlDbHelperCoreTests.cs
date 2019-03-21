using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace SqlTools.SqlServerCore.Tests
{
    
    public class SqlDbHelperCoreTests : BaseDbHelperTest
    {
        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public override IDataParameter CreateParameter(string name, object value)
        {
            var @param = new SqlParameter();
            @param.ParameterName = name;
            @param.Value = value;
            return @param;
        }

        public override IDbHelper CreateHelper()
        {
            return new SqlDbHelper("server=.;database=scratch;trusted_connection=true;");
        }
    }
}
