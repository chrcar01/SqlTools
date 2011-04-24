using SqlTools.FirebirdDbHelper;
using System;
using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace WpfFirebirdDemo
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Loaded += (x, y) =>
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///","");
				var databaseFilePath = codeBase.Substring(0, codeBase.IndexOf("/bin/Debug")) + "/SQLTOOLS.FBD";
				var connString = String.Format("Database={0};user id=SYSDBA;password=masterkey;", databaseFilePath);
				var helper = new FbDbHelper(connString);
				var service = new MainWindowService(helper);
				DataContext = new MainWindowViewModel(service);
			};
		}
	}
}
