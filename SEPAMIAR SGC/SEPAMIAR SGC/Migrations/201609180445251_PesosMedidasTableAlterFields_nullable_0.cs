namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PesosMedidasTableAlterFields_nullable_0 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.pesos_medidas", "porc_grasa_corporal", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.pesos_medidas", "porc_grasa_corporal", c => c.Double(nullable: false));
        }
    }
}
