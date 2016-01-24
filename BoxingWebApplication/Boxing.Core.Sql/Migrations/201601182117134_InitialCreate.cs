namespace Boxing.Core.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boxer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Login",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        AuthToken = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false),
                        FullName = c.String(maxLength: 250),
                        IsAdmin = c.Boolean(nullable: false),
                        Rating = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Match",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Boxer1Id = c.Int(nullable: false),
                        Boxer2Id = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 2000),
                        Time = c.DateTime(nullable: false),
                        Description = c.String(nullable: false, maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boxer", t => t.Boxer1Id)
                .ForeignKey("dbo.Boxer", t => t.Boxer2Id)
                .Index(t => t.Boxer1Id)
                .Index(t => t.Boxer2Id);
            
            CreateTable(
                "dbo.Prediction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MatchId = c.Int(nullable: false),
                        PredictedBoxerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Match", t => t.MatchId)
                .ForeignKey("dbo.Boxer", t => t.PredictedBoxerId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MatchId)
                .Index(t => t.PredictedBoxerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prediction", "UserId", "dbo.User");
            DropForeignKey("dbo.Prediction", "PredictedBoxerId", "dbo.Boxer");
            DropForeignKey("dbo.Prediction", "MatchId", "dbo.Match");
            DropForeignKey("dbo.Match", "Boxer2Id", "dbo.Boxer");
            DropForeignKey("dbo.Match", "Boxer1Id", "dbo.Boxer");
            DropForeignKey("dbo.Login", "UserId", "dbo.User");
            DropIndex("dbo.Prediction", new[] { "PredictedBoxerId" });
            DropIndex("dbo.Prediction", new[] { "MatchId" });
            DropIndex("dbo.Prediction", new[] { "UserId" });
            DropIndex("dbo.Match", new[] { "Boxer2Id" });
            DropIndex("dbo.Match", new[] { "Boxer1Id" });
            DropIndex("dbo.Login", new[] { "UserId" });
            DropTable("dbo.Prediction");
            DropTable("dbo.Match");
            DropTable("dbo.User");
            DropTable("dbo.Login");
            DropTable("dbo.Boxer");
        }
    }
}
