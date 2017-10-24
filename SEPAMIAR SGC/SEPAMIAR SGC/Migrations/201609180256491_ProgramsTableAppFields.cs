namespace SEPAMIAR_SGC.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class ProgramsTableAppFields : DbMigration
	{
		public override void Up()
		{
			AddColumn("dbo.programas", "frase", c => c.String(nullable: true, maxLength: 100, unicode: true));
			AddColumn("dbo.programas", "imagen", c => c.Binary());
			AddColumn("dbo.programas", "descripcion", c => c.String(nullable: true, unicode: true));
			AddColumn("dbo.programas", "fuerza", c => c.Int(nullable: true));
			AddColumn("dbo.programas", "resistencia", c => c.Int(nullable: true));
			AddColumn("dbo.programas", "intensidad", c => c.Int(nullable: true));
			AddColumn("dbo.programas", "icono", c => c.Binary());
		}

		public override void Down()
		{
			DropColumn("dbo.programas", "icono");
			DropColumn("dbo.programas", "intensidad");
			DropColumn("dbo.programas", "resistencia");
			DropColumn("dbo.programas", "fuerza");
			DropColumn("dbo.programas", "descripcion");
			DropColumn("dbo.programas", "imagen");
			DropColumn("dbo.programas", "frase");
		}
	}
}
