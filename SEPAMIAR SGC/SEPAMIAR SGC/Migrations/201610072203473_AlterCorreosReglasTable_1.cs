namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCorreosReglasTable_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.correos_reglas", "EsRegla", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.correos_reglas", "EsRegla");
        }
    }
}
