using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTools
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public abstract class BaseDbHelperTest
    {
        public abstract IDbCommand CreateCommand();
        public abstract IDataParameter CreateParameter(string name, object value);
        public abstract IDbHelper CreateHelper();
        protected virtual string SchemaName { get; } = "dbo";
        protected virtual string ParameterSymbol { get; } = "@";

        [Test]
        public void VerifyExecuteScalar()
        {
            var result = CreateHelper().ExecuteScalar<int>($"select count(*) from {SchemaName}.state");
            Assert.That(result, Is.GreaterThan(50));
        }

        [Test]
        public async Task VerifyExecuteScalarAsync()
        {
            var result = await CreateHelper().ExecuteScalarAsync<int>($"select count(*) from {SchemaName}.state");
            Assert.That(result, Is.GreaterThan(50));
        }

        [Test]
        public void VerifyExecuteArray()
        {
            var stateNames = CreateHelper().ExecuteArray<string>($"select Name from {SchemaName}.state");
            Assert.That(stateNames.Length, Is.AtLeast(70));
        }

        [Test]
        public async Task VerifyExecuteArrayAsync()
        {
            var stateNames = await CreateHelper().ExecuteArrayAsync<string>($"select Name from {SchemaName}.state");
            Assert.That(stateNames.Length, Is.AtLeast(70));
        }

        [Test]
        public void VerifyExecuteSingle()
        {
            var sql = $@"
SELECT Id, FirstName, MiddleInitial, LastName 
FROM {SchemaName}.Customer 
WHERE LastName={ParameterSymbol}LastName 
AND FirstName={ParameterSymbol}FirstName";
            using (var command = CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(CreateParameter($"{ParameterSymbol}LastName", "Carter"));
                command.Parameters.Add(CreateParameter($"{ParameterSymbol}FirstName", "Chris"));
                var customer = CreateHelper().ExecuteSingle<Customer>(command);
                Assert.That(customer, Is.Not.Null);
                Assert.That(customer.FirstName, Is.EqualTo("Chris"));
                Assert.That(customer.Id, Is.GreaterThan(0));
                Assert.That(customer.LastName, Is.EqualTo("Carter"));
                Assert.That(customer.MiddleInitial, Is.EqualTo("J"));
            }
        }

        [Test]
        public async Task VerifyExecuteSingleAsync()
        {
            var sql = $@"
SELECT Id, FirstName, MiddleInitial, LastName 
FROM {SchemaName}.Customer 
WHERE LastName={ParameterSymbol}LastName 
AND FirstName={ParameterSymbol}FirstName";
            using (var command = CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(CreateParameter($"{ParameterSymbol}LastName", "Carter"));
                command.Parameters.Add(CreateParameter($"{ParameterSymbol}FirstName", "Chris"));
                var customer = await CreateHelper().ExecuteSingleAsync<Customer>(command);
                Assert.That(customer, Is.Not.Null);
                Assert.That(customer.FirstName, Is.EqualTo("Chris"));
                Assert.That(customer.Id, Is.GreaterThan(0));
                Assert.That(customer.LastName, Is.EqualTo("Carter"));
                Assert.That(customer.MiddleInitial, Is.EqualTo("J"));
            }
        }

        [Test]
        public void VerifyExecuteMultiple()
        {
            var sql = $"SELECT Code, Abbreviation, Name, Display, LastUpdated FROM {SchemaName}.State";
            var states = CreateHelper().ExecuteMultiple<State>(sql);
            Assert.That(states, Is.Not.Null);
            Assert.That(states.Count(), Is.GreaterThan(70));
        }

        [Test]
        public async Task VerifyExecuteMultipleAsync()
        {
            var sql = $"SELECT Code, Abbreviation, Name, Display, LastUpdated FROM {SchemaName}.State";
            var states = await CreateHelper().ExecuteMultipleAsync<State>(sql);
            Assert.That(states, Is.Not.Null);
            Assert.That(states.Count(), Is.GreaterThan(70));
        }

        [Test]
        public void VerifyExecuteDictionary()
        {
            var sql = $"SELECT Code, Display FROM {SchemaName}.State";
            var optionsData = CreateHelper().ExecuteDictionary<string, string>(sql);
            Assert.That(optionsData, Is.Not.Null);
            Assert.That(optionsData.Count(), Is.AtLeast(70));
            Assert.That(optionsData.Any(kvp => String.IsNullOrWhiteSpace(kvp.Key) || String.IsNullOrWhiteSpace(kvp.Value)), Is.False);
        }
        [Test]
        public async Task VerifyExecuteDictionaryAsync()
        {
            var sql = $"SELECT Code, Display FROM {SchemaName}.State";
            var optionsData = await CreateHelper().ExecuteDictionaryAsync<string, string>(sql);
            Assert.That(optionsData, Is.Not.Null);
            Assert.That(optionsData.Count(), Is.AtLeast(70));
            Assert.That(optionsData.Any(kvp => String.IsNullOrWhiteSpace(kvp.Key) || String.IsNullOrWhiteSpace(kvp.Value)), Is.False);

        }
    }

    public class State
    {
        public string Code { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string Display { get; set; }
        public DateTime? LastUpdated { get; set; }        
    }
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
    }
}
