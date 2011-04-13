using System;
using System.Data;

namespace SqlTools
{
	/// <summary>
	/// Specifies common methods and properties for working with databases.
	/// </summary>
	public interface IDbHelper : IDbHelperTuple, IDbHelperMap
	{
		/// <summary>
		/// Gets the connection string for the database.
		/// </summary>
		string ConnectionString { get; }
		/// <summary>
		/// Implementing classes should return an open connection to the database.
		/// </summary>
		/// <returns>An open connection to the database.</returns>
		IDbConnection GetConnection();
		/// <summary>
		/// Gets a connection to the database.
		/// </summary>
		/// <param name="initialState">Initial state of the database connection.</param>
		/// <returns>A connection to the database.</returns>
		IDbConnection GetConnection(InitialConnectionStates initialState);
		/// <summary>
		/// Executes a query and returns the value in the first column of the first row of the resultset.
		/// </summary>
		/// <typeparam name="T">The type of the value returned.</typeparam>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>The value in the first column of the first row of the resultset.</returns>
		T ExecuteScalar<T>(string commandText);
		/// <summary>
		/// Executes a command a returns the value in the first column of the first row of the resultset.
		/// </summary>
		/// <typeparam name="T">The type of the value returned.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <returns>The value in the first column of the first row of the resultset.</returns>
		T ExecuteScalar<T>(IDbCommand command);
		/// <summary>
		/// Executes the query, and returns an array of values from the first column of all rows in the resultset.
		/// </summary>
		/// <typeparam name="TItem">The type of value in the first column.</typeparam>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>An array of values from the first column of all rows in the resultset.</returns>
		TItem[] ExecuteArray<TItem>(string commandText);
		/// <summary>
		/// Executes the command, and returns an array of values from the first column of all rows in the resultset.
		/// </summary>
		/// <typeparam name="TItem">The type of vlaue in the first column.</typeparam>
		/// <param name="command">The query to execute.</param>
		/// <returns>An array of value sfrom the first column of all rows in the resultset.</returns>
		TItem[] ExecuteArray<TItem>(IDbCommand command);

		/// <summary>
		/// Executes a query and returns the number of rows affected.
		/// </summary>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>The number of rows affected.</returns>
		int ExecuteNonQuery(string commandText);
		/// <summary>
		/// Executes a command and returns the number of rows affected.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>The number of rows affected.</returns>
		int ExecuteNonQuery(IDbCommand command);
		/// <summary>
		/// Executes a query and returns the results in a datatable.
		/// </summary>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>A datatable containing the results of executing the query.</returns>
		DataTable ExecuteDataTable(string commandText);
		/// <summary>
		/// Executes a command and returns the results in a datatable.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>A datatable containing the results of executing the command.</returns>
		DataTable ExecuteDataTable(IDbCommand command);
		/// <summary>
		/// Executes a query and returns a data reader containing the results.
		/// </summary>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>A data reader containing the results of executing the query.</returns>
		IDataReader ExecuteReader(string commandText);
		/// <summary>
		/// Executes a command and returns a data reader containing the results.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>A data reader containing the results of executing the command.</returns>
		IDataReader ExecuteReader(IDbCommand command);
		/// <summary>
		/// Changes the connection.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		void ChangeConnection(string connectionString);
		/// <summary>
		/// Occurs when [connection changed].
		/// </summary>
		event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;
		/// <summary>
		/// Gets or sets the default command timeout in seconds. 
		/// The default value for this property is the same as the default for SqlCommand's CommandTimeout property.
		/// </summary>
		/// <value>The default command timeout in seconds.</value>
		int DefaultCommandTimeoutInSeconds { get; set; }

	}
}
