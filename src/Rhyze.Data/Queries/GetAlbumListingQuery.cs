using RepoDb;
using Rhyze.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Queries
{
    public class GetAlbumListingQuery : IQueryAsync<IEnumerable<Album>>
    {
        public GetAlbumListingQuery(Guid ownerId, int skip, int take)
        {
            OwnerId = ownerId;
            Skip = skip;
            Take = take;
        }

        public async Task<IEnumerable<Album>> ExecuteAsync(IDbConnection conn)
        {
            return await conn.ExecuteQueryAsync<Album>("spGetAlbumListing", new
            {
                OwnerId,
                Skip,
                Take
            }, CommandType.StoredProcedure);
        }

        public Guid OwnerId { get; }

        public int Skip { get; }

        public int Take { get; }
    }
}