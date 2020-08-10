using RepoDb;
using RepoDb.Enumerations;
using Rhyze.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Queries
{
    public class GetAlbumByIdQuery : IQueryAsync<IEnumerable<Track>>
    {
        public AlbumId AlbumId { get; }

        public Guid OwnerId { get; }

        public GetAlbumByIdQuery(Guid ownerId, AlbumId albumId)
        {
            OwnerId = ownerId;
            AlbumId = albumId;
        }

        public async Task<IEnumerable<Track>> ExecuteAsync(IDbConnection conn)
        {
            var order = OrderField.Parse(new
            {
                DiscNo = Order.Ascending,
                TrackNo = Order.Ascending
            });

            return await conn.QueryAsync<Track>(
                t => t.OwnerId == OwnerId &&
                t.Album == AlbumId.Name &&
                t.AlbumArtist == AlbumId.AlbumArtist,
            orderBy: order);
        }
    }
}