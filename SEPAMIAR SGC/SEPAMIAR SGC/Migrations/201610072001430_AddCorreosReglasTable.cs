namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCorreosReglasTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.correos_reglas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Operador = c.Int(nullable: false),
                        Condicion = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.correos_reglas");
        }
    }
}
