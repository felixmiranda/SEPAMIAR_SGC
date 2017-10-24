namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientes_id_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.citas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes");
            DropPrimaryKey("dbo.clientes");
            AddPrimaryKey("dbo.clientes", "id_alt");
            AddForeignKey("dbo.citas", "cliente_id", "dbo.clientes", "id_alt");
            AddForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes", "id_alt");
            AddForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes", "id_alt");
            AddForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes", "id_alt");
            DropColumn("dbo.clientes", "id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.clientes", "id", c => c.Int(nullable: false));
            DropForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.citas", "cliente_id", "dbo.clientes");
            DropPrimaryKey("dbo.clientes");
            AddPrimaryKey("dbo.clientes", "id");
            AddForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.citas", "cliente_id", "dbo.clientes", "id");
        }
    }
}
