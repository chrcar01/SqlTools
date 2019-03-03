using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SqlTools
{
	/// <summary>
	/// Exception thrown when an error occurs in <see cref="DynamicResult"/>.
	/// </summary>
	[Serializable]
	public class DynamicResultException : Exception
	{
		// constructors...
		#region DynamicResultException()
		/// <summary>
		/// Constructs a new DynamicResultException.
		/// </summary>
		public DynamicResultException() { }
		#endregion
		#region DynamicResultException(string message)
		/// <summary>
		/// Constructs a new DynamicResultException.
		/// </summary>
		/// <param name="message">The exception message</param>
		public DynamicResultException(string message) : base(message) { }
		#endregion
		#region DynamicResultException(string message, Exception innerException)
		/// <summary>
		/// Constructs a new DynamicResultException.
		/// </summary>
		/// <param name="message">The exception message</param>
		/// <param name="innerException">The inner exception</param>
		public DynamicResultException(string message, Exception innerException) : base(message, innerException) { }
		#endregion
		#region DynamicResultException(SerializationInfo info, StreamingContext context)
		/// <summary>
		/// Serialization constructor.
		/// </summary>
		protected DynamicResultException(SerializationInfo info, StreamingContext context) : base(info, context) { }
		#endregion
	}
}
