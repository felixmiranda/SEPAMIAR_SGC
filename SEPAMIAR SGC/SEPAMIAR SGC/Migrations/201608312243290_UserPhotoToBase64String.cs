namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserPhotoToBase64String : DbMigration
    {
        public override void Up()
        {
			DropColumn("dbo.usuarios", "foto");
			AddColumn("dbo.usuarios", "foto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.usuarios", "foto");
        }
    }
}
