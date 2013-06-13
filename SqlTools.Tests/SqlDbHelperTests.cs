using NUnit.Framework;
using SqlTools.Tests.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace SqlTools.Tests
{
	[TestFixture]
	public class SqlDbHelperTests
	{
		
		[Test]
		public void VerifyConnectionStateChanged()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
			var helper = new SqlDbHelper(connectionString);
			helper.ConnectionStateChanged += (x, y) => Console.WriteLine("State changed: {0}", y.State);
			helper.ConnectionCreated += (x, y) => Console.WriteLine("Connection created!");
			using (var cmd = new SqlCommand("select Code, Display from state"))
			using (var reader = helper.ExecuteReader(cmd))
			{
				while (reader.Read())
				{
					
				}
			}
			
		}
		private IDbHelper _helper;

		[Test]
		public void VerifyDataColumnCollectionContains()
		{
			var table = new DataTable();
			table.Columns.Add("Unpaid");
			Assert.IsTrue(table.Columns.Contains("unPAID"));

		}
		[TestFixtureSetUp]
		public void InitializeAllTests()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["sqltools"].ConnectionString;
			var defaultCommandTimeoutInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultCommandTimeoutInSeconds"]);
			_helper = new SqlDbHelper(connectionString, defaultCommandTimeoutInSeconds);
		}

		[Test]
		[ExpectedException(typeof(DynamicResultException), UserMessage="The property 'PropertyDoesNotExist' was not found.")]
		public void VerifyDynamicResultExceptionThrownWhenPropertyNotFound()
		{
			var sql = "select firstname from customer where id = 1";
			dynamic customer = _helper.ExecuteDynamic(sql);
			Assert.AreEqual("this doesn't matter", customer.PropertyDoesNotExist);
		}
		[Test]
		public void VerifySingleDynamicObject()
		{
			var sql = "select firstname, lastname from customer where id = 1";
			dynamic customer = _helper.ExecuteDynamic(sql);
			customer.IsWorthy = true;
			Assert.AreEqual("Chris", customer.fIrStNAME);
			Assert.IsTrue(customer.IsWorthy);
		}
		[Test]
		public void VerifyMultipleDynamicObjects()
		{
			var sql = "select firstname, lastname from customer";
			IEnumerable<dynamic> customers = _helper.ExecuteDynamics(sql);
			foreach (dynamic customer in customers)
			{
				Console.WriteLine("{0}, {1}", customer.LastName, customer.FirstName);
			}
		}
		[Test]
		public void VerifyUserDefinedCommandTimeoutOverridesDefaultCommandTimeout()
		{
			var helper = new TestHelper("");
			helper.DefaultCommandTimeoutInSeconds = 600;
			using (var cmd = new SqlCommand(""))
			{
				helper.Prep(cmd);
				Assert.AreEqual(600, cmd.CommandTimeout);
				cmd.CommandTimeout = 2880;
				helper.Prep(cmd);
				Assert.AreEqual(2880, cmd.CommandTimeout);
			}
			
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
			var localHelper = new SqlDbHelper(_helper.ConnectionString);
			localHelper.ConnectionCreated += (x, y) => Console.WriteLine("Connection Created");
			//var numberOfStates = localHelper.ExecuteScalar<int>("select count(*) from state");
			//Assert.AreEqual(71, numberOfStates);
			using (var cmd = new SqlCommand("select count(*) from state"))
			{
				localHelper.ExecuteScalar<int>(cmd);
			}
			
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

		[Test]
		public void VerifyExecuteDictionary()
		{
			var states = _helper.ExecuteDictionary<string, string>("select code, name from state");
			foreach (var kvp in states)
			{
				Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
			}
		}
	
	}
}
