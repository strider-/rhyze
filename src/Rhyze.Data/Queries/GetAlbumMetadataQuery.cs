using RepoDb;
using Rhyze.Core.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyze.Data.Queries
{
    public class GetAlbumMetadataQuery : IQueryAsync<AlbumMetadata>
    {
        public GetAlbumMetadataQuery(Guid ownerId, AlbumId albumId)
        {
            OwnerId = ownerId;
            AlbumId = albumId;
        }

        public async Task<AlbumMetadata> ExecuteAsync(IDbConnection conn)
        {
            var data = (await conn.ExecuteQueryAsync<AlbumMetadata>("spGetAlbumMetadata", new
            {
                OwnerId,
                Album = AlbumId.Name,
                AlbumId.AlbumArtist
            }, CommandType.StoredProcedure)).SingleOrDefault();

            if (data != null)
            {
                data.Id = AlbumId;
            }

            return data;
        }

        public Guid OwnerId { get; }

        public AlbumId AlbumId { get; }
    }
}