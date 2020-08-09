using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(2020_08_08_02)]
    public class AddSoftDeleteColumnToTracks : Migration
    {
        public override void Up()
        {
            Alter.Table("Tracks")
                 .AddColumn("SoftDelete")
                   .AsBoolean()
                   .NotNullable()
                   .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("SoftDelete").FromTable("Tracks");
        }
    }
}
