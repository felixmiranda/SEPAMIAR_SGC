namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.citas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        cliente_id = c.Int(nullable: false),
                        nutricionista_id = c.Int(nullable: false),
                        programa_id = c.Int(nullable: false),
                        fecha = c.DateTime(nullable: false, storeType: "date"),
                        hora = c.Time(nullable: false, precision: 7),
                        tipo = c.String(nullable: false, maxLength: 20),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.clientes", t => t.cliente_id)
                .ForeignKey("dbo.programas", t => t.programa_id)
                .ForeignKey("dbo.nutricionistas", t => t.nutricionista_id)
                .Index(t => t.cliente_id)
                .Index(t => t.nutricionista_id)
                .Index(t => t.programa_id);
            
            CreateTable(
                "dbo.clientes",
                c => new
                    {
                        usuario_id = c.Int(nullable: false),
                        fecha_nacimiento = c.DateTime(nullable: false, storeType: "date"),
                        genero = c.String(nullable: false, maxLength: 5),
                        documento_tipo = c.String(nullable: false, maxLength: 3),
                        documento_numero = c.String(nullable: false, maxLength: 10),
                        direccion = c.String(maxLength: 255),
                        distrito = c.String(maxLength: 255),
                        telefono = c.String(maxLength: 25),
                        celular = c.String(maxLength: 25),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.usuario_id)
                .ForeignKey("dbo.usuarios", t => t.usuario_id, cascadeDelete: true)
                .Index(t => t.usuario_id);
            
            CreateTable(
                "dbo.fichas_medicas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        programa_id = c.Int(nullable: false),
                        cliente_id = c.Int(nullable: false),
                        estatura = c.Double(nullable: false),
                        peso_actual = c.Double(nullable: false),
                        peso_deseado = c.Double(nullable: false),
                        peso_ideal = c.Double(nullable: false),
                        presion_arterial_sistolica = c.Double(nullable: false),
                        presion_arterial_diastolica = c.Double(nullable: false),
                        hemoglobina = c.Double(nullable: false),
                        colesterol = c.Double(nullable: false),
                        creatinina = c.Double(nullable: false),
                        glucosa_basal = c.Double(nullable: false),
                        trigliceridos = c.Double(nullable: false),
                        ultimo_examen_vigente = c.Double(nullable: false),
                        historial_hijos = c.Int(nullable: false),
                        historial_frecuencia_alcohol = c.String(nullable: false, maxLength: 20, unicode: false),
                        historial_drogas = c.String(maxLength: 20, unicode: false),
                        mm_hijos = c.Int(nullable: false),
                        mm_tipo_parto = c.String(nullable: false, maxLength: 20, unicode: false),
                        mm_menopausia_edad = c.Int(),
                        mm_menopausia_proceso_contiua = c.Boolean(),
                        mm_tratamiento_hormonal = c.String(storeType: "ntext"),
                        problemas_salud = c.String(maxLength: 20),
                        problemas_traumatologicos = c.String(maxLength: 20),
                        problemas_columna = c.String(storeType: "ntext"),
                        otras_lesiones_importantes = c.String(storeType: "ntext"),
                        problemas_respiratorios = c.String(maxLength: 20),
                        antecedentes = c.String(maxLength: 20),
                        an_alimentos_fuera_casa = c.Boolean(nullable: false),
                        an_alimentos_fuera_casa_frecuencia = c.String(nullable: false, maxLength: 20, unicode: false),
                        an_tipo_comida = c.String(storeType: "ntext"),
                        an_vasos_agua = c.Int(nullable: false),
                        an_alimentos_preferencia = c.String(storeType: "ntext"),
                        an_alimentos_desagrada = c.String(storeType: "ntext"),
                        an_alimentos_dano = c.String(storeType: "ntext"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.programas", t => t.programa_id, cascadeDelete: true)
                .ForeignKey("dbo.clientes", t => t.cliente_id, cascadeDelete: true)
                .Index(t => t.programa_id)
                .Index(t => t.cliente_id);
            
            CreateTable(
                "dbo.fichas_medicas_controles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ficha_medica_id = c.Int(nullable: false),
                        fecha = c.DateTime(nullable: false, storeType: "date"),
                        act = c.Double(nullable: false),
                        mme = c.Double(nullable: false),
                        mgc = c.Double(nullable: false),
                        mc = c.Double(nullable: false),
                        pgc = c.Double(nullable: false),
                        rcc = c.Double(nullable: false),
                        peso = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.fichas_medicas", t => t.ficha_medica_id, cascadeDelete: true)
                .Index(t => t.ficha_medica_id);
            
            CreateTable(
                "dbo.programas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 255),
                        precio = c.Double(nullable: false),
                        semanas = c.Int(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.regimen_alimentacion_diarias",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ficha_medica_id = c.Int(nullable: false),
                        tipo_comida = c.String(nullable: false, maxLength: 255, unicode: false),
                        hora = c.Time(nullable: false, precision: 7),
                        detalles = c.String(storeType: "ntext"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.fichas_medicas", t => t.ficha_medica_id, cascadeDelete: true)
                .Index(t => t.ficha_medica_id);
            
            CreateTable(
                "dbo.usuarios",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        codigo = c.String(nullable: false, maxLength: 255),
                        nombres = c.String(nullable: false, maxLength: 255),
                        apellidos = c.String(nullable: false, maxLength: 255),
                        email = c.String(nullable: false, maxLength: 255),
                        password = c.String(nullable: false, maxLength: 255),
                        foto = c.String(nullable: false, maxLength: 255),
                        tipo = c.String(nullable: false, maxLength: 15, unicode: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.nutricionistas",
                c => new
                    {
                        usuario_id = c.Int(nullable: false),
                        jefe = c.Boolean(),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.usuario_id)
                .ForeignKey("dbo.usuarios", t => t.usuario_id, cascadeDelete: true)
                .Index(t => t.usuario_id);
            
            CreateTable(
                "dbo.usuario_permisos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        usuario_id = c.Int(nullable: false),
                        permiso_id = c.Int(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.permisos", t => t.permiso_id, cascadeDelete: true)
                .ForeignKey("dbo.usuarios", t => t.usuario_id, cascadeDelete: true)
                .Index(t => t.usuario_id)
                .Index(t => t.permiso_id);
            
            CreateTable(
                "dbo.permisos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 255),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.vendedores",
                c => new
                    {
                        usuario_id = c.Int(nullable: false),
                        jefe = c.Boolean(),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.usuario_id)
                .ForeignKey("dbo.usuarios", t => t.usuario_id, cascadeDelete: true)
                .Index(t => t.usuario_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.vendedores", "usuario_id", "dbo.usuarios");
            DropForeignKey("dbo.usuario_permisos", "usuario_id", "dbo.usuarios");
            DropForeignKey("dbo.usuario_permisos", "permiso_id", "dbo.permisos");
            DropForeignKey("dbo.nutricionistas", "usuario_id", "dbo.usuarios");
            DropForeignKey("dbo.citas", "nutricionista_id", "dbo.nutricionistas");
            DropForeignKey("dbo.clientes", "usuario_id", "dbo.usuarios");
            DropForeignKey("dbo.fichas_medicas", "cliente_id", "dbo.clientes");
            DropForeignKey("dbo.regimen_alimentacion_diarias", "ficha_medica_id", "dbo.fichas_medicas");
            DropForeignKey("dbo.fichas_medicas", "programa_id", "dbo.programas");
            DropForeignKey("dbo.citas", "programa_id", "dbo.programas");
            DropForeignKey("dbo.fichas_medicas_controles", "ficha_medica_id", "dbo.fichas_medicas");
            DropForeignKey("dbo.citas", "cliente_id", "dbo.clientes");
            DropIndex("dbo.vendedores", new[] { "usuario_id" });
            DropIndex("dbo.usuario_permisos", new[] { "permiso_id" });
            DropIndex("dbo.usuario_permisos", new[] { "usuario_id" });
            DropIndex("dbo.nutricionistas", new[] { "usuario_id" });
            DropIndex("dbo.regimen_alimentacion_diarias", new[] { "ficha_medica_id" });
            DropIndex("dbo.fichas_medicas_controles", new[] { "ficha_medica_id" });
            DropIndex("dbo.fichas_medicas", new[] { "cliente_id" });
            DropIndex("dbo.fichas_medicas", new[] { "programa_id" });
            DropIndex("dbo.clientes", new[] { "usuario_id" });
            DropIndex("dbo.citas", new[] { "programa_id" });
            DropIndex("dbo.citas", new[] { "nutricionista_id" });
            DropIndex("dbo.citas", new[] { "cliente_id" });
            DropTable("dbo.vendedores");
            DropTable("dbo.permisos");
            DropTable("dbo.usuario_permisos");
            DropTable("dbo.nutricionistas");
            DropTable("dbo.usuarios");
            DropTable("dbo.regimen_alimentacion_diarias");
            DropTable("dbo.programas");
            DropTable("dbo.fichas_medicas_controles");
            DropTable("dbo.fichas_medicas");
            DropTable("dbo.clientes");
            DropTable("dbo.citas");
        }
    }
}
