namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AACC_100916_1749 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.regimen_alimentacion_diarias", "fichas_medicas_id", "dbo.fichas_medicas");
            DropIndex("dbo.regimen_alimentacion_diarias", new[] { "fichas_medicas_id" });
            CreateTable(
                "dbo.FeedingRegimes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        tipo_comida = c.Int(nullable: false),
                        hora = c.Time(nullable: false, precision: 7),
                        detalles = c.String(),
                        fichas_medicas_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.fichas_medicas", t => t.fichas_medicas_id)
                .Index(t => t.fichas_medicas_id);
            
            AddColumn("dbo.fichas_medicas", "cortisona", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "esteroidesAnabolicos", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "anfetaminas", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "marihuana", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "otra", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "ninguna", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "menstruacion_menopausia_cantidad_hijos", c => c.Int());
            AddColumn("dbo.fichas_medicas", "menstruacion_menopausia_tipo_parto", c => c.Int());
            AddColumn("dbo.fichas_medicas", "menstruacion_menopausia_edad_menopausia", c => c.Int());
            AddColumn("dbo.fichas_medicas", "menstruacion_menopausia_menopausia_activa", c => c.Boolean());
            AddColumn("dbo.fichas_medicas", "menstruacion_menopausia_tratamiento_hormonal", c => c.String());
            AddColumn("dbo.fichas_medicas", "antecedentes_gastricos", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_diabetes", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_trigliceridos_altos", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_hipertiroidismo", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_hipotiroidismo", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_quistes", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_tumores", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_resistencia_insulina", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_otras_dislipidemias", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_prostata", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_renales", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_hepaticos", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_presion_alta", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_colesterol_alto", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_anemia", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_hernias", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_estrenimiento", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentes_no_mencionada", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_traumatologicos_dolores_articulares", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_traumatologicos_osteoporosis", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_traumatologicos_problemas_columna", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_traumatologicos_problemas_columna_texto", c => c.String());
            AddColumn("dbo.fichas_medicas", "problemas_traumatologicos_otras_lesiones", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_traumatologicos_otras_lesiones_texto", c => c.String());
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios_asma_cronica", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios_tos_cronica", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios_resfriados_continuos", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios_bronquitis", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios_rinitis_alergica", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios_sinusitis", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios_amigdalitis", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "problemas_respiratorios_otros", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentas_generales_obesidad", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentas_generales_hipertension", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentas_generales_diabetes", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentas_generales_cardiopatias", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "antecedentas_generales_no_mencionada", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "anamnesis_nutricional_alimentos_fuera_de_casa", c => c.Boolean(nullable: false));
            AddColumn("dbo.fichas_medicas", "anamnesis_nutricional_frecAlimentosFuera", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "anamnesis_nutricional_tipo_alimentos_fuera", c => c.String());
            AddColumn("dbo.fichas_medicas", "anamnesis_nutricional_cantidad_vasos_agua_dia", c => c.Int(nullable: false));
            AddColumn("dbo.fichas_medicas", "anamnesis_nutricional_alimentos_preferencia", c => c.String());
            AddColumn("dbo.fichas_medicas", "anamnesis_nutricional_alimentos_no_preferencia", c => c.String());
            AddColumn("dbo.fichas_medicas", "anamnesis_nutricional_alimentos_daninos", c => c.String());
            AddColumn("dbo.fichas_medicas", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.fichas_medicas", "updated_at", c => c.DateTime(nullable: false));
            DropColumn("dbo.clientes_notas", "created_at");
            DropColumn("dbo.clientes_notas", "updated_at");
            DropColumn("dbo.clientes_notas", "deleted_at");
            DropTable("dbo.regimen_alimentacion_diarias");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.regimen_alimentacion_diarias",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ficha_medica_id = c.Int(nullable: false),
                        tipo_comida = c.Int(nullable: false),
                        hora = c.Time(nullable: false, precision: 7),
                        detalles = c.String(storeType: "ntext"),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                        fichas_medicas_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.clientes_notas", "deleted_at", c => c.DateTime());
            AddColumn("dbo.clientes_notas", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.clientes_notas", "created_at", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.FeedingRegimes", "fichas_medicas_id", "dbo.fichas_medicas");
            DropIndex("dbo.FeedingRegimes", new[] { "fichas_medicas_id" });
            DropColumn("dbo.fichas_medicas", "updated_at");
            DropColumn("dbo.fichas_medicas", "created_at");
            DropColumn("dbo.fichas_medicas", "anamnesis_nutricional_alimentos_daninos");
            DropColumn("dbo.fichas_medicas", "anamnesis_nutricional_alimentos_no_preferencia");
            DropColumn("dbo.fichas_medicas", "anamnesis_nutricional_alimentos_preferencia");
            DropColumn("dbo.fichas_medicas", "anamnesis_nutricional_cantidad_vasos_agua_dia");
            DropColumn("dbo.fichas_medicas", "anamnesis_nutricional_tipo_alimentos_fuera");
            DropColumn("dbo.fichas_medicas", "anamnesis_nutricional_frecAlimentosFuera");
            DropColumn("dbo.fichas_medicas", "anamnesis_nutricional_alimentos_fuera_de_casa");
            DropColumn("dbo.fichas_medicas", "antecedentas_generales_no_mencionada");
            DropColumn("dbo.fichas_medicas", "antecedentas_generales_cardiopatias");
            DropColumn("dbo.fichas_medicas", "antecedentas_generales_diabetes");
            DropColumn("dbo.fichas_medicas", "antecedentas_generales_hipertension");
            DropColumn("dbo.fichas_medicas", "antecedentas_generales_obesidad");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios_otros");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios_amigdalitis");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios_sinusitis");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios_rinitis_alergica");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios_bronquitis");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios_resfriados_continuos");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios_tos_cronica");
            DropColumn("dbo.fichas_medicas", "problemas_respiratorios_asma_cronica");
            DropColumn("dbo.fichas_medicas", "problemas_traumatologicos_otras_lesiones_texto");
            DropColumn("dbo.fichas_medicas", "problemas_traumatologicos_otras_lesiones");
            DropColumn("dbo.fichas_medicas", "problemas_traumatologicos_problemas_columna_texto");
            DropColumn("dbo.fichas_medicas", "problemas_traumatologicos_problemas_columna");
            DropColumn("dbo.fichas_medicas", "problemas_traumatologicos_osteoporosis");
            DropColumn("dbo.fichas_medicas", "problemas_traumatologicos_dolores_articulares");
            DropColumn("dbo.fichas_medicas", "antecedentes_no_mencionada");
            DropColumn("dbo.fichas_medicas", "antecedentes_estrenimiento");
            DropColumn("dbo.fichas_medicas", "antecedentes_hernias");
            DropColumn("dbo.fichas_medicas", "antecedentes_anemia");
            DropColumn("dbo.fichas_medicas", "antecedentes_colesterol_alto");
            DropColumn("dbo.fichas_medicas", "antecedentes_presion_alta");
            DropColumn("dbo.fichas_medicas", "antecedentes_hepaticos");
            DropColumn("dbo.fichas_medicas", "antecedentes_renales");
            DropColumn("dbo.fichas_medicas", "antecedentes_prostata");
            DropColumn("dbo.fichas_medicas", "antecedentes_otras_dislipidemias");
            DropColumn("dbo.fichas_medicas", "antecedentes_resistencia_insulina");
            DropColumn("dbo.fichas_medicas", "antecedentes_tumores");
            DropColumn("dbo.fichas_medicas", "antecedentes_quistes");
            DropColumn("dbo.fichas_medicas", "antecedentes_hipotiroidismo");
            DropColumn("dbo.fichas_medicas", "antecedentes_hipertiroidismo");
            DropColumn("dbo.fichas_medicas", "antecedentes_trigliceridos_altos");
            DropColumn("dbo.fichas_medicas", "antecedentes_diabetes");
            DropColumn("dbo.fichas_medicas", "antecedentes_gastricos");
            DropColumn("dbo.fichas_medicas", "menstruacion_menopausia_tratamiento_hormonal");
            DropColumn("dbo.fichas_medicas", "menstruacion_menopausia_menopausia_activa");
            DropColumn("dbo.fichas_medicas", "menstruacion_menopausia_edad_menopausia");
            DropColumn("dbo.fichas_medicas", "menstruacion_menopausia_tipo_parto");
            DropColumn("dbo.fichas_medicas", "menstruacion_menopausia_cantidad_hijos");
            DropColumn("dbo.fichas_medicas", "ninguna");
            DropColumn("dbo.fichas_medicas", "otra");
            DropColumn("dbo.fichas_medicas", "marihuana");
            DropColumn("dbo.fichas_medicas", "anfetaminas");
            DropColumn("dbo.fichas_medicas", "esteroidesAnabolicos");
            DropColumn("dbo.fichas_medicas", "cortisona");
            DropTable("dbo.FeedingRegimes");
            CreateIndex("dbo.regimen_alimentacion_diarias", "fichas_medicas_id");
            AddForeignKey("dbo.regimen_alimentacion_diarias", "fichas_medicas_id", "dbo.fichas_medicas", "id");
        }
    }
}
