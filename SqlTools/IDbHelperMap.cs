using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SqlTools
{
	public interface IDbHelperMap
	{
		/// <summary>
		/// Executes the sql statement and attempts to map the first row of the resultset to the
		/// specified type T.  If there are no results, the default for type T is returned.  The names used
		/// in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be mapped to the first row of the resultset.</typeparam>
		/// <param name="commandText">The sql command to execute.</param>
		/// <returns></returns>
		T ExecuteSingle<T>(string commandText) where T : new();

		/// <summary>
		/// Executes the sql command and attempts to map the first row of the resultset to the
		/// specified type T.  If there are no results, the default for type T is returned.  The names used
		/// in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be mapped to the first row of the resultset.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <returns></returns>
		T ExecuteSingle<T>(IDbCommand command) where T : new();

		/// <summary>
		/// Selects all rows from the table with the same name as the type T.  
		/// Attempts to map each row in the resultset to an instance of type T.  
		/// If no results are returned from the query, the method should return null.  
		/// The names used in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be be mapped to each row in the resultset.</typeparam>
		IEnumerable<T> ExecuteMultiple<T>() where T : new();

		/// <summary>
		/// Executes the sql statement and attempts to map each row in the resultset to an instance of type T.  If no
		/// results are returned from the query, the method should return null.  The names used
		/// in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be be mapped to each row in the resultset.</typeparam>
		/// <param name="commandText">The sql command execute.</param>
		/// <returns></returns>
		IEnumerable<T> ExecuteMultiple<T>(string commandText) where T : new();

		/// <summary>
		/// Executes the sql command and attempts to map each row in the resultset to an instance of type T.  If no
		/// results are returned from the query, the method should return null.  The names used
		/// in the query MUST match the property names of the type T in order for the mapping to work.
		/// </summary>
		/// <typeparam name="T">Represents the type that will be be mapped to each row in the resultset.</typeparam>
		/// <param name="command">The command to execute.</param>
		/// <returns></returns>
		IEnumerable<T> ExecuteMultiple<T>(IDbCommand command) where T : new();
	}
}
