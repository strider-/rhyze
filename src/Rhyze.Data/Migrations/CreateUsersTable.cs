using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(2020_08_06_01)]
    public class CreateUsersTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"CREATE TABLE [dbo].[Users] (
	                        [Id]	      UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
	                        [IdentityId]  NVARCHAR(255)    NOT NULL,
	                        [Email]       NVARCHAR(255)    NOT NULL,
	                        CONSTRAINT [PK_Users] PRIMARY KEY NONCLUSTERED ([Id] ASC)
                        );");

            Create.Index("IX_IdentityId_Email")
                 .OnTable("Users")
                 .OnColumn("IdentityId").Ascending()
                 .OnColumn("Email").Ascending()
                 .WithOptions().Clustered();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}