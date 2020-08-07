using RepoDb;
using Rhyze.Core.Models;

namespace Rhyze.Data
{
    public static class ModelMapping
    {
        public static void Initialize()
        {
            FluentMapper.Entity<User>()
                .Table("[dbo].[Users]")
                .Primary(u => u.Id)
                .Column(u => u.Email, "[Email]")
                .Column(u => u.IdentityId, "[IdentityId]")
                .Column(u => u.Id, "[Id]");
        }
    }
}
