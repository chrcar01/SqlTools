using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTools.NetFramework.Tests
{
    [TestFixture]
    public class SqlDbHelperTests
    {
        private const string CONNECTION_STRING = "server=.;database=Scratch;trusted_connection=true;";

        [Test]
        public async Task CanTestAsync()
        {
            var dbHelper = new SqlDbHelper(() => new SqlConnection(CONNECTION_STRING));
            dbHelper.ConnectionStateChanged += (s, e) => Console.WriteLine(e.State);
            var states = await dbHelper.ExecuteMultipleAsync<State>("SELECT [Code], [Abbreviation], [Name] FROM [State]");
            foreach (var state in states)
            {
                Console.WriteLine(state);
            }

        }

        private class State
        {
            public string Code { get; set; }
            public string Abbreviation { get; set; }
            public string Name { get; set; }
            public override string ToString()
            {
                return $"{nameof(State)} {{ Code = {Code}, Abbreviation = {Abbreviation}, Name = {Name} }}";
            }
        }
    }
}
