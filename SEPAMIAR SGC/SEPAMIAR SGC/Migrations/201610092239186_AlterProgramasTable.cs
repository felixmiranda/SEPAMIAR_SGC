namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterProgramasTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.programas", "imagen", c => c.String());
            AlterColumn("dbo.programas", "icono", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.programas", "icono", c => c.Binary());
            AlterColumn("dbo.programas", "imagen", c => c.Binary());
        }
    }
}
