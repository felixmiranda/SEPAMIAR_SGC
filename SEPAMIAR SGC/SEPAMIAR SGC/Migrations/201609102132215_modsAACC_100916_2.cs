namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modsAACC_100916_2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.fichas_medicas_controles", newName: "ControlesNutricionales");
            DropForeignKey("dbo.regimen_alimentacion_diarias", "ficha_medica_id", "dbo.fichas_medicas");
            DropIndex("dbo.ControlesNutricionales", new[] { "ficha_medica_id" });
            DropIndex("dbo.regimen_alimentacion_diarias", new[] { "ficha_medica_id" });
            RenameColumn(table: "dbo.ControlesNutricionales", name: "ficha_medica_id", newName: "fichas_medicas_id");
            CreateTable(
                "dbo.CardioInfoes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        presion_arterial_sistolica = c.Double(nullable: false),
                        presion_arterial_diastolica = c.Double(nullable: false),
                        cardiologia_aprobacion = c.Int(nullable: false),
                        fecha = c.DateTime(nullable: false),
                        fichas_medicas_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.fichas_medicas", t => t.fichas_medicas_id)
                .Index(t => t.fichas_medicas_id);
            
            CreateTable(
                "dbo.LabResults",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        hemoglobina = c.Double(nullable: false),
                        colesterol = c.Double(nullable: false),
                        creatinina = c.Double(nullable: false),
                        glucosa_basal = c.Double(nullable: false),
                        trigliceridos = c.Double(nullable: false),
                        fecha = c.DateTime(nullable: false),
                        fichas_medicas_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.fichas_medicas", t => t.fichas_medicas_id)
                .Index(t => t.fichas_medicas_id);
            
            AddColumn("dbo.fichas_medicas", "contextura", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "frec_fumador", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "frec_bebedor", c => c.Int(nullable: false));
            AddColumn("dbo.regimen_alimentacion_diarias", "fichas_medicas_id", c => c.Int());
            AddColumn("dbo.clientes_notas", "usuarioId", c => c.Int(nullable: false));
            AlterColumn("dbo.ControlesNutricionales", "fichas_medicas_id", c => c.Int());
            AlterColumn("dbo.ControlesNutricionales", "fecha", c => c.DateTime(nullable: false));
            CreateIndex("dbo.ControlesNutricionales", "fichas_medicas_id");
            CreateIndex("dbo.clientes_notas", "usuarioId");
            CreateIndex("dbo.regimen_alimentacion_diarias", "fichas_medicas_id");
            AddForeignKey("dbo.clientes_notas", "usuarioId", "dbo.usuarios", "id");
            AddForeignKey("dbo.regimen_alimentacion_diarias", "fichas_medicas_id", "dbo.fichas_medicas", "id");
            DropColumn("dbo.fichas_medicas", "presion_arterial_sistolica");
            DropColumn("dbo.fichas_medicas", "presion_arterial_diastolica");
            DropColumn("dbo.fichas_medicas", "hemoglobina");
            DropColumn("dbo.fichas_medicas", "colesterol");
            DropColumn("dbo.fichas_medicas", "creatinina");
            DropColumn("dbo.fichas_medicas", "glucosa_basal");
            DropColumn("dbo.fichas_medicas", "trigliceridos");
            DropColumn("dbo.fichas_medicas", "ultimo_examen_vigente");
            DropColumn("dbo.fichas_medicas", "historial_hijos");
            DropColumn("dbo.fichas_medicas", "historial_frecuencia_alcohol");
            DropColumn("dbo.fichas_medicas", "historial_drogas");
            DropColumn("dbo.fichas_medicas", "mm_hijos");
            DropColumn("dbo.fichas_medicas", "mm_tipo_parto");
            DropColumn("dbo.fichas_medicas", "mm_menopausia_edad");
            DropColumn("dbo.fichas_medicas", "mm_menopausia_proceso_contiua");
            DropColumn("dbo.fichas_medicas", "mm_tratamiento_hormonal");
            DropColumn("dbo.fichas_medicas", "problemas_salud");
            DropColumn("dbo.fichas_medicas", "problemas_traumatologicos");
            DropColumn("dbo.fichas_medicas", "problemas_columna");
            DropColumn("dbo.fichas_medicas", "otras_lesiones_importantes");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios");
            DropColumn("dbo.fichas_medicas", "antecedentes");
            DropColumn("dbo.fichas_medicas", "an_alimentos_fuera_casa");
            DropColumn("dbo.fichas_medicas", "an_alimentos_fuera_casa_frecuencia");
            DropColumn("dbo.fichas_medicas", "an_tipo_comida");
            DropColumn("dbo.fichas_medicas", "an_vasos_agua");
            DropColumn("dbo.fichas_medicas", "an_alimentos_preferencia");
            DropColumn("dbo.fichas_medicas", "an_alimentos_desagrada");
            DropColumn("dbo.fichas_medicas", "an_alimentos_dano");
            DropColumn("dbo.fichas_medicas", "created_at");
            DropColumn("dbo.fichas_medicas", "updated_at");
            DropColumn("dbo.fichas_medicas", "deleted_at");
            DropColumn("dbo.ControlesNutricionales", "deleted_at");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlesNutricionales", "deleted_at", c => c.DateTime());
            AddColumn("dbo.fichas_medicas", "deleted_at", c => c.DateTime());
            AddColumn("dbo.fichas_medicas", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.fichas_medicas", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.fichas_medicas", "an_alimentos_dano", c => c.String(storeType: "ntext"));
            AddColumn("dbo.fichas_medicas", "an_alimentos_desagrada", c => c.String(storeType: "ntext"));
            AddColumn("dbo.fichas_medicas", "an_alimentos_preferencia", c => c.String(storeType: "ntext"));
            AddColumn("dbo.fichas_medicas", "an_vasos_agua", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "an_tipo_comida", c => c.String(storeType: "ntext"));
            AddColumn("dbo.fichas_medicas", "an_alimentos_fuera_casa_frecuencia", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AddColumn("dbo.fichas_medicas", "an_alimentos_fuera_casa", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes", c => c.String(maxLength: 20));
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios", c => c.String(maxLength: 20));
            AddColumn("dbo.fichas_medicas", "otras_lesiones_importantes", c => c.String(storeType: "ntext"));
            AddColumn("dbo.fichas_medicas", "problemas_columna", c => c.String(storeType: "ntext"));
            AddColumn("dbo.fichas_medicas", "problemas_traumatologicos", c => c.String(maxLength: 20));
            AddColumn("dbo.fichas_medicas", "problemas_salud", c => c.String(maxLength: 20));
            AddColumn("dbo.fichas_medicas", "mm_tratamiento_hormonal", c => c.String(storeType: "ntext"));
            AddColumn("dbo.fichas_medicas", "mm_menopausia_proceso_contiua", c => c.Boolean());
            AddColumn("dbo.fichas_medicas", "mm_menopausia_edad", c => c.Int());
            AddColumn("dbo.fichas_medicas", "mm_tipo_parto", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AddColumn("dbo.fichas_medicas", "mm_hijos", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "historial_drogas", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.fichas_medicas", "historial_frecuencia_alcohol", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AddColumn("dbo.fichas_medicas", "historial_hijos", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "ultimo_examen_vigente", c => c.Double(nullable: false));
            AddColumn("dbo.fichas_medicas", "trigliceridos", c => c.Double(nullable: false));
            AddColumn("dbo.fichas_medicas", "glucosa_basal", c => c.Double(nullable: false));
            AddColumn("dbo.fichas_medicas", "creatinina", c => c.Double(nullable: false));
            AddColumn("dbo.fichas_medicas", "colesterol", c => c.Double(nullable: false));
            AddColumn("dbo.fichas_medicas", "hemoglobina", c => c.Double(nullable: false));
            AddColumn("dbo.fichas_medicas", "presion_arterial_diastolica", c => c.Double(nullable: false));
            AddColumn("dbo.fichas_medicas", "presion_arterial_sistolica", c => c.Double(nullable: false));
            DropForeignKey("dbo.regimen_alimentacion_diarias", "fichas_medicas_id", "dbo.fichas_medicas");
            DropForeignKey("dbo.clientes_notas", "usuarioId", "dbo.usuarios");
            DropForeignKey("dbo.LabResults", "fichas_medicas_id", "dbo.fichas_medicas");
            DropForeignKey("dbo.CardioInfoes", "fichas_medicas_id", "dbo.fichas_medicas");
            DropIndex("dbo.regimen_alimentacion_diarias", new[] { "fichas_medicas_id" });
            DropIndex("dbo.clientes_notas", new[] { "usuarioId" });
            DropIndex("dbo.LabResults", new[] { "fichas_medicas_id" });
            DropIndex("dbo.ControlesNutricionales", new[] { "fichas_medicas_id" });
            DropIndex("dbo.CardioInfoes", new[] { "fichas_medicas_id" });
            AlterColumn("dbo.ControlesNutricionales", "fecha", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.ControlesNutricionales", "fichas_medicas_id", c => c.Int(nullable: false));
            DropColumn("dbo.clientes_notas", "usuarioId");
            DropColumn("dbo.regimen_alimentacion_diarias", "fichas_medicas_id");
            DropColumn("dbo.fichas_medicas", "frec_bebedor");
            DropColumn("dbo.fichas_medicas", "frec_fumador");
            DropColumn("dbo.fichas_medicas", "contextura");
            DropTable("dbo.LabResults");
            DropTable("dbo.CardioInfoes");
            RenameColumn(table: "dbo.ControlesNutricionales", name: "fichas_medicas_id", newName: "ficha_medica_id");
            CreateIndex("dbo.regimen_alimentacion_diarias", "ficha_medica_id");
            CreateIndex("dbo.ControlesNutricionales", "ficha_medica_id");
            AddForeignKey("dbo.regimen_alimentacion_diarias", "ficha_medica_id", "dbo.fichas_medicas", "id");
            RenameTable(name: "dbo.ControlesNutricionales", newName: "fichas_medicas_controles");
        }
    }
}
