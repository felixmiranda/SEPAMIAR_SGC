namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.semanas_precios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        cantidad_semanas = c.Int(nullable: false),
                        dias_congelamiento = c.Int(nullable: false),
                        precio = c.Double(nullable: false),
                        localesId = c.Int(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.locales", t => t.localesId)
                .Index(t => t.localesId);

			DropColumn("dbo.venta_usuarios", "cantidad_semanas");
			AddColumn("dbo.venta_usuarios", "semanas_precio_id", c => c.Int(nullable: false));
            AlterColumn("dbo.venta_usuarios", "monto", c => c.Double(nullable: false));
            CreateIndex("dbo.venta_usuarios", "semanas_precio_id");
            //AddForeignKey("dbo.venta_usuarios", "semanas_precio_id", "dbo.semanas_precios", "id"); crear manualmente
        }
        
        public override void Down()
        {
            AddColumn("dbo.venta_usuarios", "cantidad_semanas", c => c.Int(nullable: false));
            //DropForeignKey("dbo.venta_usuarios", "semanas_precio_id", "dbo.semanas_precios");
            DropForeignKey("dbo.semanas_precios", "localesId", "dbo.locales");
            DropIndex("dbo.venta_usuarios", new[] { "semanas_precio_id" });
            DropIndex("dbo.semanas_precios", new[] { "localesId" });
            AlterColumn("dbo.venta_usuarios", "monto", c => c.Int(nullable: false));
            DropColumn("dbo.venta_usuarios", "semanas_precio_id");
            DropTable("dbo.semanas_precios");
        }
    }
}
