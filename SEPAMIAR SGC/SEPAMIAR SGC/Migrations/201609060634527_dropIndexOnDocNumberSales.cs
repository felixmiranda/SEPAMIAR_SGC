namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropIndexOnDocNumberSales : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.venta_usuarios", new[] { "numero_boleta" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.venta_usuarios", "numero_boleta", unique: true);
        }
    }
}
