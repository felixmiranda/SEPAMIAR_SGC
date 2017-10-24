namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableDietas_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.clientes_dietas", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.clientes_dietas", "updated_at", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.clientes_dietas", "updated_at");
            DropColumn("dbo.clientes_dietas", "created_at");
        }
    }
}
