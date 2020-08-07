using FluentMigrator;

namespace Rhyze.Data.Migrations
{
    [Migration(0)]
    public class CreateDatabase : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql("CREATE DATABASE [Rhyze]");
        }
    }
}
