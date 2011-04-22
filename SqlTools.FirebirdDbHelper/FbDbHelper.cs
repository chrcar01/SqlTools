using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace SqlTools.FirebirdDbHelper
{
	/// <summary>
	/// FirebirdSql implementation of IDbHelper.
	/// </summary>
	public class FbDbHelper : DbHelperBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FbDbHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public FbDbHelper(string connectionString)
			: base(connectionString, DbHelperBase.INITIAL_DEFAULT_COMMAND_TIMEOUT_IN_SECONDS)
		{			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FbDbHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="defaultCommandTimeoutInSeconds">The default command timeout in seconds. The default for this value is whatever SqlCommand.CommandTimeout returns which is usually 30.</param>
		public FbDbHelper(string connectionString, int defaultCommandTimeoutInSeconds)
			: base(connectionString, defaultCommandTimeoutInSeconds)
		{			
		}
		/// <summary>
		/// Creates a provider specific implementation of IDbCommand.
		/// </summary>
		/// <returns></returns>
		protected override IDbCommand CreateCommand()
		{
			return new FbCommand();
		}

		/// <summary>
		/// Creates a provider specific implementation of IDbConnection.
		/// </summary>
		/// <returns></returns>
		protected override IDbConnection CreateConnection()
		{
			return new FbConnection();
		}

		/// <summary>
		/// Creates a provider specific implementation of IDbDataAdapter.
		/// </summary>
		/// <param name="command">The command used by the IDbDataAdapter.</param>
		/// <returns></returns>
		protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
		{
			var cmd = command as FbCommand;
			if (cmd == null)
				throw new InvalidCastException("The command cannot be cast to FbCommand. The actual type is: " + command.GetType().ToString());

			return new FbDataAdapter(cmd);
		}
	}
}
