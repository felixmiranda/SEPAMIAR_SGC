namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFreezingWeeksTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.congelamiento_semanas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        desde = c.Int(nullable: false),
                        hasta = c.Int(nullable: false),
                        cantidad_dias = c.Int(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        deleted_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.congelamiento_semanas");
        }
    }
}
