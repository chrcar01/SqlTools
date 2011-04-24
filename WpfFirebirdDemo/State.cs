using GalaSoft.MvvmLight;
using System;

namespace WpfFirebirdDemo
{
	public class State : ViewModelBase
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
				RaisePropertyChanged("Code");
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
				RaisePropertyChanged("Abbreviation");
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
				RaisePropertyChanged("Name");
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
				RaisePropertyChanged("Display");
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
				RaisePropertyChanged("LastUpdated");
			}
		}
        
	}
}
