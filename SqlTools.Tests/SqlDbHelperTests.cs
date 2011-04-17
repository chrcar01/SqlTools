using NUnit.Framework;
using SqlTools.Tests.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;

namespace SqlTools.Tests
{
	[TestFixture]
	public class SqlDbHelperTests
	{
		private IDbHelper _helper;
		
		[TestFixtureSetUp]
		public void InitializeAllTests()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
			var defaultCommandTimeoutInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCommandTimeoutInSeconds"]);
			_helper = new SqlDbHelper(connectionString, defaultCommandTimeoutInSeconds);
		}
		[Test]
		public void VerifyChangeConnectionChangesInternalConnectionString()
		{
			var helper = new SqlDbHelper("godzilla");
			Assert.AreEqual("godzilla", helper.ConnectionString);
			helper.ChangeConnection("mothra");
			Assert.AreEqual("mothra", helper.ConnectionString);
		}
		[Test]
		[ExpectedException(typeof(ArgumentNullException), UserMessage = "connectionString is null or empty.")]
		public void ChangeConnectionThrowsArgNullExceptionIfConnectionStringMissing()
		{
			_helper.ChangeConnection(String.Empty);
		}
		[Test]
		public void DefaultCommandTimeoutInSecondsShouldDefaultToSqlCommandCommandTimeout()
		{
			var helper = new SqlDbHelper("");
			Assert.AreEqual(helper.DefaultCommandTimeoutInSeconds, new SqlCommand().CommandTimeout);
		}
		[Test]
		public void VerifyExecuteArray()
		{
			var stateNames = _helper.ExecuteArray<string>("select name from state");
			Assert.IsTrue(stateNames.All(x => !String.IsNullOrEmpty(x)));
			Assert.AreEqual(71, stateNames.Length);
			var stateCodes = _helper.ExecuteArray<string>("select code from state");
			Assert.IsTrue(stateCodes.All(x => x.Length == 2));
		}
		[Test]
		public void VerifyExecuteScalar()
		{
			var numberOfStates = _helper.ExecuteScalar<int>("select count(*) from state");
			Assert.AreEqual(71, numberOfStates);
		}
		[Test]
		public void VerifyExecuteNonQuery()
		{
			var numberRowsAffected = _helper.ExecuteNonQuery("update state set lastupdated=getdate()");
			Assert.AreEqual(71, numberRowsAffected);
		}
		[Test]
		public void VerifyExecuteMultiple()
		{
			var states = _helper.ExecuteMultiple<State>("select * from state");
			Assert.AreEqual(71, states.Count());
		}
		[Test]
		public void VerifyExecuteMultipleIsCaseInsensitive()
		{
			var states = _helper.ExecuteMultiple<State>("select CoDe, naME from state");
			Assert.IsTrue(states.All(x => !String.IsNullOrEmpty(x.Code) && !String.IsNullOrEmpty(x.Name)));
		}
		[Test]
		public void VerifyExecuteSingle()
		{
			var colorado = _helper.ExecuteSingle<State>("select * from state where code = '08'");
			Assert.AreEqual("Colorado", colorado.Name);
			Assert.AreEqual("Colorado", colorado.Display);
			Assert.AreEqual("CO", colorado.Abbreviation);
			Assert.AreEqual("08", colorado.Code);		
		}

		[Test]
		public void VerifyExecuteSingleReturnsTheFirstRow()
		{
			// the sql statement below still executes and returns the results of: select * from state, but
			// only the first result is used.  Important to note, if you only want one row, make sure your
			// sql only returns one tuple.
			var state = _helper.ExecuteSingle<State>("select * from state");
			Assert.AreEqual("AL", state.Abbreviation);
		}

