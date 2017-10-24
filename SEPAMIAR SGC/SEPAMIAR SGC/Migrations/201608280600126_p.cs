namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class p : DbMigration
    {
        public override void Up()
        {
            //MoveTable(name: "Nutricion.citas", newSchema: "dbo");
        }
        
        public override void Down()
        {
            //MoveTable(name: "dbo.citas", newSchema: "Nutricion");
        }
    }
}
