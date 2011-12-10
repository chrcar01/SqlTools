using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Data;

namespace SqlTools
{
	/// <summary>
	/// Represents a type whose members are defined by the results of executing a sql query.
	/// </summary>
	public class DynamicResult : DynamicObject
	{
		//should contain a case insensitive list of property values
		private IDictionary<string, dynamic> _values;
		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicResult"/> class.
		/// </summary>
		/// <param name="columns">The columns in the result set.</param>
		/// <param name="row">The row used to populate this instance.</param>
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
		/// <summary>
		/// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
		/// </summary>
		/// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
		/// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result"/>.</param>
		/// <returns>
		/// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
		/// </returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var key = binder.Name;
			result = _values[key];
			return true;
		}
		/// <summary>
		/// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
		/// </summary>
		/// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
		/// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, the <paramref name="value"/> is "Test".</param>
		/// <returns>
		/// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
		/// </returns>
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
