using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(2020_08_08_01)]
    public class CreateGetAlbumListingStoredProcedure : Migration
    {
        public override void Up()
        {
			Execute.Sql(
			  @"CREATE PROCEDURE spGetAlbumListing
					@OwnerId uniqueidentifier,
					@Skip int,
					@Take int
				AS
				BEGIN
					WITH AlbumList AS (
						SELECT t.Album, 
							   t.AlbumArtist,
							   t.ImageUrl,
							   IIF(t.UploadedUtc > ISNULL(t.LastPlayedUtc, '1970-01-01'), t.UploadedUtc, t.LastPlayedUtc) AS TouchedUtc,
							   ROW_NUMBER() OVER(PARTITION BY t.Album 
												 ORDER BY IIF(t.UploadedUtc > ISNULL(t.LastPlayedUtc, '1970-01-01'), t.UploadedUtc, t.LastPlayedUtc) DESC) AS [Rank]
						FROM Tracks t WHERE t.OwnerId = @OwnerId)
					SELECT a.Album,
						   a.AlbumArtist,
						   a.ImageUrl,
						   a.TouchedUtc
					FROM AlbumList a
					WHERE a.[Rank] = 1
					ORDER BY a.TouchedUtc DESC
					OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
				END");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE spGetAlbumListing");
        }
    }
}
