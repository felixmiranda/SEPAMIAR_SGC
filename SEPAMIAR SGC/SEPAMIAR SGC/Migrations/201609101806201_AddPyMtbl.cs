namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPyMtbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.pesos_medidas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        clienteId = c.Int(nullable: false),
                        peso = c.Double(nullable: false),
                        porc_grasa_corporal = c.Double(nullable: false),
                        medidas_cuello = c.Double(nullable: false),
                        medidas_hombros = c.Double(nullable: false),
                        medidas_torax = c.Double(nullable: false),
                        medidas_biceps = c.Double(nullable: false),
                        medidas_muneca = c.Double(nullable: false),
                        medidas_cintura = c.Double(nullable: false),
                        medidas_gluteos = c.Double(nullable: false),
                        medidas_muslo = c.Double(nullable: false),
                        medidas_pantorrilla = c.Double(nullable: false),
                        created_at = c.DateTimeOffset(nullable: false, precision: 7),
                        created_by = c.Int(nullable: false),
                        updated_at = c.DateTimeOffset(nullable: false, precision: 7),
                        updated_by = c.Int(nullable: false),
                        deleted_at = c.DateTimeOffset(nullable: false, precision: 7),
                        deleted_by = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.clienteId)
                .Index(t => t.clienteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.pesos_medidas", "clienteId", "dbo.clientes");
            DropIndex("dbo.pesos_medidas", new[] { "clienteId" });
            DropTable("dbo.pesos_medidas");
        }
    }
}
