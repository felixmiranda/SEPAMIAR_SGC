namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using System.Data.Entity.ModelConfiguration.Conventions;

	public partial class _SGCModel : DbContext
	{
		public _SGCModel()
			: base("name=SGCModel")
		{
		}

		public virtual DbSet<campana_marketing> campana_marketing { get; set; }
		public virtual DbSet<citas> citas { get; set; }
		public virtual DbSet<clientes_asistencia> clientes_asistencia { get; set; }
		public virtual DbSet<clientes_congelamientos> clientes_congelamientos { get; set; }
		public virtual DbSet<clientes> clientes { get; set; }
		public virtual DbSet<clientes_notas> clientes_notas { get; set; }
		public virtual DbSet<fichas_medicas> fichas_medicas { get; set; }
		public virtual DbSet<clientes_dietas> clientes_dietas { get; set; }
		public virtual DbSet<nutricionistas> nutricionistas { get; set; }
		public virtual DbSet<permisos> permisos { get; set; }
		public virtual DbSet<programas> programas { get; set; }
		public virtual DbSet<usuario_permisos> usuario_permisos { get; set; }
		public virtual DbSet<usuarios> usuarios { get; set; }
		public virtual DbSet<vendedores> vendedores { get; set; }
		public virtual DbSet<horarios> horarios { get; set; }
		public virtual DbSet<locales> locales { get; set; }
		public virtual DbSet<programa_clientes> programa_clientes { get; set; }
		public virtual DbSet<prospectos> prospectos { get; set; }
		public virtual DbSet<salas> salas { get; set; }
		public virtual DbSet<solicitud_permisos> solicitud_permisos { get; set; }
		public virtual DbSet<semanas_precios> semanas_precios { get; set; }
		public virtual DbSet<venta_usuarios> venta_usuarios { get; set; }
		public virtual DbSet<congelamiento_semanas> congelamiento_semanas { get; set; }
		public virtual DbSet<pesos_medidas> pesos_medidas { get; set; }
		public virtual DbSet<CardioInfo> CardioInfo { get; set; }
		public virtual DbSet<ControlesNutricionales> ControlesNutricionales { get; set; }
		public virtual DbSet<FeedingRegime> FeedingRegime { get; set; }
		public virtual DbSet<correos> correos { get; set; }
		public virtual DbSet<correos_reglas> correos_reglas { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			modelBuilder.Entity<clientes>()
				.HasMany(e => e.citas)
				.WithRequired(e => e.clientes)
				.HasForeignKey(e => e.cliente_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<nutricionistas>()
				.HasMany(e => e.citas)
				.WithRequired(e => e.nutricionistas)
				.HasForeignKey(e => e.nutricionista_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<permisos>()
				.HasMany(e => e.usuario_permisos)
				.WithRequired(e => e.permisos)
				.HasForeignKey(e => e.permiso_id);

			modelBuilder.Entity<programas>()
				.HasMany(e => e.citas)
				.WithRequired(e => e.programas)
				.HasForeignKey(e => e.programa_id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<usuarios>()
				.Property(e => e.tipo);

			modelBuilder.Entity<usuarios>()
				.HasOptional(e => e.nutricionistas)
				.WithRequired(e => e.usuarios)
				.WillCascadeOnDelete();

			modelBuilder.Entity<usuarios>()
				.HasMany(e => e.usuario_permisos)
				.WithRequired(e => e.usuarios)
				.HasForeignKey(e => e.usuario_id);

			modelBuilder.Entity<usuarios>()
				.HasOptional(e => e.vendedores)
				.WithRequired(e => e.usuarios)
				.WillCascadeOnDelete();

			modelBuilder.Entity<solicitud_permisos>()
				.HasRequired(e => e.jefe)
				.WithMany()
				.HasForeignKey(e => e.usuario_jefe)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<solicitud_permisos>()
				.HasRequired(e => e.solicitante)
				.WithMany()
				.HasForeignKey(e => e.usuario_solicitante)
				.WillCascadeOnDelete(false);
		}

		public System.Data.Entity.DbSet<SEPAMIAR_SGC.Models.LabResults> LabResults { get; set; }
	}
}
