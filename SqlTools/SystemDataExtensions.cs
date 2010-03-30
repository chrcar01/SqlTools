using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;

namespace SqlTools
{
	public static class SystemDataExtensions
	{
		/// <summary>
		/// Gets the strongly typed value for the columnName out of the current IDataReader.
		/// </summary>
		/// <typeparam name="T">The type of data contained in the specified column.</typeparam>
		/// <param name="reader">The IDataReader instance.</param>
		/// <param name="columnName">Name of the column to read.</param>
		/// <returns></returns>
		public static T GetValue<T>(this IDataReader reader, string columnName)
		{
			#region method contract

			if (reader == null)
				throw new ArgumentNullException("reader", "reader is null.");
			if (String.IsNullOrEmpty(columnName))
				throw new ArgumentException("columnName is null or empty.", "columnName");
			#endregion

			T result = default(T);
			if (reader[columnName] == DBNull.Value)
				return result;

			return (T)ChangeType(reader[columnName], typeof(T));
		}


		/// <summary>
		/// Gets the strongly typed value for the column at the specified index out of the current IDataReader.
		/// </summary>
		/// <typeparam name="T">The type of data contained in the specified column.</typeparam>
		/// <param name="reader">The IDataReader instance.</param>
		/// <param name="index">The position of the column in the IDataReader to read.</param>
		/// <returns></returns>
		public static T GetValue<T>(this IDataReader reader, int index)
		{
			#region method contract
			if (reader == null)
				throw new ArgumentNullException("reader", "reader is null.");
			#endregion

			T result = default(T);
			if (reader.IsDBNull(index))
				return result;

			result = (T)ChangeType(reader.GetValue(index), typeof(T));
			return result;
		}

		/// <summary>
		/// Gets the strongly typed value of the specified columnName in the DataRow.
		/// </summary>
		/// <typeparam name="T">The type of data contained in the specified column.</typeparam>
		/// <param name="row">The DataRow to read.</param>
		/// <param name="columnName">Name of the column to read.</param>
		/// <returns></returns>
		public static T GetValue<T>(this DataRow row, string columnName)
		{
			#region method contract
			if (row == null)
				throw new ArgumentNullException("row", "row is null.");
			if (String.IsNullOrEmpty(columnName))
				throw new ArgumentException("columnName is null or empty.", "columnName");
			#endregion

			T result = default(T);
			if (row.IsNull(columnName))
				return result;

			result = (T)ChangeType(row[columnName], typeof(T));
			return result;
		}

		private static object ChangeType(object value, Type conversionType)
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

	}
}
