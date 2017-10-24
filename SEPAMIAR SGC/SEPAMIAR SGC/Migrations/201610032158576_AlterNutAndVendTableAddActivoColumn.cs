namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNutAndVendTableAddActivoColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.nutricionistas", "activo", c => c.Boolean(nullable: false));
            AddColumn("dbo.vendedores", "activo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.vendedores", "activo");
            DropColumn("dbo.nutricionistas", "activo");
        }
    }
}
