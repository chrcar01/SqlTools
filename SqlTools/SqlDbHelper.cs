using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;


namespace SqlTools
{
	/// <summary>
	/// Implementation of IDbhelper for SqlServer databases.
	/// </summary>
	public class SqlDbHelper : IDbHelper
	{
		/// <summary>
		/// Initializes a new instance of the SqlDbHelper class.
		/// </summary>
		/// <param name="connectionString"></param>
		public SqlDbHelper(string connectionString)
			: this(connectionString, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the SqlDbHelper class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="defaultCommandTimeoutInSeconds">The default command timeout in seconds. The default for this value is whatever SqlCommand.CommandTimeout returns which is usually 30.</param>
		public SqlDbHelper(string connectionString, int defaultCommandTimeoutInSeconds)
		{
			_connectionString = connectionString;
			_defaultCommandTimeoutInSeconds = defaultCommandTimeoutInSeconds;
		}

		private int _defaultCommandTimeoutInSeconds = default(int);
		/// <summary>
		/// Gets or sets the default command timeout in seconds. 
		/// The default value for this property is the same as the default for SqlCommand's CommandTimeout property.
		/// </summary>
		/// <value>The default command timeout in seconds.</value>
		public int DefaultCommandTimeoutInSeconds
		{
			get
			{
				if (_defaultCommandTimeoutInSeconds == default(int))
				{
					_defaultCommandTimeoutInSeconds = new SqlCommand().CommandTimeout;
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
			IDbConnection result = new SqlConnection(this._connectionString);
			if (initialState == InitialConnectionStates.Open)
				result.Open();			
			return result;
		}

		/// <summary>
		/// Creates the command.
		/// </summary>
		/// <param name="commandText">The command text.</param>
		/// <returns></returns>
		private IDbCommand CreateCommand(string commandText)
		{
			IDbCommand cmd = new SqlCommand(commandText);
			PrepCommand(cmd, GetConnection());
			return cmd;
		}

		private IDbCommand PrepCommand(IDbCommand command)
		{
			IDbConnection cn = command.Connection;
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
			if (command.CommandTimeout == new SqlCommand().CommandTimeout)
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
			var result = new DataTable();
			using (var cn = this.GetConnection())
            {
				PrepCommand(command, cn);
				using (SqlDataAdapter da = new SqlDataAdapter((SqlCommand)command))
                {
					da.Fill(result);
                }
            }
			return result;
		}

		/// <summary>
		/// Executes a query and returns a SqlDataReader containing the resultset.
		/// </summary>
		/// <param name="commandText">The query to execute.</param>
		/// <returns>A SqlDataReader containing the resultset.</returns>
		public IDataReader ExecuteReader(string commandText)
		{
			return this.ExecuteReader(CreateCommand(commandText));
		}

		/// <summary>
		/// Executes a command and returns a SqlDataReader containing the resultset.
		/// </summary>
		/// <param name="command">The command to execute.</param>
		/// <returns>A SqlDataReader containing the resultset.</returns>
		public IDataReader ExecuteReader(IDbCommand command)
		{
			PrepCommand(command, GetConnection());
			return command.ExecuteReader();
		}
		private TResult ChangeType<TResult>(object value)
		{
			return (TResult)ChangeType(value, typeof(TResult));
		}
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

		public IEnumerable<Tuple<TFirst, TSecond>> ExecuteTuple<TFirst, TSecond>(string commandText)
		{
			return ExecuteTuple<TFirst, TSecond>(CreateCommand(commandText));
		}

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

		public IEnumerable<Tuple<TFirst, TSecond, TThird>> ExecuteTuple<TFirst, TSecond, TThird>(string commandText)
		{
			return ExecuteTuple<TFirst, TSecond, TThird>(CreateCommand(commandText));
		}

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
	}
}
