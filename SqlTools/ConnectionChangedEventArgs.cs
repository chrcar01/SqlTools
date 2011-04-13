using System;

namespace SqlTools
{
	/// <summary>
	/// Represents the connection string change information passed to the <seealso cref="ConnectionChanged"/> event.
	/// </summary>
	public class ConnectionChangedEventArgs : System.EventArgs
	{
		public string OldConnectionString { get; private set; }
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
