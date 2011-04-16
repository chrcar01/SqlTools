using System;

namespace SqlTools.Tests.Models
{
	public class State
	{
		private string _code;
		public string Code
		{
			get
			{
				return _code;
			}
			set
			{
				_code = value;
			}
		}
		private string _abbreviation;
		public string Abbreviation
		{
			get
			{
				return _abbreviation;
			}
			set
			{
				_abbreviation = value;
			}
		}
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		private string _display;
		public string Display
		{
			get
			{
				return _display;
			}
			set
			{
				_display = value;
			}
		}
		private DateTime? _lastUpdated;
		public DateTime? LastUpdated
		{
			get
			{
				return _lastUpdated;
			}
			set
			{
				_lastUpdated = value;
			}
		}
        
                
	}
}