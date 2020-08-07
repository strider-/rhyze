using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Queries
{
    public interface IQueryAsync<T>
    {
        Task<T> ExecuteAsync(IDbConnection conn);
    }
}