using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace WpfFirebirdDemo
{
	public class MainWindowViewModel : ViewModelBase
	{
		private IMainWindowService _service;
		public MainWindowViewModel(IMainWindowService service)
		{
			_service = service;
			States = _service.GetStates();
		}
		private ObservableCollection<State> _states;
		public ObservableCollection<State> States
		{
			get
			{
				return _states;
			}
			set
			{
				_states = value;
				RaisePropertyChanged("States");
			}
		}


	}
}
