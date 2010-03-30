using System;
using System.Collections.Generic;
using System.Data;

namespace SqlTools
{
	public interface IDbHelperTuple
	{
		IEnumerable<Tuple<TFirst>> ExecuteTuple<TFirst>(string commandText);
		IEnumerable<Tuple<TFirst>> ExecuteTuple<TFirst>(IDbCommand command);
		IEnumerable<Tuple<TFirst, TSecond>> ExecuteTuple<TFirst, TSecond>(string commandText);
		IEnumerable<Tuple<TFirst, TSecond>> ExecuteTuple<TFirst, TSecond>(IDbCommand command);
		IEnumerable<Tuple<TFirst, TSecond, TThird>> ExecuteTuple<TFirst, TSecond, TThird>(string commandText);
		IEnumerable<Tuple<TFirst, TSecond, TThird>> ExecuteTuple<TFirst, TSecond, TThird>(IDbCommand command);
	}
}
