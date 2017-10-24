namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkFotoClientes : DbMigration
    {
        public override void Up()
        {
			DropColumn("dbo.clientes", "foto");
			AddColumn("dbo.clientes", "foto", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.clientes", "foto");
        }
    }
}
