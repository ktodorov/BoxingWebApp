namespace Boxing.Core.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCascadeDeletesForUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Login", "UserId", "dbo.User");
            DropForeignKey("dbo.Prediction", "UserId", "dbo.User");
            AddForeignKey("dbo.Login", "UserId", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Prediction", "UserId", "dbo.User", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prediction", "UserId", "dbo.User");
            DropForeignKey("dbo.Login", "UserId", "dbo.User");
            AddForeignKey("dbo.Prediction", "UserId", "dbo.User", "Id");
            AddForeignKey("dbo.Login", "UserId", "dbo.User", "Id");
        }
    }
}
