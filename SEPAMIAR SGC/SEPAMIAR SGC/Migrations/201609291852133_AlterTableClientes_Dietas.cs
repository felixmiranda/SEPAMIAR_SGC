namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableClientes_Dietas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailyDiets",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        dia = c.Int(nullable: false),
                        tipo_comida = c.Int(nullable: false),
                        texto = c.String(),
                        clientes_dietas_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes_dietas", t => t.clientes_dietas_id)
                .Index(t => t.clientes_dietas_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DailyDiets", "clientes_dietas_id", "dbo.clientes_dietas");
            DropIndex("dbo.DailyDiets", new[] { "clientes_dietas_id" });
            DropTable("dbo.DailyDiets");
        }
    }
}
