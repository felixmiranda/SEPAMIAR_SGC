namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modsAACC_100916 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.clientes_asistencia",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        clienteId = c.Int(nullable: false),
                        fecha = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.clienteId)
                .Index(t => t.clienteId);
            
            CreateTable(
                "dbo.clientes_congelamientos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        clienteId = c.Int(nullable: false),
                        fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.clienteId)
                .Index(t => t.clienteId);
            
            AddColumn("dbo.clientes", "nutricionista_asignado", c => c.Int());
            CreateIndex("dbo.clientes", "nutricionista_asignado");
            AddForeignKey("dbo.clientes", "nutricionista_asignado", "dbo.nutricionistas", "usuario_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.clientes_congelamientos", "clienteId", "dbo.clientes");
            DropForeignKey("dbo.clientes_asistencia", "clienteId", "dbo.clientes");
            DropForeignKey("dbo.clientes", "nutricionista_asignado", "dbo.nutricionistas");
            DropIndex("dbo.clientes_congelamientos", new[] { "clienteId" });
            DropIndex("dbo.clientes_asistencia", new[] { "clienteId" });
            DropIndex("dbo.clientes", new[] { "nutricionista_asignado" });
            DropColumn("dbo.clientes", "nutricionista_asignado");
            DropTable("dbo.clientes_congelamientos");
            DropTable("dbo.clientes_asistencia");
        }
    }
}
