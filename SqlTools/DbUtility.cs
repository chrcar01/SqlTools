using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace SqlTools
{
	/// <summary>
	/// Utility methods for working with DataTables, DataRows, et al.
	/// </summary>
	public static class DbUtility
	{

        /// <summary>
        /// Combines the specified items into a comma separated string that can be used in a SQL IN CLAUSE.
        /// </summary>
        /// <param name="items">The items to combine.</param>
        /// <returns></returns>
        public static string Combine(IEnumerable<int> items)
        {
            return String.Join(",", new List<int>(items).ConvertAll<string>(item => { return item.ToString(); }).ToArray());
        }

		/// <summary>
		/// Builds an array of values for the specified columnName from 
		/// the rows collection.
		/// </summary>
		/// <typeparam name="T">The type of data that exists in the column.</typeparam>
		/// <param name="rows">The DataRowCollection to iterate through.</param>
		/// <param name="columnName">The name of the column in the datatable, from which 
		/// the array values are pulled.</param>
		/// <returns>An array of values of the specified type created from the values
		/// of the specified columnName for all of the rows.</returns>
		public static T[] GetValues<T>(DataRowCollection rows, string columnName)
		{
			int start = 0;
			int length = rows != null ? rows.Count : 0;
			return GetValues<T>(rows, columnName, start, length);
		}

		/// <summary>
		/// Slices a range of values.
		/// </summary>
		/// <typeparam name="T">The type of values that are being retrieved.</typeparam>
		/// <param name="rows">The rows to parse.</param>
		/// <param name="columnName">The name of the column containing the value to parse.</param>
		/// <param name="start">The position of the first elemtn in rows to retrieve.</param>
		/// <param name="length">The number of values to retrieve.</param>
		/// <returns>A strongly typed array of values from a section of rows.</returns>
		public static T[] GetValues<T>(DataRowCollection rows, string columnName, int start, int length)
		{
			DataRow[] rowArray = new DataRow[length];
			for (int i = 0; i < length; i++)
			{
				rowArray[i] = rows[start + i];
			}
			return GetValues<T>(rowArray, columnName);
		}

		/// <summary>
		/// Builds a strongly typed array of values of a single column within an
		/// arrya of DataRows.
		/// </summary>
		/// <typeparam name="T">The type of value in the column to retrieve.</typeparam>
		/// <param name="rows">The array of DataRows to parse.</param>
		/// <param name="columnName">The name of the column from which data values will be retrieved.</param>
		/// <returns>An array of values representing all of the values 
		/// from extracting the columnName column values from the rows array.</returns>
		public static T[] GetValues<T>(DataRow[] rows, string columnName)
		{
			if (rows == null)
				return null;
			Array arr = Array.CreateInstance(typeof(T), rows.Length);
			for (int i = 0; i < rows.Length; i++)
			{
				object value = null;
				if (rows[i][columnName] != null)
					value = (T)rows[i][columnName];
				arr.SetValue(value, i);
			}
			return (T[])arr;
		}

		/// <summary>
		/// Builds a strongly typed array of all of the values from the first column
		/// or all of the rows of the DataTable.
		/// </summary>
		/// <typeparam name="T">The type of data for the resultset.</typeparam>
		/// <param name="table">The DataTable containing all of the values.</param>
		/// <returns>A strongly typed array of values from the first column of every DataRow from 
		/// all rows in the DataTable.</returns>
		public static T[] ToArray<T>(DataTable table)
		{
			DataRow[] rows = table.Select();
			Array arr = Array.CreateInstance(typeof(T), rows.Length);
			for (int i = 0; i < rows.Length; i++)
			{
				object value = null;
				if (rows[i][0] != null)
					value = (T)rows[i][0];
				arr.SetValue(value, i);
			}
			return (T[])arr;
		}

		/// <summary>
		/// Builds an array of values from the column values for all rows
		/// in the columnName column in the DataTable.
		/// </summary>
		/// <typeparam name="T">The type of data in the specified column.</typeparam>
		/// <param name="table">The datatable with all of the values to parse.</param>
		/// <param name="columnName">The name of the column of the DataTable from which all 
		/// values are obtained.</param>
		/// <returns>An array of values from the specified column within the DataTable.</returns>
		public static T[] ToArray<T>(DataTable table, string columnName)
		{
			T[] result = null;
			if (table == null || String.IsNullOrEmpty(columnName))
				return result;

			result = GetValues<T>(table.Rows, columnName, 0, table.Rows.Count);
			return result;
		}

		/// <summary>
		/// Converts a DataRowCollection to an array of DataRows.
		/// </summary>
		/// <param name="rows">The DataRowCollection that will be converted to an
		/// array of DataRows.</param>
		/// <returns>An array of DataRows representing each row of the DataRowCollection.</returns>
		public static DataRow[] ToArray(DataRowCollection rows)
		{
			DataRow[] result = null;
			if (rows == null)
				return result;
			
			result = new DataRow[rows.Count];
			rows.CopyTo(result, 0);
			return result;
		}

        /// <summary>
        /// Creates parameters for each value in <paramref name="values"/>.  Rewrites the command text by replacing <paramref name="name"/>
        /// with the list of parameters created in the method.
        /// </summary>
        /// <param name="command">The command being parameterized.</param>
        /// <param name="name">The name of the variable in the commandText of the command that will be replaced with
        /// the list of variable names.</param>
        /// <param name="values">The values for each parameter.</param>
        public static void Parameterize(SqlCommand command, string name, int[] values)
        {
            var parameterNames = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                var paramName = name + i;
                parameterNames[i] = paramName;
                command.Parameters.Add(paramName, SqlDbType.Int).Value = values[i];
            }
            command.CommandText = command.CommandText.Replace(name, String.Join(",", parameterNames));
        }
    }
}
