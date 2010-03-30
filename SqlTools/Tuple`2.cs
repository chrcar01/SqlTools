using System;

namespace SqlTools
{
    public class Tuple<TFirst, TSecond> : Tuple<TFirst>
    {
        public Tuple()
            : base()
        {
        }
        public Tuple(TFirst first)
            : base(first)
        {
        }
        public Tuple(TFirst first, TSecond second)
            : this(first)
        {
            this.Second = second;
        }
        private TSecond _second = default(TSecond);
        public TSecond Second
        {
            get
            {
                return _second;
            }
            set
            {
                _second = value;
            }
        }
    }
}
