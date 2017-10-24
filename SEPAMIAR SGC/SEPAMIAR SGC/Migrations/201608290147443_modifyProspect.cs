namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyProspect : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.prospectos", "telefono", c => c.String());
            AlterColumn("dbo.prospectos", "documento_numero", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.prospectos", "documento_numero", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.prospectos", "telefono");
        }
    }
}
