namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambio_de_nombre_injutificado : DbMigration
    {
        public override void Up()
        {
			DropColumn("dbo.clientes", "foto");
			AddColumn("dbo.clientes", "foto", c => c.Binary(nullable: true));
		}
        
        public override void Down()
        {
        }
    }
}
