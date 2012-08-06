using System;

namespace SqlTools
{
	/// <summary>
	/// State of the current connection, depends on the provider.
	/// </summary>
	public enum ConnectionStates
	{
		/// <summary>
		/// The current connection is open.
		/// </summary>
		Open,
		/// <summary>
		/// The current connection is closed.
		/// </summary>
		Closed
	}
}
