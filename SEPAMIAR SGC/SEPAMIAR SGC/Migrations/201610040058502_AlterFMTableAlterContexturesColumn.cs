namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFMTableAlterContexturesColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.fichas_medicas", "contextura", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.fichas_medicas", "contextura", c => c.Int(nullable: false));
        }
    }
}
