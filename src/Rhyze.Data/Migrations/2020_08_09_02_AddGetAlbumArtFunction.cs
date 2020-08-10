using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(2020_08_09_02)]
    public class AddGetAlbumArtFunction : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE FUNCTION fnGetAlbumArt
                (
	                @OwnerId uniqueidentifier,
	                @Album nvarchar(255),
	                @AlbumArtist nvarchar(255)
                )
                RETURNS nvarchar(500)
                AS
                BEGIN
	                DECLARE @url nvarchar(500)

	                SELECT 
		                TOP 1 @url=ImageUrl 
	                FROM Tracks 
	                WHERE Album = @Album AND 
	                      AlbumArtist = @AlbumArtist 
		                  AND OwnerId = @OwnerId 
	                ORDER BY DiscNo, TrackNo

	                RETURN @url
                END");
        }

        public override void Down()
        {
            Execute.Sql("DROP FUNCTION dbo.fnGetAlbumArt");
        }
    }
}
