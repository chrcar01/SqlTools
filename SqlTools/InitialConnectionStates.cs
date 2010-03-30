using System;

namespace SqlTools
{
	/// <summary>
	/// The state of a connection.
	/// </summary>
	public enum InitialConnectionStates
	{
		/// <summary>
		/// The connection is open.
		/// </summary>
		Open,
		/// <summary>
		/// The connection is closed.
		/// </summary>
		Closed
	}
}
