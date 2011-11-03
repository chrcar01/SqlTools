using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Data;

namespace SqlTools
{
	public class DynamicResult : DynamicObject
	{
		//should contain a case insensitive list of property values
		private IDictionary<string, dynamic> _values;
		public DynamicResult(DataColumnCollection columns, DataRow row)
		{
			_values = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);
			foreach (DataColumn col in columns)
			{
				if (row[col.ColumnName] == System.DBNull.Value)
				{
					_values.Add(col.ColumnName, null);
				}
				else
				{
					_values.Add(col.ColumnName, row[col.ColumnName]);
				}
			}
		}
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var key = binder.Name;
			result = _values[key];
			return true;
		}
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (_values.ContainsKey(binder.Name))
			{
				_values[binder.Name] = value;
			}
			else
			{
				_values.Add(binder.Name, value);
			}
			return true;
		}
	}
}
