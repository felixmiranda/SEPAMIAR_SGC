namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableDietas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DailyDiets", "opcion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DailyDiets", "opcion");
        }
    }
}
