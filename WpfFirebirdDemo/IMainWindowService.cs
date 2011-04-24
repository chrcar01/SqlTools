using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace WpfFirebirdDemo
{
	public interface IMainWindowService
	{
		ObservableCollection<State> GetStates();
	}
}
