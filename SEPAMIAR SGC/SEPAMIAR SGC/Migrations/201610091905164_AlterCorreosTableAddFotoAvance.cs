namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCorreosTableAddFotoAvance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.citas", "foto_avance", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.citas", "foto_avance");
        }
    }
}
