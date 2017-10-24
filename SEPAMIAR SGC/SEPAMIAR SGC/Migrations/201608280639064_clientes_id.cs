namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientes_id : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.citas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes");
            DropPrimaryKey("dbo.clientes");
            AddColumn("dbo.clientes", "id_alt", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.clientes", "id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.clientes", "id");
            AddForeignKey("dbo.citas", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.citas", "cliente_id", "dbo.clientes");
            DropPrimaryKey("dbo.clientes");
            AlterColumn("dbo.clientes", "id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.clientes", "id_alt");
            AddPrimaryKey("dbo.clientes", "id");
            AddForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.citas", "cliente_id", "dbo.clientes", "id");
        }
    }
}
