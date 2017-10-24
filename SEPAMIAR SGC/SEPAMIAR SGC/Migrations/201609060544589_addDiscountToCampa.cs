namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDiscountToCampa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.campana_marketing", "dscto", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.campana_marketing", "dscto");
        }
    }
}
