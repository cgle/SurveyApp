namespace SurveyApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responses", "UniqueId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Responses", "UniqueId");
        }
    }
}
