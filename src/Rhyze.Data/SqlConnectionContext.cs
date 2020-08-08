using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Rhyze.Data
{
    public class SqlConnectionContext : IConnectionContext
    {
        public SqlConnectionContext(string connectionString) => ConnectionString = connectionString;

        public DbConnection CreateDbConnection() => new SqlConnection(ConnectionString);

        private string ConnectionString { get; }
    }
}