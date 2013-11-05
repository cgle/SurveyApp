namespace SurveyApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migraton2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "options", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "options");
        }
    }
}
