using System;

namespace SqlTools
{
	public class Tuple<TFirst>
	{
		private TFirst _first = default(TFirst);
		public TFirst First
		{
			get
			{
				return _first;
			}
			set
			{
				_first = value;
			}
		}
		public Tuple()
		{
		}
		public Tuple(TFirst first)
		{
			_first = first;
		}
	}
}
