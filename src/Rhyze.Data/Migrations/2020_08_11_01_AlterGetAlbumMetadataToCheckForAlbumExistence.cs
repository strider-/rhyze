using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(2020_08_11_01)]
    public class AlterGetAlbumMetadataToCheckForAlbumExistence : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
				ALTER PROCEDURE [dbo].[spGetAlbumMetadata]
					@OwnerId uniqueidentifier,
					@Album nvarchar(255),
					@AlbumArtist nvarchar(255)
				AS
				BEGIN
					IF(EXISTS(SELECT OwnerId FROM Tracks WHERE OwnerId = @OwnerId
															   AND Album= @Album
															   AND AlbumArtist=@AlbumArtist))
					BEGIN
						SELECT 
							@Album as Album,
							@AlbumArtist as AlbumArtist,
							CASE WHEN COUNT(distinct Year) = 1 THEN MIN(Year) ELSE NULL END AS Year,
							CASE WHEN COUNT(distinct TrackCount) = 1 THEN MIN(TrackCount) ELSE NULL END AS TrackCount,
							CASE WHEN COUNT(distinct DiscCount) = 1 THEN MIN(DiscCount) ELSE NULL END AS DiscCount,
							dbo.fnGetAlbumArt(@OwnerId, @Album, @AlbumArtist) as ImageUrl
						FROM Tracks
						WHERE OwnerId = @OwnerId
								AND Album= @Album
								AND AlbumArtist=@AlbumArtist
					END
				END
            ");
        }
    }
}
