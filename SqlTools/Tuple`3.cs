using System;

namespace SqlTools
{
    public class Tuple<TFirst, TSecond, TThird> : Tuple<TFirst, TSecond>
    {
        public Tuple()
        {
        }
        public Tuple(TFirst first)
            : base(first)
        {
        }
        public Tuple(TFirst first, TSecond second)
            : base(first, second)
        {
        }
        public Tuple(TFirst first, TSecond second, TThird third)
            : this(first, second)
        {
            this.Third = third;
        }
        private TThird _third = default(TThird);
        public TThird Third
        {
            get
            {
                return _third;
            }
            set
            {
                _third = value;
            }
        }
    }
}
