namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomCodeToPermisosTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.permisos", "codigo_interno", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.permisos", "codigo_interno");
        }
    }
}
