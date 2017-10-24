namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estructura : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.clientes", "centro_laboral", c => c.String(nullable: false));
            AddColumn("dbo.clientes", "cargo_laboral", c => c.String(nullable: false));
            AddColumn("dbo.clientes", "email_empresa", c => c.String(nullable: false));
            AddColumn("dbo.clientes", "ce_nombres", c => c.String());
            AddColumn("dbo.clientes", "ce_apellidos", c => c.String());
            AddColumn("dbo.clientes", "ce_telefono", c => c.String());
            AddColumn("dbo.clientes", "ce_celular", c => c.String());
            AddColumn("dbo.clientes", "ce_email", c => c.String());
            AddColumn("dbo.clientes", "como_se_entero", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.fichas_medicas", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.fichas_medicas", "deleted_at", c => c.DateTime());
            AddColumn("dbo.fichas_medicas_controles", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.fichas_medicas_controles", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.fichas_medicas_controles", "deleted_at", c => c.DateTime());
            AddColumn("dbo.programas", "dias_congelamiento", c => c.Int(nullable: false));
            AddColumn("dbo.regimen_alimentacion_diarias", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.regimen_alimentacion_diarias", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.regimen_alimentacion_diarias", "deleted_at", c => c.DateTime());
            AddColumn("dbo.usuarios", "activo", c => c.Boolean(nullable: false));
            AlterColumn("dbo.clientes", "genero", c => c.Int(nullable: false));
            AlterColumn("dbo.clientes", "documento_tipo", c => c.Int(nullable: false));
            //AlterColumn("dbo.usuarios", "foto", c => c.Binary());

			AddColumn("dbo.usuarios", "foto_bynary", c => c.Binary());
			Sql("Update dbo.usuarios SET foto_bynary = Convert(varbinary, foto_bynary)");
			DropColumn("dbo.usuarios", "foto");
			RenameColumn("dbo.usuarios", "foto_bynary", "foto");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.usuarios", "foto", c => c.String(maxLength: 255));
            AlterColumn("dbo.clientes", "documento_tipo", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.clientes", "genero", c => c.String(nullable: false, maxLength: 5));
            DropColumn("dbo.usuarios", "activo");
            DropColumn("dbo.regimen_alimentacion_diarias", "deleted_at");
            DropColumn("dbo.regimen_alimentacion_diarias", "updated_at");
            DropColumn("dbo.regimen_alimentacion_diarias", "created_at");
            DropColumn("dbo.programas", "dias_congelamiento");
            DropColumn("dbo.fichas_medicas_controles", "deleted_at");
            DropColumn("dbo.fichas_medicas_controles", "updated_at");
            DropColumn("dbo.fichas_medicas_controles", "created_at");
            DropColumn("dbo.fichas_medicas", "deleted_at");
            DropColumn("dbo.fichas_medicas", "updated_at");
            DropColumn("dbo.fichas_medicas", "created_at");
            DropColumn("dbo.clientes", "como_se_entero");
            DropColumn("dbo.clientes", "ce_email");
            DropColumn("dbo.clientes", "ce_celular");
            DropColumn("dbo.clientes", "ce_telefono");
            DropColumn("dbo.clientes", "ce_apellidos");
            DropColumn("dbo.clientes", "ce_nombres");
            DropColumn("dbo.clientes", "email_empresa");
            DropColumn("dbo.clientes", "cargo_laboral");
            DropColumn("dbo.clientes", "centro_laboral");
        }
    }
}
