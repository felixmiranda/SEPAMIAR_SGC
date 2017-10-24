namespace SEPAMIAR_SGC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCorreosTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.correos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreMail = c.String(),
                        Asunto = c.String(),
                        Contenido = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.correos");
        }
    }
}
