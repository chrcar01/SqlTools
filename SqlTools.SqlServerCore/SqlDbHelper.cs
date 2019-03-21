using System;
using System.Data;
using System.Data.SqlClient;

namespace SqlTools.SqlServerCore
{
	/// <summary>
	/// Sql Server specific implementation of IDbHelper.
	/// </summary>
	public class SqlDbHelper : DbHelperBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlDbHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public SqlDbHelper(string connectionString)
			: base(connectionString, DbHelperBase.INITIAL_DEFAULT_COMMAND_TIMEOUT_IN_SECONDS)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="SqlDbHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="defaultCommandTimeoutInSeconds">The default command timeout in seconds. The default for this value is whatever SqlCommand.CommandTimeout returns which is usually 30.</param>
		public SqlDbHelper(string connectionString, int defaultCommandTimeoutInSeconds)
			: base(connectionString, defaultCommandTimeoutInSeconds)
		{	
		}

		/// <summary>
		/// Creates a provider specific implementation of IDbCommand.
		/// </summary>
		/// <returns></returns>
		protected override IDbCommand CreateCommand()
		{
			return new SqlCommand();
		}


		/// <summary>
		/// Creates a provider specific implementation of IDbConnection.
		/// </summary>
		/// <returns></returns>
		protected override IDbConnection CreateConnection()
		{
			var connection = new SqlConnection();
			connection.StateChange += connection_StateChange;
			return connection;
		}

		private void connection_StateChange(object sender, StateChangeEventArgs e)
		{
			var state = e.CurrentState == ConnectionState.Open ? ConnectionStates.Open : ConnectionStates.Closed;
			RaiseConnectionStateChanged(state);
		}

		/// <summary>
		/// Creates a provider specific implementation of IDbDataAdapter.
		/// </summary>
		/// <param name="command">The command used by the IDbDataAdapter.</param>
		/// <returns></returns>
		protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
		{
			var cmd = command as SqlCommand;
			if (cmd == null)
				throw new InvalidCastException("The command cannot be cast to SqlCommand. The actual type is: " + command.GetType().ToString());

			return new SqlDataAdapter(cmd);
		}
	}
}
