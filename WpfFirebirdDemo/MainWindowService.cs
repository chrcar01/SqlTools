using SqlTools;
using System;
using System.Collections.ObjectModel;

namespace WpfFirebirdDemo
{
	public class MainWindowService : IMainWindowService
	{
		private IDbHelper _helper;
		public MainWindowService(IDbHelper helper)
		{
			_helper = helper;
		}
		public ObservableCollection<State> GetStates()
		{
			var result = new ObservableCollection<State>();
			var data = _helper.ExecuteMultiple<State>("select * from state");
			foreach (var item in data)
				result.Add(item);
			return result;
		}
	}
}
