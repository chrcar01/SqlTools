using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlTools
{
#if (NET35)
	/// <summary>
	/// Represents a single item.
	/// </summary>
	/// <typeparam name="T1">The type of item.</typeparam>
	public class Tuple<T1>
	{
		/// <summary>
		/// Gets or sets the item1.
		/// </summary>
		/// <value>
		/// The item1.
		/// </value>
		public T1 Item1 { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple&lt;T1&gt;"/> class.
		/// </summary>
		public Tuple() { }
		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple&lt;T1&gt;"/> class.
		/// </summary>
		/// <param name="item1">The item1.</param>
		public Tuple(T1 item1)
		{
			Item1 = item1;
		}
	}
	/// <summary>
	/// Represents two items.
	/// </summary>
	/// <typeparam name="T1">The type of the 1.</typeparam>
	/// <typeparam name="T2">The type of the 2.</typeparam>
	public class Tuple<T1, T2> : Tuple<T1>
	{
		/// <summary>
		/// Gets or sets the item2.
		/// </summary>
		/// <value>
		/// The item2.
		/// </value>
		public T2 Item2 { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple&lt;T1, T2&gt;"/> class.
		/// </summary>
		public Tuple() : base() { }
		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple&lt;T1, T2&gt;"/> class.
		/// </summary>
		/// <param name="item1">The item1.</param>
		/// <param name="item2">The item2.</param>
		public Tuple(T1 item1, T2 item2) : base(item1)
		{
			Item2 = item2;
		}
	}
	/// <summary>
	/// Represents three items.
	/// </summary>
	/// <typeparam name="T1">The type of the 1.</typeparam>
	/// <typeparam name="T2">The type of the 2.</typeparam>
	/// <typeparam name="T3">The type of the 3.</typeparam>
	public class Tuple<T1, T2, T3> : Tuple<T1, T2>
	{
		/// <summary>
		/// Gets or sets the item3.
		/// </summary>
		/// <value>
		/// The item3.
		/// </value>
		public T3 Item3 { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple&lt;T1, T2, T3&gt;"/> class.
		/// </summary>
		public Tuple() : base() { }
		/// <summary>
		/// Initializes a new instance of the <see cref="Tuple&lt;T1, T2, T3&gt;"/> class.
		/// </summary>
		/// <param name="item1">The item1.</param>
		/// <param name="item2">The item2.</param>
		/// <param name="item3">The item3.</param>
		public Tuple(T1 item1, T2 item2, T3 item3) : base(item1, item2)
		{
			Item3 = item3;
		}
	}
#endif
}
