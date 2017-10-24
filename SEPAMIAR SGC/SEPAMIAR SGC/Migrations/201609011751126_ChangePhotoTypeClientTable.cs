namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePhotoTypeClientTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.clientes", "foto", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.clientes", "foto", c => c.Binary());
        }
    }
}
