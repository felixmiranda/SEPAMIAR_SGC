namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removePriceForProgram : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.programas", "precio");
        }
        
        public override void Down()
        {
            AddColumn("dbo.programas", "precio", c => c.Double(nullable: false));
        }
    }
}
