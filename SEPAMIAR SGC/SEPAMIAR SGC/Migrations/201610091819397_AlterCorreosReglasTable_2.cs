namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCorreosReglasTable_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.correos", "To", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.correos", "To");
        }
    }
}
