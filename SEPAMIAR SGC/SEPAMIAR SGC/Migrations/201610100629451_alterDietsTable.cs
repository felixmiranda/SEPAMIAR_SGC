namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterDietsTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DailyDiets", new[] { "clientes_dietas_id" });
            AlterColumn("dbo.DailyDiets", "clientes_dietas_id", c => c.Int(nullable: false));
            CreateIndex("dbo.DailyDiets", "clientes_dietas_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DailyDiets", new[] { "clientes_dietas_id" });
            AlterColumn("dbo.DailyDiets", "clientes_dietas_id", c => c.Int());
            CreateIndex("dbo.DailyDiets", "clientes_dietas_id");
        }
    }
}
