namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientsTablePhotoColumn : DbMigration
    {
        public override void Up()
        {
			DropColumn("dbo.clientes", "foto");
            AddColumn("dbo.clientes", "foto", c => c.Binary(storeType: "image"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.clientes", "foto", c => c.String());
        }
    }
}
