namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_clientes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.clientes", "usuarios_id", "dbo.usuarios");
            DropForeignKey("dbo.citas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes");
            DropIndex("dbo.clientes", new[] { "usuarios_id" });
            DropPrimaryKey("dbo.clientes");
            AlterColumn("dbo.clientes", "id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.clientes", "id");
            AddForeignKey("dbo.citas", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes", "id");
            DropColumn("dbo.clientes", "usuarios_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.clientes", "usuarios_id", c => c.Int());
            DropForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.citas", "cliente_id", "dbo.clientes");
            DropPrimaryKey("dbo.clientes");
            AlterColumn("dbo.clientes", "id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.clientes", "id");
            CreateIndex("dbo.clientes", "usuarios_id");
            AddForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.Citas", "cliente_id", "dbo.clientes", "id");
            AddForeignKey("dbo.clientes", "usuarios_id", "dbo.usuarios", "id");
        }
    }
}
