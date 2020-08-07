using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Commands
{
    public interface ICommandAsync
    {
        Task ExecuteAsync(IDbConnection conn);
    }
}
