using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlTools
{
	/// <summary>
	/// Utility methods for working with DataTables, DataRows, et al.
	/// </summary>
	public static class DbUtility
	{
		
		/// <summary>
		/// Creates a separate parameter for each value in the specified values and adds the parameter to the supplied command.
		/// </summary>
		/// <param name="command">The command the parameters are being appended.</param>
		/// <param name="values">The values for the parameters.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		public static void Parameterize(IDbCommand command, ICollection values, string parameterName)
		{
			var list = new ArrayList(values);
			var firstItem = list[0];
			var itemType = firstItem.GetType();
			var dbType = ToSqlDbType(itemType.ToString());
			var maxSizeOfData = 0;
			if (TypeValueRequiresQuotes(itemType))
			{
				foreach(var item in list)
				{
					if (item.ToString().Length > maxSizeOfData)
						maxSizeOfData = item.ToString().Length;
				}
			}
			Parameterize(command, values, parameterName, dbType, maxSizeOfData);
		}

        /// <summary>
		/// Creates a separate parameter for each value in the specified values and adds the parameter to the supplied command.
		/// </summary>
		/// <param name="command">The command the parameters are being appended.</param>
		/// <param name="values">The values for the parameters.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="maxSizeOfData">The max size of data.</param>
		public static void Parameterize(IDbCommand command, ICollection values, string parameterName, int maxSizeOfData)
		{
			var arrayList = new ArrayList(values);
			var firstItem = arrayList[0];
			var dbType = ToSqlDbType(firstItem.GetType().ToString());
			Parameterize(command, values, parameterName, dbType, maxSizeOfData);
		}

		
        /// <summary>
		/// Creates a separate parameter for each value in the specified values and adds the parameter to the supplied command.
		/// </summary>
		/// <param name="command">The command the parameters are being appended.</param>
		/// <param name="values">The values for the parameters.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="dbType">The Sql Server specific data type.</param>
		/// <param name="maxSizeOfData">The max size of data.</param>
		public static void Parameterize(IDbCommand command, ICollection values, string parameterName, SqlDbType dbType, int maxSizeOfData)
		{
			var parameterNames = new string[values.Count];
			var i = 0;
			foreach(var value in values)
			{				
				var parameter = new SqlParameter();
				parameterNames[i] = String.Format("@{0}{1}", parameterName.Replace("@", ""), i);
				parameter.ParameterName = parameterNames[i];
				parameter.SqlDbType = dbType;
				parameter.Size = maxSizeOfData;
				parameter.Value = value;	
				command.Parameters.Add(parameter);
				i++;
			}
			command.CommandText = command.CommandText.Replace(parameterName, String.Join(",", parameterNames));
		}

		/// <summary>
		/// Creates a separate parameter for each value in the specified values and adds the parameter to the supplied command.
		/// </summary>
		/// <param name="this">The command the parameters are being appended.</param>
		/// <param name="values">The values for the parameters.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		public static void AddParameters(this IDbCommand @this, string parameterName, ICollection values)
		{
			DbUtility.Parameterize(@this, values, parameterName);
		}

		/// <summary>
		/// Creates a separate parameter for each value in the specified values and adds the parameter to the supplied command.
		/// </summary>
		/// <param name="this">The command the parameters are being appended.</param>
		/// <param name="values">The values for the parameters.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="maxSizeOfData">The max size of data.</param>
		public static void AddParameters(this IDbCommand @this, string parameterName, ICollection values, int maxSizeOfData)
		{
			DbUtility.Parameterize(@this, values, parameterName, maxSizeOfData);
		}
		/// <summary>
		/// Creates a separate parameter for each value in the specified values and adds the parameter to the supplied command.
		/// </summary>
		/// <param name="this">The command the parameters are being appended.</param>
		/// <param name="values">The values for the parameters.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="dbType">The Sql Server specific data type.</param>
		/// <param name="maxSizeOfData">The max size of data.</param>
		public static void AddParameters(this IDbCommand @this, string parameterName, ICollection values, SqlDbType dbType, int maxSizeOfData)
		{
			DbUtility.Parameterize(@this, values, parameterName, dbType, maxSizeOfData);
		}
		/// <summary>
		/// Creates a separate parameter for each value in the specified values and adds the parameter to the supplied command.
		/// </summary>
		/// <param name="this">The command the parameters are being appended.</param>
		/// <param name="values">The values for the parameters.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="dbType">The Sql Server specific data type.</param>
		public static void AddParameters(this IDbCommand @this, string parameterName, ICollection values, SqlDbType dbType)
		{
			DbUtility.Parameterize(@this, values, parameterName, dbType, 0);
		}
		private static bool TypeValueRequiresQuotes(Type type)
		{
			switch (type.ToString())
			{
				case "System.String":
				case "System.Guid":
				case "System.Char":
				case "System.DateTime": return true;
				default: return false;
			}
		}
		
		private static SqlDbType ToSqlDbType(string clrDataTypeName)
		{
			var dict = new Dictionary<string, SqlDbType>(StringComparer.OrdinalIgnoreCase);
			dict.Add("System.Int64", SqlDbType.BigInt);
			dict.Add("System.Boolean", SqlDbType.Bit);
			dict.Add("System.Char", SqlDbType.Char);
			dict.Add("System.DateTime", SqlDbType.Date);
			dict.Add("System.Decimal", SqlDbType.Decimal);
			dict.Add("System.Double", SqlDbType.Float);
			dict.Add("System.Int32", SqlDbType.Int);
			dict.Add("System.Int16", SqlDbType.SmallInt);
			dict.Add("System.Byte", SqlDbType.TinyInt);
			dict.Add("System.Guid", SqlDbType.UniqueIdentifier);
			dict.Add("System.String", SqlDbType.VarChar);

			if (!dict.ContainsKey(clrDataTypeName))
				throw new KeyNotFoundException(clrDataTypeName + " is not mapped.");

			return dict[clrDataTypeName];
		}

    }
}


