using RepoDb;
using Rhyze.Core.Models;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyze.Data.Queries
{
    public class GetUserIdFromIdentityQuery : IQueryAsync<Guid>
    {
        public GetUserIdFromIdentityQuery(string identityId, string email)
        {
            IdentityId = identityId;
            Email = email;
        }

        public async Task<Guid> ExecuteAsync(IDbConnection conn)
        {
            Expression<Func<User, bool>> predicate = u => u.IdentityId == IdentityId && u.Email == Email;

            if (await conn.ExistsAsync(predicate))
            {
                return (await conn.QueryAsync(predicate)).Single().Id;
            }
            else
            {
                return (Guid)await conn.InsertAsync(new User
                {
                    IdentityId = IdentityId,
                    Email = Email,
                    Id = Guid.NewGuid()
                });
            }
        }

        public string IdentityId { get; }

        public string Email { get; }
    }
}