namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTableCliente_Congelamientos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.clientes_congelamientos", "fecha_desde", c => c.DateTime(nullable: false));
            AddColumn("dbo.clientes_congelamientos", "fecha_hasta", c => c.DateTime(nullable: false));
            AddColumn("dbo.clientes_congelamientos", "dias_congelados", c => c.Int(nullable: false));
            DropColumn("dbo.clientes_congelamientos", "fecha");
        }
        
        public override void Down()
        {
            AddColumn("dbo.clientes_congelamientos", "fecha", c => c.DateTime(nullable: false));
            DropColumn("dbo.clientes_congelamientos", "dias_congelados");
            DropColumn("dbo.clientes_congelamientos", "fecha_hasta");
            DropColumn("dbo.clientes_congelamientos", "fecha_desde");
        }
    }
}
