using RepoDb;
using Rhyze.Core.Models;
using System;
using System.Data;
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
            var user = new User
            {
                Id = Guid.NewGuid(),
                IdentityId = IdentityId,
                Email = Email
            };

            var id = (Guid)await conn.MergeAsync(user, qualifiers: (u => new { u.IdentityId, u.Email }));

            return id;
        }

        public string IdentityId { get; }

        public string Email { get; }
    }
}