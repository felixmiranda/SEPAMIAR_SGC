namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeFreezingForProgram : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.programas", "dias_congelamiento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.programas", "dias_congelamiento", c => c.Int(nullable: false));
        }
    }
}
