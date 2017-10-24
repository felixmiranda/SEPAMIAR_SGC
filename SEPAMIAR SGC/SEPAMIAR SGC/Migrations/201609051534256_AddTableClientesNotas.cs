namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableClientesNotas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.clientes_notas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idCliente = c.Int(nullable: false),
                        nota = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.idCliente)
                .Index(t => t.idCliente);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.clientes_notas", "idCliente", "dbo.clientes");
            DropIndex("dbo.clientes_notas", new[] { "idCliente" });
            DropTable("dbo.clientes_notas");
        }
    }
}
