using System;
using System.Data.SqlClient;

namespace SqlTools.Tests
{
	public class TestHelper : DbHelperBase
	{
		public TestHelper(string connectionString)
			: base(connectionString, 30, new PropertyDescriptorDataReaderObjectMapper())
		{

		}
		public void Prep(SqlCommand cmd)
		{
			PrepCommand(cmd);
		}
		protected override System.Data.IDbCommand CreateCommand()
		{
			return new SqlCommand();
		}

		protected override System.Data.IDbConnection CreateConnection()
		{
			return new SqlConnection();
		}

		protected override System.Data.IDbDataAdapter CreateDataAdapter(System.Data.IDbCommand command)
		{
			return new SqlDataAdapter(command as SqlCommand);
		}
	}
}
