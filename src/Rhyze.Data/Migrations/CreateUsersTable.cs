using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(2020_08_06_01)]
    public class CreateUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                  .WithColumn("Id").AsGuid().NotNullable().PrimaryKey().WithDefaultValue(SystemMethods.NewGuid)
                  .WithColumn("IdentityId").AsString().NotNullable()
                  .WithColumn("Email").AsString().NotNullable();

            Create.Index("IX_IdentityId")
                .OnTable("Users")
                .OnColumn("IdentityId").Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_Email")
                .OnTable("Users")
                .OnColumn("Email").Ascending()
                .WithOptions().NonClustered();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}
