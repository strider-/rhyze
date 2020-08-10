using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(2020_08_09_04)]
    public class AlterGetAlbumListingToUseGetAlbumArt : ForwardOnlyMigration
    {
        public override void Up()
        {
			Execute.Sql(
			  @"ALTER PROCEDURE spGetAlbumListing
					@OwnerId uniqueidentifier,
					@Skip int,
					@Take int
				AS
				BEGIN
					WITH AlbumList AS (
						SELECT t.Album, 
							   t.AlbumArtist,
							   IIF(t.UploadedUtc > ISNULL(t.LastPlayedUtc, '1970-01-01'), t.UploadedUtc, t.LastPlayedUtc) AS TouchedUtc,
							   ROW_NUMBER() OVER(PARTITION BY t.Album, t.AlbumArtist 
												 ORDER BY IIF(t.UploadedUtc > ISNULL(t.LastPlayedUtc, '1970-01-01'), t.UploadedUtc, t.LastPlayedUtc) DESC) AS [Rank]
						FROM Tracks t 
						WHERE t.OwnerId = @OwnerId AND 
                              t.SoftDelete = 0)
					SELECT a.Album,
						   a.AlbumArtist,
						   dbo.fnGetAlbumArt(@OwnerId, a.Album, a.AlbumArtist) AS ImageUrl,
						   a.TouchedUtc
					FROM AlbumList a
					WHERE a.[Rank] = 1
					ORDER BY a.TouchedUtc DESC
					OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
				END");
		}
    }
}
