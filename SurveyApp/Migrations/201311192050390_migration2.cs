






























































namespace SurveyApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "QuestionType", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "Instructions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Instructions");
            DropColumn("dbo.Questions", "QuestionType");
        }
    }
}
