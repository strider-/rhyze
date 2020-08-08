using RepoDb;
using Rhyze.Data.Commands;
using Rhyze.Data.Queries;
using System.Threading.Tasks;

namespace Rhyze.Data
{
    public class Database : IDatabase
    {
        private readonly IConnectionContext _ctx;

        public Database(IConnectionContext context) => _ctx = context;

        static Database()
        {
            SqlServerBootstrap.Initialize();
            ModelMapping.Initialize();
        }

        public async Task<T> ExecuteAsync<T>(IQueryAsync<T> query)
        {
            using (var conn = await _ctx.CreateDbConnection().EnsureOpenAsync())
            {
                return await query.ExecuteAsync(conn);
            }
        }

        public async Task ExecuteAsync(ICommandAsync command)
        {
            using (var conn = await _ctx.CreateDbConnection().EnsureOpenAsync())
            {
                await command.ExecuteAsync(conn);
            }
        }
    }
}