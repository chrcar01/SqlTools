using System;

namespace SqlTools.Tests.Models
{
    public class Blog : IActiveRecord
    {
		public int ID { get; set; }
        public string Name { get; set; }
        public string TagLine { get; set; }
    }
}