using Rhyze.Data.Commands;
using Rhyze.Data.Queries;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Rhyze.Data
{
    public class Database : IDatabase
    {
        private readonly string _connString;

        public Database(string connectionString) => _connString = connectionString;

        public Task<T> ExecuteAsync<T>(IQueryAsync<T> query)
        {
            using (var conn = NewConnection())
            {
                return query.ExecuteAsync(conn);
            }
        }

        public Task ExecuteAsync(ICommandAsync command)
        {
            using (var conn = NewConnection())
            {
                return command.ExecuteAsync(conn);
            }
        }

        protected virtual DbConnection NewConnection() => new SqlConnection(_connString);
    }
}