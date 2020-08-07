using Rhyze.Data.Commands;
using Rhyze.Data.Queries;
using System.Threading.Tasks;

namespace Rhyze.Data
{
    public interface IDatabase
    {
        Task<T> ExecuteAsync<T>(IQueryAsync<T> query);

        Task ExecuteAsync(ICommandAsync command);
    }
}
