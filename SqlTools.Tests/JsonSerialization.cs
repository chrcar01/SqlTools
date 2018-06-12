using System;
using System.Data;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SqlTools.Tests
{
    [TestFixture]
    public class JsonSerialization
    {
        [Test]
        public void CanSerializeDynamicResult()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("FirstName", typeof(string));
            dataTable.Columns.Add("LastName", typeof(string));
            var row = dataTable.NewRow();
            row["FirstName"] = "Chris";
            row["LastName"] = "Carter";
            dataTable.Rows.Add(row);
            var columns = dataTable.Columns;
            dynamic result = new DynamicResult(columns, row);
            Assert.That(result.FirstName, Is.EqualTo("Chris"));
            Assert.That(result.LastName, Is.EqualTo("Carter"));
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }
    }
}
