using NUnit.Framework;
using SqlTools.Tests.Models;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace SqlTools.Tests
{
	[TestFixture]
    [Category("System")]
	public class WithoutSqlToolsTests
	{		
		[Test]
		public void ExecuteArray_ExampleSyntaxWithoutSqlTools_WithoutUsingStatements()
		{
			var connString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
			var sql = "select name from state";
			string[] stateNames = null;
			var cn = new SqlConnection(connString);
			try
			{
				var cmd = new SqlCommand(sql, cn);
				try
				{
					cn.Open();
					using (var reader = cmd.ExecuteReader())
					{
						var list = new List<string>();
						while (reader.Read())
						{
							list.Add(reader.GetString(0));
						}
						stateNames = list.ToArray();
					}
				}
				finally
				{
					if (cmd != null)
						cmd.Dispose();
				}
			}
			finally
			{
				if (cn != null)
					cn.Dispose();
			}
			Assert.IsTrue(stateNames.All(x => !String.IsNullOrEmpty(x)));
			Assert.AreEqual(71, stateNames.Length);
		}

		[Test]
		public void VerifyExecuteScalar()
		{
			var connString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
			var sql = "select count(*) from state";
			var numberOfStates = 0;
			var cn = new SqlConnection(connString);
			try
			{
				var cmd = new SqlCommand(sql, cn);
				try
				{
					cn.Open();
					var result = cmd.ExecuteScalar();
					if (result != System.DBNull.Value)
						numberOfStates = System.Convert.ToInt32(result);
				}
				finally
				{
					if (cmd != null)
						cmd.Dispose();
				}
			}
			finally
			{
				if (cn != null)
					cn.Dispose();
			}
			Assert.AreEqual(71, numberOfStates);
		}
		//[Test]
		//public void VerifyExecuteNonQuery()
		//{
		//	var numberRowsAffected = _helper.ExecuteNonQuery("update state set lastupdated=getdate()");
		//	Assert.AreEqual(71, numberRowsAffected);
		//}
		[Test]
		public void VerifyExecuteMultiple()
		{
			var connString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
			var sql = "select [Code], [Abbreviation], [Name], [Display], [LastUpdated] from state";
			IEnumerable<State> states = null;
			using (var cn = new SqlConnection(connString))
			using (var cmd = new SqlCommand(sql, cn))
			{
				cn.Open();
				using (var reader = cmd.ExecuteReader())
				{
					var items = new List<State>();
					while (reader.Read())
					{
						var item = new State();
						if (!reader.IsDBNull(0)) item.Code = reader.GetString(0);
						if (!reader.IsDBNull(1)) item.Abbreviation = reader.GetString(1);
						if (!reader.IsDBNull(2)) item.Name = reader.GetString(2);
						if (!reader.IsDBNull(3)) item.Display = reader.GetString(3);
						if (!reader.IsDBNull(4)) item.LastUpdated = reader.GetDateTime(4);
						items.Add(item);
					}
					states = items;
				}
			}
			Assert.AreEqual(71, states.Count());
		}
		//[Test]
		//public void VerifyExecuteMultipleIsCaseInsensitive()
		//{
		//	var states = _helper.ExecuteMultiple<State>("select CoDe, naME from state");
		//	Assert.IsTrue(states.All(x => !String.IsNullOrEmpty(x.Code) && !String.IsNullOrEmpty(x.Name)));
		//}
		//[Test]
		//public void VerifyExecuteSingle()
		//{
		//	var colorado = _helper.ExecuteSingle<State>("select * from state where code = '08'");
		//	Assert.AreEqual("Colorado", colorado.Name);
		//	Assert.AreEqual("Colorado", colorado.Display);
		//	Assert.AreEqual("CO", colorado.Abbreviation);
		//	Assert.AreEqual("08", colorado.Code);
		//}

		//[Test]
		//public void VerifyExecuteSingleReturnsTheFirstRow()
		//{
		//	// the sql statement below still executes and returns the results of: select * from state, but
		//	// only the first result is used.  Important to note, if you only want one row, make sure your
		//	// sql only returns one tuple.
		//	var state = _helper.ExecuteSingle<State>("select * from state");
		//	Assert.AreEqual("AL", state.Abbreviation);
		//}

		//[Test]
		//public void VerifyDbUtilityCanParameterizeQuery()
		//{
		//	var includeStateAbbreviations = new string[] { "CO", "TX" };
		//	var sql = "select * from state where abbreviation in (@abbreviation)";
		//	using (var cmd = new SqlCommand(sql))
		//	{
		//		// The Parameterize method rewrites the sql to look like this:
		//		//		select * from state where abbreviation in (@abbreviation0,@abbreviation1)
		//		// Parameterize takes care of setting the correct parameters per item in the list of abbreviations
		//		DbUtility.Parameterize(cmd, "@abbreviation", includeStateAbbreviations, 2);
		//		Assert.AreEqual(2, cmd.Parameters.Count, "Should have added two parameters");
		//		Assert.AreEqual("@abbreviation0", cmd.Parameters[0].ParameterName);
		//		Assert.AreEqual("@abbreviation1", cmd.Parameters[1].ParameterName);
		//		var states = _helper.ExecuteMultiple<State>(cmd);
		//		Assert.AreEqual(2, states.Count());
		//	}
		//}
		//[Test]
		//public void VerifyExecuteTupleHandlesCasting()
		//{
		//	var rows = _helper.ExecuteTuple<string, DateTime?>("select abbreviation, lastupdated from state");
		//	Assert.AreEqual(71, rows.Count());
		//	Assert.IsTrue(rows.All(x => !String.IsNullOrEmpty(x.Item1)));
		//}

		//[Test]
		//public void VerifyDefaultCommandTimeoutRespectsCustomTimeout()
		//{
		//	using (var cmd = new SqlCommand("select * from state"))
		//	{
		//		cmd.CommandTimeout = 666;
		//		_helper.ExecuteNonQuery(cmd);
		//		Assert.AreEqual(666, cmd.CommandTimeout);
		//	}
		//}
		//[Test]
		//public void VerifyDefaultCommandTimeoutOverridesDefaultSqlCommandCommandTimeout()
		//{
		//	using (var cmd = new SqlCommand("select * from state"))
		//	{
		//		Assert.AreEqual(30, cmd.CommandTimeout);
		//		// run it through the dbhelper(this should update the commandtimeout to 60)
		//		_helper.ExecuteNonQuery(cmd);
		//		Assert.AreEqual(60, cmd.CommandTimeout);
		//	}
		//}
		//[Test]
		//public void VerifyDataTable()
		//{
		//	var dataTable = _helper.ExecuteDataTable("select code, display from state");
		//	Assert.AreEqual(2, dataTable.Columns.Count);
		//	Assert.AreEqual("code", dataTable.Columns[0].ColumnName);
		//	Assert.AreEqual("display", dataTable.Columns[1].ColumnName);
		//	Assert.AreEqual(71, dataTable.Rows.Count);
		//}

		//[Test]
		//public void VerifyExecuteArraySkipsNullValuesWhenInstructed()
		//{
		//	var middleInitials = _helper.ExecuteArray<string>("select middleinitial from customer", ExecuteArrayOptions.IgnoreNullValues);
		//	Assert.AreEqual(1, middleInitials.Length);
		//}
		//[Test]
		//public void VerifyExecuteArrayIncludesNullEntriesByDefault()
		//{
		//	var middleInitials = _helper.ExecuteArray<string>("select middleinitial from customer");
		//	Assert.AreEqual(2, middleInitials.Length);
		//}
	}
}
