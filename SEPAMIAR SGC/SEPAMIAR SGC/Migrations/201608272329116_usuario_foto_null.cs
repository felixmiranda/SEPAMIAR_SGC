namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usuario_foto_null : DbMigration
    {
        public override void Up()
        {
			DropColumn("dbo.usuarios", "foto");
			AddColumn("dbo.usuarios", "foto", c => c.Binary());
		}
        
        public override void Down()
        {
            AddColumn("dbo.usuarios", "foto", c => c.Binary());
        }
    }
}
