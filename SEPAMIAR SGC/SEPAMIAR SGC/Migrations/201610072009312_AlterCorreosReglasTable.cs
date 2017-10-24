namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCorreosReglasTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.correos_reglas", "Tabla", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.correos_reglas", "Tabla");
        }
    }
}
