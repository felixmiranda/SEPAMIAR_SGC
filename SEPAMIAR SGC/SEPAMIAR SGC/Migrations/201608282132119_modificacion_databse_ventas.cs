namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modificacion_databse_ventas : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.programa_usuarios", newName: "programa_clientes");
            AddColumn("dbo.clientes", "acceso_autorizado", c => c.Boolean(nullable: false));
            AddColumn("dbo.horarios", "lunes", c => c.Boolean(nullable: false));
            AddColumn("dbo.horarios", "martes", c => c.Boolean(nullable: false));
            AddColumn("dbo.horarios", "miercoles", c => c.Boolean(nullable: false));
            AddColumn("dbo.horarios", "jueves", c => c.Boolean(nullable: false));
            AddColumn("dbo.horarios", "viernes", c => c.Boolean(nullable: false));
            AddColumn("dbo.horarios", "sabado", c => c.Boolean(nullable: false));
            AddColumn("dbo.horarios", "domingo", c => c.Boolean(nullable: false));
            AddColumn("dbo.horarios", "hora", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.prospectos", "centros_laboral", c => c.String());
            AddColumn("dbo.prospectos", "cargo_laboral", c => c.String());
            AddColumn("dbo.solicitud_permisos", "modelo_nombre", c => c.String());
            AddColumn("dbo.solicitud_permisos", "modelo_id", c => c.Int(nullable: false));
            AddColumn("dbo.solicitud_permisos", "autorizado", c => c.Boolean(nullable: false));
            AddColumn("dbo.venta_usuarios", "fecha_inicio", c => c.DateTime(nullable: false));
            AddColumn("dbo.venta_usuarios", "fecha_fin", c => c.DateTime(nullable: false));
            AddColumn("dbo.venta_usuarios", "cantidad_semanas", c => c.Int(nullable: false));
            AddColumn("dbo.venta_usuarios", "cantidad_dias_congelamiento", c => c.Int(nullable: false));
            AddColumn("dbo.venta_usuarios", "solicitud_permiso_congelamiento", c => c.Boolean(nullable: false));
            AddColumn("dbo.venta_usuarios", "tipo_ingreso", c => c.Int(nullable: false));
            AddColumn("dbo.venta_usuarios", "campana_marketing_id", c => c.Int());
            AddColumn("dbo.venta_usuarios", "numero_boleta", c => c.String(maxLength: 20));
            AddColumn("dbo.venta_usuarios", "tipo_pago", c => c.Int(nullable: false));
            CreateIndex("dbo.programa_clientes", "horario_id");
            CreateIndex("dbo.venta_usuarios", "numero_boleta", unique: true);
            AddForeignKey("dbo.programa_clientes", "horario_id", "dbo.horarios", "id");
            DropColumn("dbo.horarios", "dia_hora");
            DropColumn("dbo.programa_clientes", "tipo_ingreso");
            DropColumn("dbo.programa_clientes", "campana_marketing_id");
            DropColumn("dbo.prospectos", "centrol_laboral");
            DropColumn("dbo.solicitud_permisos", "model_id");
            DropColumn("dbo.solicitud_permisos", "permiso_id");
            DropColumn("dbo.solicitud_permisos", "respuesta");
            DropColumn("dbo.venta_usuarios", "boleta");
        }
        
        public override void Down()
        {
            AddColumn("dbo.venta_usuarios", "boleta", c => c.String());
            AddColumn("dbo.solicitud_permisos", "respuesta", c => c.Boolean(nullable: false));
            AddColumn("dbo.solicitud_permisos", "permiso_id", c => c.Int(nullable: false));
            AddColumn("dbo.solicitud_permisos", "model_id", c => c.Int(nullable: false));
            AddColumn("dbo.prospectos", "centrol_laboral", c => c.String());
            AddColumn("dbo.programa_clientes", "campana_marketing_id", c => c.Int());
            AddColumn("dbo.programa_clientes", "tipo_ingreso", c => c.Int(nullable: false));
            AddColumn("dbo.horarios", "dia_hora", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.programa_clientes", "horario_id", "dbo.horarios");
            DropIndex("dbo.venta_usuarios", new[] { "numero_boleta" });
            DropIndex("dbo.programa_clientes", new[] { "horario_id" });
            DropColumn("dbo.venta_usuarios", "tipo_pago");
            DropColumn("dbo.venta_usuarios", "numero_boleta");
            DropColumn("dbo.venta_usuarios", "campana_marketing_id");
            DropColumn("dbo.venta_usuarios", "tipo_ingreso");
            DropColumn("dbo.venta_usuarios", "solicitud_permiso_congelamiento");
            DropColumn("dbo.venta_usuarios", "cantidad_dias_congelamiento");
            DropColumn("dbo.venta_usuarios", "cantidad_semanas");
            DropColumn("dbo.venta_usuarios", "fecha_fin");
            DropColumn("dbo.venta_usuarios", "fecha_inicio");
            DropColumn("dbo.solicitud_permisos", "autorizado");
            DropColumn("dbo.solicitud_permisos", "modelo_id");
            DropColumn("dbo.solicitud_permisos", "modelo_nombre");
            DropColumn("dbo.prospectos", "cargo_laboral");
            DropColumn("dbo.prospectos", "centros_laboral");
            DropColumn("dbo.horarios", "hora");
            DropColumn("dbo.horarios", "domingo");
            DropColumn("dbo.horarios", "sabado");
            DropColumn("dbo.horarios", "viernes");
            DropColumn("dbo.horarios", "jueves");
            DropColumn("dbo.horarios", "miercoles");
            DropColumn("dbo.horarios", "martes");
            DropColumn("dbo.horarios", "lunes");
            DropColumn("dbo.clientes", "acceso_autorizado");
            RenameTable(name: "dbo.programa_clientes", newName: "programa_usuarios");
        }
    }
}
