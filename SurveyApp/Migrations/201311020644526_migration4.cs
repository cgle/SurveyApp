namespace SurveyApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "UserId", "dbo.UserProfile");
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.Answers", new[] { "UserId" });
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        ResponseId = c.Int(nullable: false, identity: true),
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
            
            DropTable("dbo.Answers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId);
            
            DropIndex("dbo.Responses", new[] { "UserId" });
            DropIndex("dbo.Responses", new[] { "QuestionId" });
            DropForeignKey("dbo.Responses", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Responses", "QuestionId", "dbo.Questions");
            DropTable("dbo.Responses");
            CreateIndex("dbo.Answers", "UserId");
            CreateIndex("dbo.Answers", "QuestionId");
            AddForeignKey("dbo.Answers", "UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "QuestionId", cascadeDelete: true);
        }
    }
}
