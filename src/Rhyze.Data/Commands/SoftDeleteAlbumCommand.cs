using RepoDb;
using Rhyze.Core.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Commands
{
    public class SoftDeleteAlbumCommand : ICommandAsync
    {
        public SoftDeleteAlbumCommand(Guid ownerId, AlbumId albumId)
        {
            AlbumId = albumId;
            OwnerId = ownerId;
        }

        public async Task ExecuteAsync(IDbConnection conn)
        {
            await conn.ExecuteNonQueryAsync(@"
                UPDATE Tracks SET SoftDelete = 1 
                WHERE Album = @Name AND AlbumArtist = @AlbumArtist AND OwnerId = @OwnerId", new
            {
                AlbumId.Name,
                AlbumId.AlbumArtist,
                OwnerId
            });
        }

        public AlbumId AlbumId { get; }

        public Guid OwnerId { get; }
    }
}