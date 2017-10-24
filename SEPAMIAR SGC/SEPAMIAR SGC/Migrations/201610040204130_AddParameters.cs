namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParameters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.fichas_medicas", "antecedentes_presion_baja", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.fichas_medicas", "antecedentes_presion_baja");
        }
    }
}
