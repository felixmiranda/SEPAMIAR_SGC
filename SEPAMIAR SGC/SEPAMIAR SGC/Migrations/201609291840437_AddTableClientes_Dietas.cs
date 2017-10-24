namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableClientes_Dietas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.clientes_dietas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        programa_clientes_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.programa_clientes", t => t.programa_clientes_id)
                .Index(t => t.programa_clientes_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.clientes_dietas", "programa_clientes_id", "dbo.programa_clientes");
            DropIndex("dbo.clientes_dietas", new[] { "programa_clientes_id" });
            DropTable("dbo.clientes_dietas");
        }
    }
}
