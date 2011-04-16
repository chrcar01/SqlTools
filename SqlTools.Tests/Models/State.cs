using System;

namespace SqlTools.Tests.Models
{
	public class State
	{
		public string Code { get; set; }
		public string Abbreviation { get; set; }
		public string Name { get; set; }
		public string Display { get; set; }
		public DateTime? LastUpdated { get; set; }        
	}
}