		[Test]
		public void VerifyDbUtilityCanParameterizeQuery()
		{
			var includeStateAbbreviations = new string[] { "CO", "TX" };
			var sql = "select * from state where abbreviation in (@abbreviation)";
			using (var cmd = new SqlCommand(sql))
			{
				// The Parameterize method rewrites the sql to look like this:
				//		select * from state where abbreviation in (@abbreviation0,@abbreviation1)
				// Parameterize takes care of setting the correct parameters per item in the list of abbreviations
				DbUtility.Parameterize(cmd, includeStateAbbreviations, "@abbreviation");
				Assert.AreEqual(2, cmd.Parameters.Count, "Should have added two parameters");
				Assert.AreEqual("@abbreviation0", cmd.Parameters[0].ParameterName);
				Assert.AreEqual("@abbreviation1", cmd.Parameters[1].ParameterName);
				var states = _helper.ExecuteMultiple<State>(cmd);
				Assert.AreEqual(2, states.Count());
			}
		}
		[Test]
		public void VerifyExecuteTupleHandlesCasting()
		{
			var rows = _helper.ExecuteTuple<string, DateTime?>("select abbreviation, lastupdated from state");
			Assert.AreEqual(71, rows.Count());
			Assert.IsTrue(rows.All(x => !String.IsNullOrEmpty(x.Item1)));
		}

		[Test]
		public void VerifyDefaultCommandTimeoutRespectsCustomTimeout()
		{
			using (var cmd = new SqlCommand("select * from state"))
			{
				cmd.CommandTimeout = 666;
				_helper.ExecuteNonQuery(cmd);
				Assert.AreEqual(666, cmd.CommandTimeout);
			}
		}
		[Test]
		public void VerifyDefaultCommandTimeoutOverridesDefaultSqlCommandCommandTimeout()
		{
			using (var cmd = new SqlCommand("select * from state"))
			{
				Assert.AreEqual(30, cmd.CommandTimeout);
				// run it through the dbhelper(this should update the commandtimeout to 60)
				_helper.ExecuteNonQuery(cmd);
				Assert.AreEqual(60, cmd.CommandTimeout);
			}
		}
		[Test]
		public void VerifyDataTable()
		{
			var dataTable = _helper.ExecuteDataTable("select code, display from state");
			Assert.AreEqual(2, dataTable.Columns.Count);
			Assert.AreEqual("code", dataTable.Columns[0].ColumnName);
			Assert.AreEqual("display", dataTable.Columns[1].ColumnName);
			Assert.AreEqual(71, dataTable.Rows.Count);
		}

		[Test]
		public void VerifyExecuteArraySkipsNullValuesWhenInstructed()
		{
			var middleInitials = _helper.ExecuteArray<string>("select middleinitial from customer", ExecuteArrayOptions.IgnoreNullValues);
			Assert.AreEqual(1, middleInitials.Length);
		}
		[Test]
		public void VerifyExecuteArrayIncludesNullEntriesByDefault()
		{
			var middleInitials = _helper.ExecuteArray<string>("select middleinitial from customer");
			Assert.AreEqual(2, middleInitials.Length);
		}
		[Test]
		public void VerifyParameterizedQuery()
		{
			var excludeTheseStateAbbreviations = new string[] { "CO", "CA", "TX" };
			var sql = "select * from state where abbreviation not in (@abbreviation)";
			using (var cmd = new SqlCommand(sql))
			{
				DbUtility.Parameterize(cmd, excludeTheseStateAbbreviations, "@abbreviation");
				var data = _helper.ExecuteDataTable(cmd);
				Assert.AreEqual(68, data.Rows.Count);
			}
		}
		[Test]
		public void VerifyExtensionMethods()
		{
			var excludeTheseStateAbbreviations = new string[] { "CO", "CA", "TX" };
			var sql = "select * from state where abbreviation not in (@abbreviation)";
			using (var cmd = new SqlCommand(sql))
			{
				cmd.AddParameters("@abbreviation", excludeTheseStateAbbreviations);
				var data = _helper.ExecuteDataTable(cmd);
				Assert.AreEqual(68, data.Rows.Count);
			}
		}
	}
}
