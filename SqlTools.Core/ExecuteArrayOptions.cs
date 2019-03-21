using System;

namespace SqlTools
{
	/// <summary>
	/// Options that are applied to how arrays are generated in ExecuteArray.
	/// </summary>
	[Flags]
	public enum ExecuteArrayOptions
	{
		/// <summary>
		/// Indicates that no options are set.
		/// </summary>
		None,
		/// <summary>
		/// Indicates that while building arrays, null values are ignored.  Ignoring null values means 
		/// the array will contain fewer elements.
		/// </summary>
		IgnoreNullValues
	}
}
