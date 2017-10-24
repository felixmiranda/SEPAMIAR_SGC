namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientes_notas_fechas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.clientes_notas", "created_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.clientes_notas", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.clientes_notas", "deleted_at", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.clientes_notas", "deleted_at");
            DropColumn("dbo.clientes_notas", "updated_at");
            DropColumn("dbo.clientes_notas", "created_at");
        }
    }
}
