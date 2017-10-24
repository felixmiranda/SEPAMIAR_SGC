namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserTableAlterType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.usuarios", "tipo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.usuarios", "tipo", c => c.String(nullable: false, maxLength: 15, unicode: false));
        }
    }
}
