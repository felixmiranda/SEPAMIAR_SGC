namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFMTableAddUserColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.fichas_medicas", "created_by", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "updated_by", c => c.Int(nullable: false));
            CreateIndex("dbo.fichas_medicas", "created_by");
            CreateIndex("dbo.fichas_medicas", "updated_by");
            AddForeignKey("dbo.fichas_medicas", "updated_by", "dbo.usuarios", "id");
            AddForeignKey("dbo.fichas_medicas", "created_by", "dbo.usuarios", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.fichas_medicas", "created_by", "dbo.usuarios");
            DropForeignKey("dbo.fichas_medicas", "updated_by", "dbo.usuarios");
            DropIndex("dbo.fichas_medicas", new[] { "updated_by" });
            DropIndex("dbo.fichas_medicas", new[] { "created_by" });
            DropColumn("dbo.fichas_medicas", "updated_by");
            DropColumn("dbo.fichas_medicas", "created_by");
        }
    }
}
