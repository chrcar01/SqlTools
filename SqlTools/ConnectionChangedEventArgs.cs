using System;

namespace SqlTools
{
	/// <summary>
	/// Args passed the ConnectionStateChanged event handler.
	/// </summary>
	public class ConnectionStateChangedEventArgs : System.EventArgs
	{
		/// <summary>
		/// Gets the state of the connection.
		/// </summary>
		public ConnectionStates State { get; private set; }
		/// <summary>
		/// Initializes a new instance of the ConnectionStateChangedEventArgs class.
		/// </summary>
		public ConnectionStateChangedEventArgs(ConnectionStates state)
		{
			State = state;
		}
	}
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
