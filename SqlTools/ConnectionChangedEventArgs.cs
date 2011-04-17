using System;

namespace SqlTools
{
	/// <summary>
	/// Represents the connection string change information passed to the ConnectionChanged event.
	/// </summary>
	public class ConnectionChangedEventArgs : System.EventArgs
	{
		/// <summary>
		/// Gets the old connection string.
		/// </summary>
		/// <value>The old connection string.</value>
		public string OldConnectionString { get; private set; }
		/// <summary>
		/// Gets the new connection string.
		/// </summary>
		/// <value>The new connection string.</value>
		public string NewConnectionString { get; private set; }
		/// <summary>
		/// Initializes a new instance of the ConnectionChangedEventArgs class.
		/// </summary>
		public ConnectionChangedEventArgs(string oldConnectionString, string newConnectionString)
		{
			OldConnectionString = oldConnectionString;
			NewConnectionString = newConnectionString;
		}
	}
}
