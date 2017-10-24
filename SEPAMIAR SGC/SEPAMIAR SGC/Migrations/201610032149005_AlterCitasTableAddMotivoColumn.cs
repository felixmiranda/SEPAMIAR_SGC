namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCitasTableAddMotivoColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.citas", "motivo_reprogramacion", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.citas", "motivo_reprogramacion");
        }
    }
}
