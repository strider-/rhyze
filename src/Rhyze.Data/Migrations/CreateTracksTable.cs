using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(2020_08_07_01)]
    public class CreateTracksTable : Migration
    {
        public override void Up()
        {
            Create.Table("Tracks")
                  .WithColumn("Id").AsGuid().PrimaryKey().NotNullable().WithDefaultValue(SystemMethods.NewSequentialId)
                  .WithColumn("Title").AsString(255).NotNullable().Indexed()
                  .WithColumn("Album").AsString(255).Indexed()
                  .WithColumn("Artist").AsString(255).Indexed()
                  .WithColumn("AlbumArtist").AsString(500)
                  .WithColumn("TrackNo").AsInt32().NotNullable()
                  .WithColumn("TrackCount").AsInt32().NotNullable()
                  .WithColumn("DiscNo").AsInt32().NotNullable()
                  .WithColumn("DiscCount").AsInt32().NotNullable()
                  .WithColumn("Year").AsInt32()
                  .WithColumn("Duration").AsDouble().NotNullable()
                  .WithColumn("ImageUrl").AsString(500)
                  .WithColumn("AudioUrl").AsString(500).NotNullable()
                  .WithColumn("UploadedUtc").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
                  .WithColumn("LastPlayedUtc").AsDateTime().Nullable()
                  .WithColumn("PlayCount").AsInt32().NotNullable()
                  .WithColumn("OwnerId").AsGuid().NotNullable().Indexed();

            Create.ForeignKey("FK_Tracks_To_User")
                  .FromTable("Tracks").ForeignColumn("OwnerId")
                  .ToTable("Users").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Tracks");
        }
    }
}