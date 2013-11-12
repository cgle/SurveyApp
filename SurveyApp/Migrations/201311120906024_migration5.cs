namespace SurveyApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        ResponseId = c.Int(nullable: false, identity: true),
                        UniqueId = c.String(),
                        QuestionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.ResponseId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        Text = c.String(),
                        options = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        SurveyId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Tips = c.String(),
                        CreatorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SurveyId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Questions", new[] { "SurveyId" });
            DropIndex("dbo.Responses", new[] { "UserId" });
            DropIndex("dbo.Responses", new[] { "QuestionId" });
            DropForeignKey("dbo.Questions", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.Responses", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Responses", "QuestionId", "dbo.Questions");
            DropTable("dbo.Surveys");
            DropTable("dbo.Questions");
            DropTable("dbo.Responses");
            DropTable("dbo.UserProfile");
        }
    }
}
