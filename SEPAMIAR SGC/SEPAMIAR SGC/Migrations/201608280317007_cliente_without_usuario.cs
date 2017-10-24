namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cliente_without_usuario : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.clientes", "usuario_id", "dbo.usuarios");
            DropForeignKey("dbo.citas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes");
            DropIndex("dbo.clientes", new[] { "usuario_id" });
            RenameColumn(table: "dbo.clientes", name: "usuario_id", newName: "usuarios_id");
            DropPrimaryKey("dbo.clientes");
            AddColumn("dbo.clientes", "id", c => c.Int(nullable: false));
            AddColumn("dbo.clientes", "codigo", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.clientes", "nombres", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.clientes", "apellidos", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.clientes", "email", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.clientes", "password", c => c.Int(nullable: false));
            AddColumn("dbo.clientes", "activo", c => c.Boolean(nullable: false));
            AlterColumn("dbo.clientes", "usuarios_id", c => c.Int());
            AddPrimaryKey("dbo.clientes", "id");
            CreateIndex("dbo.clientes", "usuarios_id");
            AddForeignKey("dbo.clientes", "usuarios_id", "dbo.usuarios", "id");
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
            DropForeignKey("dbo.clientes", "usuarios_id", "dbo.usuarios");
            DropIndex("dbo.clientes", new[] { "usuarios_id" });
            DropPrimaryKey("dbo.clientes");
            AlterColumn("dbo.clientes", "usuarios_id", c => c.Int(nullable: false));
            DropColumn("dbo.clientes", "activo");
            DropColumn("dbo.clientes", "password");
            DropColumn("dbo.clientes", "email");
            DropColumn("dbo.clientes", "apellidos");
            DropColumn("dbo.clientes", "nombres");
            DropColumn("dbo.clientes", "codigo");
            DropColumn("dbo.clientes", "id");
            AddPrimaryKey("dbo.clientes", "usuario_id");
            RenameColumn(table: "dbo.clientes", name: "usuarios_id", newName: "usuario_id");
            CreateIndex("dbo.clientes", "usuario_id");
            AddForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes", "usuario_id");
            AddForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes", "usuario_id");
            AddForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes", "usuario_id");
            AddForeignKey("dbo.citas", "cliente_id", "dbo.clientes", "usuario_id");
            AddForeignKey("dbo.clientes", "usuario_id", "dbo.usuarios", "id", cascadeDelete: true);
        }
    }
}
