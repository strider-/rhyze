using RepoDb;
using Rhyze.Core.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Rhyze.Data.Commands
{
    public class UpdateAlbumMetadataCommand : ICommandAsync
    {
        private readonly Guid _ownerId;
        private readonly AlbumMetadata _metadata;

        public UpdateAlbumMetadataCommand(Guid ownerId, AlbumMetadata metadata)
        {
            _ownerId = ownerId;
            _metadata = metadata;
        }

        public async Task ExecuteAsync(IDbConnection conn)
        {
            await conn.ExecuteNonQueryAsync(@"
                UPDATE Tracks SET 
                    Album = @NewAlbumName,
                    AlbumArtist = @NewAlbumArtist,
                    Year = @Year,
                    TrackCount = @TrackCount,
                    DiscCount = @DiscCount
                WHERE Album = @Name AND AlbumArtist = @AlbumArtist AND OwnerId = @OwnerId", new
            {
                _metadata.Id.Name,
                _metadata.Id.AlbumArtist,
                OwnerId = _ownerId,
                NewAlbumName = _metadata.Album,
                NewAlbumArtist = _metadata.AlbumArtist,
                _metadata.Year,
                _metadata.TrackCount,
                _metadata.DiscCount
            });

            _metadata.Id = new AlbumId(_metadata.Album, _metadata.AlbumArtist);
        }
    }
}
