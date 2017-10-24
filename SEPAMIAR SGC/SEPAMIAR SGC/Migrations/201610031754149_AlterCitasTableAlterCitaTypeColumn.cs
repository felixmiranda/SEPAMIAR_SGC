namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCitasTableAlterCitaTypeColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.citas", "tipo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.citas", "tipo", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
