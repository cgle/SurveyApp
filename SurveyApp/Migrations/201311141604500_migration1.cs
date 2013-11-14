namespace SurveyApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "Tips", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Surveys", "Tips");
        }
    }
}
