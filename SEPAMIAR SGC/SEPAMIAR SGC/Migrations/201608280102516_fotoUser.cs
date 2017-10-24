namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fotoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.clientes", "foto", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.clientes", "foto");
        }
    }
}
