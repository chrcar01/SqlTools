using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTools.Oracle.Tests
{
    [TestFixture]
    public class HelperTests
    {
        private IDbHelper CreateHelper()
        {
            var connectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=host.docker.internal)(PORT=32769)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCLPDB1.localdomain)));User Id=sqltools;Password=boomsauce";
            return new OracleDbHelper(connectionString);
        }

        [Test]
        public async Task VerifyExecuteScalarAsync()
        {
            var result = await CreateHelper().ExecuteScalarAsync<int>("select count(*) from SCOTT.State");
            Assert.That(result, Is.GreaterThan(50));
        }
    }
}
