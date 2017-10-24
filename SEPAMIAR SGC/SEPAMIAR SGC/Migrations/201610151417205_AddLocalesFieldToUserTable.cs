namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocalesFieldToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.usuarios", "localesId", c => c.Int());
            CreateIndex("dbo.usuarios", "localesId");
            AddForeignKey("dbo.usuarios", "localesId", "dbo.locales", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.usuarios", "localesId", "dbo.locales");
            DropIndex("dbo.usuarios", new[] { "localesId" });
            DropColumn("dbo.usuarios", "localesId");
        }
    }
}
