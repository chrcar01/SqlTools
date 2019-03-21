using System.ComponentModel;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace SqlTools.Oracle.Tests
{
    
    public class OracleDbHelperTests : BaseDbHelperTest
    {
        protected override string SchemaName { get; } = "SCOTT";
        protected override string ParameterSymbol { get; } = ":";

        public override IDbCommand CreateCommand()
        {
            return new OracleCommand();
        }

        public override IDataParameter CreateParameter(string name, object value)
        {
            var @param = new OracleParameter();
            @param.ParameterName = name;
            @param.Value = value;
            return @param;
        }

        public override IDbHelper CreateHelper()
        {
            var connectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=host.docker.internal)(PORT=32769)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCLPDB1.localdomain)));User Id=sqltools;Password=boomsauce";
            return new OracleDbHelper(connectionString);
        }
    }
}
