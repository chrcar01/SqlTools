using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlTools.SqlServer.Tests
{
    [TestFixture]
    public class HelperTests
    {
        private IDbHelper CreateHelper()
        {
            return new SqlDbHelper("server=.;database=scratch;trusted_connection=true;");
        }

        [Test]
        public async Task VerifyExecuteScalarAsync()
        {
            var result = await CreateHelper().ExecuteScalarAsync<int>("select count(*) from state");
            Assert.That(result, Is.GreaterThan(50));
        }
    }
}
