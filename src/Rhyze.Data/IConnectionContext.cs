using System.Data.Common;

namespace Rhyze.Data
{
    public interface IConnectionContext
    {
        DbConnection CreateDbConnection();
    }
}