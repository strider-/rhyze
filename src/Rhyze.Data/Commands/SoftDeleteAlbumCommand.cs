using RepoDb;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Commands
{
    public class SoftDeleteAlbumCommand : ICommandAsync
    {
        public SoftDeleteAlbumCommand(Guid ownerId, string name)
        {
            AlbumName = name;
            OwnerId = ownerId;
        }

        public async Task ExecuteAsync(IDbConnection conn)
        {
            await conn.ExecuteNonQueryAsync("UPDATE Tracks SET SoftDelete = 1 WHERE Album = @Album AND OwnerId = @OwnerId", new
            {
                Album = AlbumName,
                OwnerId
            });
        }

        public string AlbumName { get; }

        public Guid OwnerId { get; }
    }
}