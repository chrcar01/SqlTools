using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SqlTools;
using System.Data;

namespace SqlTools.Tests
{
	[TestFixture]
	public class OracleDbHelperTests
	{
		[Test]
		public void CanOpenConnection()
		{
			var helper = CreateHelper();
			helper.ConnectionStateChanged += new EventHandler<ConnectionStateChangedEventArgs>(helper_ConnectionStateChanged);
			using (var cn = helper.GetConnection(InitialConnectionStates.Open))
			{
				Assert.AreEqual(ConnectionState.Open, cn.State);
			}
		}

		void helper_ConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
		{
			Console.WriteLine(e.State);
		}
		private string _oracleConnString = "user id=readapps;password=fcrs2013;Enlist=False;data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=192.8.200.29)(PORT=1556))(CONNECT_DATA=(SERVICE_NAME=FCAEBF)))";
		private IDbHelper CreateHelper()
		{
			return new OracleDbHelper(_oracleConnString);
		}
	}
}
