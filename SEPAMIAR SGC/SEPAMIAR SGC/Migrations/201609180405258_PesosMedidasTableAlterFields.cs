namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PesosMedidasTableAlterFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.pesos_medidas", "peso_1", c => c.Double(nullable: false));
            AddColumn("dbo.pesos_medidas", "peso_2", c => c.Double(nullable: true));
            AddColumn("dbo.pesos_medidas", "peso_3", c => c.Double(nullable: true));
            DropColumn("dbo.pesos_medidas", "peso");
        }
        
        public override void Down()
        {
            AddColumn("dbo.pesos_medidas", "peso", c => c.Double(nullable: false));
            DropColumn("dbo.pesos_medidas", "peso_3");
            DropColumn("dbo.pesos_medidas", "peso_2");
            DropColumn("dbo.pesos_medidas", "peso_1");
        }
    }
}
