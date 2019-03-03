using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace SqlTools.Oracle
{
    public class OracleDbHelper : DbHelperBase
    {
        /// <summary>
        /// Initializes a new instance of the SqlDbHelper class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="defaultCommandTimeoutInSeconds">The default command timeout in seconds. The default for this value is whatever SqlCommand.CommandTimeout returns which is usually 30.</param>
        public OracleDbHelper(string connectionString) : base(connectionString, DbHelperBase.INITIAL_DEFAULT_COMMAND_TIMEOUT_IN_SECONDS)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SqlDbHelper class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="defaultCommandTimeoutInSeconds">The default command timeout in seconds. The default for this value is whatever SqlCommand.CommandTimeout returns which is usually 30.</param>
        public OracleDbHelper(string connectionString, int defaultCommandTimeoutInSeconds) : base(connectionString, defaultCommandTimeoutInSeconds)
        {
        }

        /// <summary>
        /// Creates a provider specific implementation of IDbCommand.
        /// </summary>
        /// <returns></returns>
        protected override IDbCommand CreateCommand()
        {
            return new OracleCommand();
        }

        /// <summary>
        /// Creates a provider specific implementation of IDbConnection.
        /// </summary>
        /// <returns></returns>
        protected override IDbConnection CreateConnection()
        {
            return new OracleConnection();
        }

        /// <summary>
        /// Creates a provider specific implementation of IDbDataAdapter.
        /// </summary>
        /// <param name="command">The command used by the IDbDataAdapter.</param>
        /// <returns></returns>
        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new OracleDataAdapter(command as OracleCommand);
        }
    }
}
