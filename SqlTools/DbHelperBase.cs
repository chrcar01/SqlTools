using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace SqlTools
{
	/// <summary>
	/// Implementation of IDbhelper for SqlServer databases.
	/// </summary>
	public abstract class DbHelperBase : IDbHelper
	{
		/// <summary>
		/// The initial value[Int32.MinValue] for DefaultCommandTimeoutInSeconds.
		/// </summary>
		public static readonly int INITIAL_DEFAULT_COMMAND_TIMEOUT_IN_SECONDS = Int32.MinValue;

		/// <summary>
		/// The initial value[Int32.MinValue] for CommandProviderCommandTimeout.
		/// </summary>
		public static readonly int INITIAL_COMMAND_PROVIDER_COMMAND_TIMEOUT = Int32.MinValue;

		/// <summary>
		/// Creates a provider specific implementation of IDbCommand.
		/// </summary>
		/// <returns></returns>
		protected abstract IDbCommand CreateCommand();


		/// <summary>
		/// Creates a provider specific implementation of IDbConnection.
		/// </summary>
		/// <returns></returns>
		protected abstract IDbConnection CreateConnection();


		/// <summary>
		/// Creates a provider specific implementation of IDbDataAdapter.
		/// </summary>
		/// <param name="command">The command used by the IDbDataAdapter.</param>
		/// <returns></returns>
		protected abstract IDbDataAdapter CreateDataAdapter(IDbCommand command);


        /// <summary>
		/// Initializes a new instance of the SqlDbHelper class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="defaultCommandTimeoutInSeconds">The default command timeout in seconds. The default for this value is whatever SqlCommand.CommandTimeout returns which is usually 30.</param>
		public DbHelperBase(string connectionString, int defaultCommandTimeoutInSeconds)
		{
			_connectionString = connectionString;
			_defaultCommandTimeoutInSeconds = defaultCommandTimeoutInSeconds;
		}


		private int _commandProviderCommandTimeout = INITIAL_COMMAND_PROVIDER_COMMAND_TIMEOUT;
		private int CommandProviderCommandTimeout
		{
			get
			{
				if (_commandProviderCommandTimeout == INITIAL_COMMAND_PROVIDER_COMMAND_TIMEOUT)
				{
					_commandProviderCommandTimeout = CreateCommand().CommandTimeout;
				}
				return _commandProviderCommandTimeout;
			}
		}

		private int _defaultCommandTimeoutInSeconds = INITIAL_DEFAULT_COMMAND_TIMEOUT_IN_SECONDS;
		/// <summary>
		/// Gets or sets the default command timeout in seconds. 
		/// The default value for this property is the same as the default for IDbCommand implementor's CommandTimeout property.
		/// </summary>
		/// <value>The default command timeout in seconds.</value>
		public int DefaultCommandTimeoutInSeconds
		{
			get
			{
				if (_defaultCommandTimeoutInSeconds == INITIAL_DEFAULT_COMMAND_TIMEOUT_IN_SECONDS)
				{
					_defaultCommandTimeoutInSeconds = CommandProviderCommandTimeout;
				}
				return _defaultCommandTimeoutInSeconds;
			}
			set
			{
				_defaultCommandTimeoutInSeconds = value;
			}
		}

		private string _connectionString;
		/// <summary>
		/// Gets the connection string to the database.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}
		}

		/// <summary>
		/// Opens a connection to the database.
		/// </summary>
		/// <returns>An open connection to the database.</returns>
		public IDbConnection GetConnection()
		{
			return this.GetConnection(InitialConnectionStates.Open);
		}


		/// <summary>
		/// Gets a connection to the database.
		/// </summary>
		/// <param name="initialState">Indicates the state of the connection returned.</param>
		/// <returns>A connection to the database, either open or closed.</returns>
		public IDbConnection GetConnection(InitialConnectionStates initialState)
		{
			IDbConnection result = CreateConnection();
			RaiseConnectionCreated();
			result.ConnectionString = ConnectionString;
			if (initialState == InitialConnectionStates.Open)
				result.Open();
						
			return result;
		}

		private IDbCommand CreateCommand(string commandText)
		{
			var cmd = CreateCommand();
			cmd.CommandText = commandText;
			PrepCommand(cmd, GetConnection());
			return cmd;
		}

		/// <summary>
		/// Preps the command.  Currently this just sets the appropriate command timeout.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		protected IDbCommand PrepCommand(IDbCommand command)
		{
			var cn = command.Connection;
			return PrepCommand(command, cn);
		}

		private IDbCommand PrepCommand(IDbCommand command, IDbConnection cn)
		{
			if (cn != null)
			{
				command.Connection = cn;
			}
			// Respect user defined CommandTimeout... only use the DefaultCommandTimeoutInSeconds 
			// if a custom value has not been already set.
			if (command.CommandTimeout == CommandProviderCommandTimeout)
			{
				command.CommandTimeout = DefaultCommandTimeoutInSeconds;
			}
			return command;
		}


		/// <summary>
		/// Executes the query, and returns the first column of the first row of the resultset.
		/// </summary>
		/// <typeparam name="T">The type of the data returned.</typeparam>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>The first column of the first row of the result of executeing the query.</returns>
		public T ExecuteScalar<T>(string commandText)
		{
			return this.ExecuteScalar<T>(CreateCommand(commandText));
		}



		/// <summary>
		/// Executes the command, and returns the first column of the first row of the resultset.
		/// </summary>
		/// <typeparam name="T">The type of the data returned.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <returns>The first colunm of the first row of the result of executing the command.</returns>
		public T ExecuteScalar<T>(IDbCommand command)
		{
			T result = default(T);
			using (var cn = GetConnection())
			{
				PrepCommand(command, cn);
				object queryResult = command.ExecuteScalar();
				if (queryResult != null && queryResult != DBNull.Value)
					result = ChangeType<T>(queryResult);
			}
			return result;
		}

		/// <summary>
		/// Executes the query and returns an array of all of the values of the first column of all rows in the resultset.
		/// </summary>
		/// <typeparam name="TItem">The type of data in the first column of each row.</typeparam>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>
		/// An array of all of the values of the first column of all of the rows in the resultset.
		/// </returns>
		public TItem[] ExecuteArray<TItem>(string commandText)
		{
			ExecuteArrayOptions options = ExecuteArrayOptions.None;
			return ExecuteArray<TItem>(commandText, options);
		}
		/// <summary>
		/// Executes the query and returns an array of all of the values of the first column of all rows in the resultset.
		/// </summary>
		/// <typeparam name="TItem">The type of data in the first column of each row.</typeparam>
		/// <param name="commandText">The query to execute.</param>
		/// <param name="options">The options that are applied to how arrays are created.</param>
		/// <returns>
		/// An array of all of the values of the first column of all of the rows in the resultset.
		/// </returns>
		public TItem[] ExecuteArray<TItem>(string commandText, ExecuteArrayOptions options)
		{
			return this.ExecuteArray<TItem>(CreateCommand(commandText), options);
		}

		/// <summary>
		/// Executes the command and returns an array of all of the values of the first column of all rows in the resultset.
		/// </summary>
		/// <typeparam name="TItem">The type of data in the first column of each row.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <returns>
		/// An array of all of the values of the first column of all of the rows in the resultset.
		/// </returns>
		public TItem[] ExecuteArray<TItem>(IDbCommand command)
		{
			ExecuteArrayOptions options = ExecuteArrayOptions.None;
			return ExecuteArray<TItem>(command, options);
		}
		/// <summary>
		/// Executes the command and returns an array of all of the values of the first column of all rows in the resultset.
		/// </summary>
		/// <typeparam name="TItem">The type of data in the first column of each row.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <param name="options">The options that are applied to how arrays are created.</param>
		/// <returns>
		/// An array of all of the values of the first column of all of the rows in the resultset.
		/// </returns>
		public TItem[] ExecuteArray<TItem>(IDbCommand command, ExecuteArrayOptions options)
		{
			var result = new List<TItem>();
			using (var reader = this.ExecuteReader(command))
			{
				while (reader.Read())
				{
					var item = reader.GetValue(0);
					if (ExecuteArrayOptions.IgnoreNullValues == options && (item == System.DBNull.Value || item == null))
						continue;

					var value = ChangeType<TItem>(item);
					result.Add(value);
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Executes the query and returns the number of rows affected.
		/// </summary>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>The number of rows affected.</returns>
		public int ExecuteNonQuery(string commandText)
		{
			return this.ExecuteNonQuery(CreateCommand(commandText));
		}

		/// <summary>
		/// Executes the command and returns the number of rows affected.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>The number of rows affected.</returns>
		public int ExecuteNonQuery(IDbCommand command)
		{
			using (var cn = this.GetConnection())
			{
				PrepCommand(command, cn);
				return command.ExecuteNonQuery();
			}

		}

		/// <summary>
		/// Executes the query and returns a DataTable filled with the results.
		/// </summary>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>DataTable containing the results of executing the query.</returns>
		public DataTable ExecuteDataTable(string commandText)
		{
			return this.ExecuteDataTable(CreateCommand(commandText));
		}

		/// <summary>
		/// Executes the command and returns a DataTable filled with the results.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>DataTable containing the results of executing the command.</returns>
		public DataTable ExecuteDataTable(IDbCommand command)
		{
			var result = new DataSet();
			using (var cn = this.GetConnection())
			{
				PrepCommand(command, cn);
				var da = CreateDataAdapter(command);
				da.Fill(result);
			}
			return result.Tables[0];
		}

		/// <summary>
		/// Executes a query and returns a data reader containing the results.
		/// Implementors should use CommandBehavior.CloseConnection as the default behavior.
		/// </summary>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>
		/// A data reader containing the results of executing the query.
		/// </returns>
		public IDataReader ExecuteReader(string commandText)
		{
			return this.ExecuteReader(CreateCommand(commandText), CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Executes a query and returns a SqlDataReader containing the resultset.
		/// </summary>
		/// <param name="commandText">The query to execute.</param>
		/// <param name="behavior">Effects of executing the command on the connection.</param>
		/// <returns>
		/// A SqlDataReader containing the resultset.
		/// </returns>
		public IDataReader ExecuteReader(string commandText, CommandBehavior behavior)
		{
			return this.ExecuteReader(CreateCommand(commandText), behavior);
		}

		/// <summary>
		/// Executes a command and returns a data reader containing the results.
		/// Implementors should use CommandBehavior.CloseConnection as the default behavior.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>
		/// A data reader containing the results of executing the command.
		/// </returns>
		public IDataReader ExecuteReader(IDbCommand command)
		{
			return ExecuteReader(command, CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Executes a command and returns a data reader containing the results.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <param name="behavior">Effects of executing the command on the connection.</param>
		/// <returns>
		/// A data reader containing the results of executing the command.
		/// </returns>
		public IDataReader ExecuteReader(IDbCommand command, CommandBehavior behavior)
		{
			PrepCommand(command, GetConnection());
			return command.ExecuteReader(behavior);
		}
		private TResult ChangeType<TResult>(object value)
		{
			return (TResult)ChangeType(value, typeof(TResult));
		}

		/// <summary>
		/// Returns an Object with the specified Type and whose value is equivalent to the specified object.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="conversionType">Type of the conversion.</param>
		/// <remarks>
		/// This method was written by Peter Johnson at:
		/// http://aspalliance.com/author.aspx?uId=1026.
		/// </remarks>
		/// <returns></returns>
		private object ChangeType(object value, Type conversionType)
		{
			// Note: This if block was taken from Convert.ChangeType as is, and is needed here since we're
			// checking properties on conversionType below.
			if (conversionType == null)
			{
				throw new ArgumentNullException("conversionType");
			} // end if

			// If it's not a nullable type, just pass through the parameters to Convert.ChangeType

			if (conversionType.IsGenericType &&
			  conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				// It's a nullable type, so instead of calling Convert.ChangeType directly which would throw a
				// InvalidCastException (per http://weblogs.asp.net/pjohnson/archive/2006/02/07/437631.aspx),
				// determine what the underlying type is
				// If it's null, it won't convert to the underlying type, but that's fine since nulls don't really
				// have a type--so just return null
				// Note: We only do this check if we're converting to a nullable type, since doing it outside
				// would diverge from Convert.ChangeType's behavior, which throws an InvalidCastException if
				// value is null and conversionType is a value type.
				if (value == null)
				{
					return null;
				} // end if

				// It's a nullable type, and not null, so that means it can be converted to its underlying type,
				// so overwrite the passed-in conversion type with this underlying type
				NullableConverter nullableConverter = new NullableConverter(conversionType);
				conversionType = nullableConverter.UnderlyingType;
			} // end if

			// Now that we've guaranteed conversionType is something Convert.ChangeType can handle (i.e. not a
			// nullable type), pass the call on to Convert.ChangeType
			return Convert.ChangeType(value, conversionType);
		}


		/// <summary>
		/// Executes the sql and returns a resultset of tuple
		/// </summary>
		/// <typeparam name="TFirst">The type of the first.</typeparam>
		/// <param name="commandText">The command text.</param>
		/// <returns></returns>
		public IEnumerable<Tuple<TFirst>> ExecuteTuple<TFirst>(string commandText)
		{
			return ExecuteTuple<TFirst>(CreateCommand(commandText));
		}

		/// <summary>
		/// Executes command and returns a collection one dimensional tuple.
		/// </summary>
		/// <typeparam name="TFirst">The type of the first.</typeparam>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		public IEnumerable<Tuple<TFirst>> ExecuteTuple<TFirst>(IDbCommand command)
		{
			List<Tuple<TFirst>> tuples = new List<Tuple<TFirst>>();
			using (IDataReader reader = ExecuteReader(command))
			{
				while (reader.Read())
				{
					if (!reader.IsDBNull(0)) tuples.Add(new Tuple<TFirst>(reader.GetValue<TFirst>(0)));
				}
			}
			return tuples;
		}

		/// <summary>
		/// Executes the tuple.
		/// </summary>
		/// <typeparam name="TFirst">The type of the first.</typeparam>
		/// <typeparam name="TSecond">The type of the second.</typeparam>
		/// <param name="commandText">The command text.</param>
		/// <returns></returns>
		public IEnumerable<Tuple<TFirst, TSecond>> ExecuteTuple<TFirst, TSecond>(string commandText)
		{
			return ExecuteTuple<TFirst, TSecond>(CreateCommand(commandText));
		}

		/// <summary>
		/// Executes the tuple.
		/// </summary>
		/// <typeparam name="TFirst">The type of the first.</typeparam>
		/// <typeparam name="TSecond">The type of the second.</typeparam>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		public IEnumerable<Tuple<TFirst, TSecond>> ExecuteTuple<TFirst, TSecond>(IDbCommand command)
		{
			List<Tuple<TFirst, TSecond>> tuples = new List<Tuple<TFirst, TSecond>>();
			using (IDataReader reader = ExecuteReader(command))
			{
				while (reader.Read())
				{
					var tuple = new Tuple<TFirst, TSecond>(reader.GetValue<TFirst>(0), reader.GetValue<TSecond>(1));
					tuples.Add(tuple);
				}
			}
			return tuples;
		}

		/// <summary>
		/// Executes the tuple.
		/// </summary>
		/// <typeparam name="TFirst">The type of the first.</typeparam>
		/// <typeparam name="TSecond">The type of the second.</typeparam>
		/// <typeparam name="TThird">The type of the third.</typeparam>
		/// <param name="commandText">The command text.</param>
		/// <returns></returns>
		public IEnumerable<Tuple<TFirst, TSecond, TThird>> ExecuteTuple<TFirst, TSecond, TThird>(string commandText)
		{
			return ExecuteTuple<TFirst, TSecond, TThird>(CreateCommand(commandText));
		}

		/// <summary>
		/// Executes the tuple.
		/// </summary>
		/// <typeparam name="TFirst">The type of the first.</typeparam>
		/// <typeparam name="TSecond">The type of the second.</typeparam>
		/// <typeparam name="TThird">The type of the third.</typeparam>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		public IEnumerable<Tuple<TFirst, TSecond, TThird>> ExecuteTuple<TFirst, TSecond, TThird>(IDbCommand command)
		{
			var tuples = new List<Tuple<TFirst, TSecond, TThird>>();
			using (var reader = ExecuteReader(command))
			{
				while (reader.Read())
				{
					var tuple = new Tuple<TFirst, TSecond, TThird>(reader.GetValue<TFirst>(0), reader.GetValue<TSecond>(1), reader.GetValue<TThird>(2));
					tuples.Add(tuple);
				}
			}
			return tuples;
		}


		/// <summary>
		/// Executes the sql statement and attempts to map the first row of the resultset to the
		/// specified type T.  If there are no results, the default for type T is returned.  The names used
		/// in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be mapped to the first row of the resultset.</typeparam>
		/// <param name="commandText">The sql command to execute.</param>
		/// <returns></returns>
		public T ExecuteSingle<T>(string commandText) where T : new()
		{
			return ExecuteSingle<T>(CreateCommand(commandText));
		}

		/// <summary>
		/// Executes the sql command and attempts to map the first row of the resultset to the
		/// specified type T.  If there are no results, the default for type T is returned.  The names used
		/// in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be mapped to the first row of the resultset.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <returns></returns>
		public T ExecuteSingle<T>(IDbCommand command) where T : new()
		{
			T result = default(T);
			using (var data = ExecuteDataTable(command))
			{
				if (data.Rows.Count > 0)
					result = CreateFromRow<T>(data.Rows[0]);
			}
			return result;
		}

		/// <summary>
		/// Selects all rows from the table with the same name as the type T.
		/// Attempts to map each row in the resultset to an instance of type T.
		/// If no results are returned from the query, the method should return null.
		/// The names used in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be be mapped to each row in the resultset.</typeparam>
		/// <returns></returns>
		public IEnumerable<T> ExecuteMultiple<T>() where T : new()
		{
			return ExecuteMultiple<T>(String.Format("select * from [{0}]", typeof(T).Name));
		}


		/// <summary>
		/// Executes the sql statement and attempts to map each row in the resultset to an instance of type T.  If no
		/// results are returned from the query, the method should return null.  The names used
		/// in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be be mapped to each row in the resultset.</typeparam>
		/// <param name="commandText">The sql command execute.</param>
		/// <returns></returns>
		public IEnumerable<T> ExecuteMultiple<T>(string commandText) where T : new()
		{
			return ExecuteMultiple<T>(CreateCommand(commandText));
		}

		/// <summary>
		/// Executes the sql command and attempts to map each row in the resultset to an instance of type T.  If no
		/// results are returned from the query, the method should return null.  The names used
		/// in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be be mapped to each row in the resultset.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <returns></returns>
		public IEnumerable<T> ExecuteMultiple<T>(IDbCommand command) where T : new()
		{
			List<T> result = null;
			using (var data = ExecuteDataTable(command))
			{
				if (data.Rows.Count > 0)
				{
					foreach (DataRow row in data.Rows)
					{
						if (result == null) result = new List<T>();
						result.Add(CreateFromRow<T>(row));
					}
				}
			}
			return result;
		}

		private T CreateFromRow<T>(DataRow row) where T : new()
		{
			T result = new T();
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(result);
			foreach (PropertyDescriptor prop in props)
			{
				if (!row.Table.Columns.Contains(prop.Name) || row.IsNull(prop.Name))
					continue;

				prop.SetValue(result, ChangeType(row[prop.Name], prop.PropertyType));
			}

			return result;
		}

		/// <summary>
		/// Changes the connection.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public void ChangeConnection(string connectionString)
		{
			if (String.IsNullOrEmpty(connectionString))
				throw new ArgumentNullException("connectionString is null or empty.", "connectionString");

			_connectionString = connectionString;
		}
		/// <summary>
		/// Occurs when [connection state changed].
		/// </summary>
		public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
		/// <summary>
		/// Raises the connection state changed event.
		/// </summary>
		/// <param name="state">The state.</param>
		protected virtual void RaiseConnectionStateChanged(ConnectionStates state)
		{
			if (ConnectionStateChanged == null) return;
			ConnectionStateChanged(this, new ConnectionStateChangedEventArgs(state));
		}
		/// <summary>
		/// Occurs when [connection changed].
		/// </summary>
		public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;

		/// <summary>
		/// Raises the connection changed event.
		/// </summary>
		/// <param name="oldConnectionString">The old connection string.</param>
		/// <param name="newConnectionString">The new connection string.</param>
		protected virtual void RaiseConnectionChanged(string oldConnectionString, string newConnectionString)
		{
			if (ConnectionChanged == null) return;
			ConnectionChanged(this, new ConnectionChangedEventArgs(oldConnectionString, newConnectionString));
		}

		/// <summary>
		/// Occurs when [connection created].
		/// </summary>
		public event EventHandler ConnectionCreated;

		/// <summary>
		/// Raises the connection created event.
		/// </summary>
		protected virtual void RaiseConnectionCreated()
		{
			if (ConnectionCreated == null) return;
			ConnectionCreated(this, new EventArgs());
		}
#if (!NET35)
		/// <summary>
		/// Executes sql, and returns a strongly typed instance of a class contructed at runtime containing the values of the
		/// first row in the resultset.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns></returns>
		public dynamic ExecuteDynamic(string sql)
		{
			return ExecuteDynamic(CreateCommand(sql));
		}
		/// <summary>
		/// Executes command, and returns a strongly typed instance of a class contructed at runtime containing the values of the
		/// first row in the resultset.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		public dynamic ExecuteDynamic(IDbCommand command)
		{
			var data = ExecuteDataTable(command);
			return new DynamicResult(data.Columns, data.Rows[0]);
		}
		/// <summary>
		/// Executes the sql and returns a list of dynamically created objects representing each row in the resultset.
		/// </summary>
		/// <param name="sql">The SQL.</param>
		/// <returns></returns>
		public IEnumerable<dynamic> ExecuteDynamics(string sql)
		{
			return ExecuteDynamics(CreateCommand(sql));
		}
		/// <summary>
		/// Executes the sql and returns a list of dynamically created objects representing each row in the resultset.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		public IEnumerable<dynamic> ExecuteDynamics(IDbCommand command)
		{
			var result = new List<dynamic>();
			var data = ExecuteDataTable(command);
			if (data == null || data.Rows.Count == 0)
				return result;

			foreach (DataRow row in data.Rows)
			{
				result.Add(new DynamicResult(data.Columns, row));
			}
			return result;
		}
#endif

		
	}
}
