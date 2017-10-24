namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estructura_tablas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.campana_marketing",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        activo = c.Boolean(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.horarios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        sala_id = c.Int(nullable: false),
                        programa_id = c.Int(nullable: false),
                        dia_hora = c.DateTime(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.programas", t => t.programa_id, cascadeDelete: true)
                .ForeignKey("dbo.salas", t => t.sala_id, cascadeDelete: true)
                .Index(t => t.sala_id)
                .Index(t => t.programa_id);
            
            CreateTable(
                "dbo.salas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        local_id = c.Int(nullable: false),
                        nombre = c.String(),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.locales", t => t.local_id, cascadeDelete: true)
                .Index(t => t.local_id);
            
            CreateTable(
                "dbo.locales",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        distrito = c.String(),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.programa_usuarios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        programa_id = c.Int(nullable: false),
                        cliente_id = c.Int(nullable: false),
                        horario_id = c.Int(nullable: false),
                        fecha_inicio = c.DateTime(nullable: false),
                        fecha_fin = c.DateTime(nullable: false),
                        tipo_ingreso = c.Int(nullable: false),
                        campana_marketing_id = c.Int(),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.cliente_id, cascadeDelete: true)
                .ForeignKey("dbo.programas", t => t.programa_id, cascadeDelete: true)
                .Index(t => t.programa_id)
                .Index(t => t.cliente_id);
            
            CreateTable(
                "dbo.prospectos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nombres = c.String(nullable: false),
                        apellidos = c.String(nullable: false),
                        genero = c.Int(nullable: false),
                        fecha_nacimiento = c.DateTime(nullable: false, storeType: "date"),
                        documento_tipo = c.Int(nullable: false),
                        documento_numero = c.String(nullable: false, maxLength: 10),
                        email_personal = c.String(nullable: false),
                        centrol_laboral = c.String(),
                        email_empresa = c.String(),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.solicitud_permisos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        usuario_jefe = c.Int(nullable: false),
                        usuario_solicitante = c.Int(nullable: false),
                        model_id = c.Int(nullable: false),
                        permiso_id = c.Int(nullable: false),
                        respuesta = c.Boolean(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.usuarios", t => t.usuario_jefe, cascadeDelete: false)
                .ForeignKey("dbo.usuarios", t => t.usuario_solicitante, cascadeDelete: false)
                .Index(t => t.usuario_jefe)
                .Index(t => t.usuario_solicitante);
            
            CreateTable(
                "dbo.venta_usuarios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        local_id = c.Int(nullable: false),
                        cliente_id = c.Int(nullable: false),
                        vendedor_id = c.Int(nullable: false),
                        boleta = c.String(),
                        monto = c.Int(nullable: false),
                        atendido = c.Boolean(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.cliente_id, cascadeDelete: true)
                .ForeignKey("dbo.locales", t => t.local_id, cascadeDelete: true)
                .Index(t => t.local_id)
                .Index(t => t.cliente_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.venta_usuarios", "local_id", "dbo.locales");
            DropForeignKey("dbo.venta_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.solicitud_permisos", "usuario_solicitante", "dbo.usuarios");
            DropForeignKey("dbo.solicitud_permisos", "usuario_jefe", "dbo.usuarios");
            DropForeignKey("dbo.programa_usuarios", "programa_id", "dbo.programas");
            DropForeignKey("dbo.programa_usuarios", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.horarios", "sala_id", "dbo.salas");
            DropForeignKey("dbo.salas", "local_id", "dbo.locales");
            DropForeignKey("dbo.horarios", "programa_id", "dbo.programas");
            DropIndex("dbo.venta_usuarios", new[] { "cliente_id" });
            DropIndex("dbo.venta_usuarios", new[] { "local_id" });
            DropIndex("dbo.solicitud_permisos", new[] { "usuario_solicitante" });
            DropIndex("dbo.solicitud_permisos", new[] { "usuario_jefe" });
            DropIndex("dbo.programa_usuarios", new[] { "cliente_id" });
            DropIndex("dbo.programa_usuarios", new[] { "programa_id" });
            DropIndex("dbo.salas", new[] { "local_id" });
            DropIndex("dbo.horarios", new[] { "programa_id" });
            DropIndex("dbo.horarios", new[] { "sala_id" });
            DropTable("dbo.venta_usuarios");
            DropTable("dbo.solicitud_permisos");
            DropTable("dbo.prospectos");
            DropTable("dbo.programa_usuarios");
            DropTable("dbo.locales");
            DropTable("dbo.salas");
            DropTable("dbo.horarios");
            DropTable("dbo.campana_marketing");
        }
    }
}
