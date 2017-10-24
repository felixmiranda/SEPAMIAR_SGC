namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PesosMedidasTableAlterFields_nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.pesos_medidas", "peso_2", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "peso_3", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_cuello", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_hombros", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_torax", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_biceps", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_muneca", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_cintura", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_gluteos", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_muslo", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "medidas_pantorrilla", c => c.Double());
            AlterColumn("dbo.pesos_medidas", "deleted_at", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.pesos_medidas", "deleted_by", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.pesos_medidas", "deleted_by", c => c.Int(nullable: false));
            AlterColumn("dbo.pesos_medidas", "deleted_at", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.pesos_medidas", "medidas_pantorrilla", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "medidas_muslo", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "medidas_gluteos", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "medidas_cintura", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "medidas_muneca", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "medidas_biceps", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "medidas_torax", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "medidas_hombros", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "medidas_cuello", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "peso_3", c => c.Double(nullable: false));
            AlterColumn("dbo.pesos_medidas", "peso_2", c => c.Double(nullable: false));
        }
    }
}
