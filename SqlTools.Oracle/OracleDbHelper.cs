using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OracleClient;

namespace SqlTools
{
	/// <summary>
	/// Oracle implementation of IDbHelper.
	/// </summary>
	public class OracleDbHelper : DbHelperBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OracleDbHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public OracleDbHelper(string connectionString)
			: base(connectionString, DbHelperBase.INITIAL_DEFAULT_COMMAND_TIMEOUT_IN_SECONDS)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="OracleDbHelper"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="defaultCommandTimeoutInSeconds">The default command timeout in seconds. The default for this value is whatever SqlCommand.CommandTimeout returns which is usually 30.</param>
		public OracleDbHelper(string connectionString, int defaultCommandTimeoutInSeconds)
			: base(connectionString, defaultCommandTimeoutInSeconds)
		{	
		}
		/// <summary>
		/// Creates a provider specific implementation of IDbCommand.
		/// </summary>
		/// <returns></returns>
		protected override IDbCommand CreateCommand()
		{
			return new OracleCommand();
		}

		/// <summary>
		/// Creates a provider specific implementation of IDbConnection.
		/// </summary>
		/// <returns></returns>
		protected override IDbConnection CreateConnection()
		{
			var connection = new OracleConnection();
			connection.StateChange += connection_StateChange;
			return connection;
		}

		void connection_StateChange(object sender, StateChangeEventArgs e)
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
			var cmd = command as OracleCommand;
			if (cmd == null)
				throw new InvalidCastException("The command cannot be cast to OracleCommand. The actual type is: " + command.GetType().ToString());

			return new OracleDataAdapter(cmd);
		}
	}
}
