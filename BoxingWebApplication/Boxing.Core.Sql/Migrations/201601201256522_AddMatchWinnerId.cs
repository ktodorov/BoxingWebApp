namespace Boxing.Core.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMatchWinnerId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Match", "WinnerId", c => c.Int());
            CreateIndex("dbo.Match", "WinnerId");
            AddForeignKey("dbo.Match", "WinnerId", "dbo.Boxer", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Match", "WinnerId", "dbo.Boxer");
            DropIndex("dbo.Match", new[] { "WinnerId" });
            DropColumn("dbo.Match", "WinnerId");
        }
    }
}
