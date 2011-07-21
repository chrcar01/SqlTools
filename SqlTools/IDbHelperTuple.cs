using System;
using System.Collections.Generic;
using System.Data;

namespace SqlTools
{
	/// <summary>
	/// Defines methods used for returning strongly typed resultsets.
	/// </summary>
	public interface IDbHelperTuple
	{
        /// <summary>
        /// Executes the sql and returns a Tuple with one strongly typed property mapped to the first column in the resultset.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
		IEnumerable<Tuple<TFirst>> ExecuteTuple<TFirst>(string commandText);
        /// <summary>
        /// Executes the tuple.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
		IEnumerable<Tuple<TFirst>> ExecuteTuple<TFirst>(IDbCommand command);
        /// <summary>
        /// Executes the tuple.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TSecond">The type of the second.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
		IEnumerable<Tuple<TFirst, TSecond>> ExecuteTuple<TFirst, TSecond>(string commandText);
        /// <summary>
        /// Executes the tuple.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TSecond">The type of the second.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
		IEnumerable<Tuple<TFirst, TSecond>> ExecuteTuple<TFirst, TSecond>(IDbCommand command);
        /// <summary>
        /// Executes the tuple.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TSecond">The type of the second.</typeparam>
        /// <typeparam name="TThird">The type of the third.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
		IEnumerable<Tuple<TFirst, TSecond, TThird>> ExecuteTuple<TFirst, TSecond, TThird>(string commandText);
        /// <summary>
        /// Executes the tuple.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first.</typeparam>
        /// <typeparam name="TSecond">The type of the second.</typeparam>
        /// <typeparam name="TThird">The type of the third.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
		IEnumerable<Tuple<TFirst, TSecond, TThird>> ExecuteTuple<TFirst, TSecond, TThird>(IDbCommand command);
	}
}
