namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class citas
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		public int cliente_id { get; set; }

		public int nutricionista_id { get; set; }

		public int programa_id { get; set; }

		[Column(TypeName = "date")]
		public DateTime fecha { get; set; }

		public TimeSpan hora { get; set; }

		public string foto_avance { get; set; }

		[Required]
		[EnumDataType(typeof(AppointmentsTypes))]
		[Display(Name = "Tipo de Cita")]
		public AppointmentsTypes tipo { get; set; }

		[StringLength(200)]
		[Display(Name = "Motivo Reprogramación")]
		public string motivo_reprogramacion { get; set; }

		public DateTime created_at { get; set; }

		public DateTime updated_at { get; set; }

		public DateTime? deleted_at { get; set; }

		[ForeignKey("cliente_id")]
		public virtual clientes clientes { get; set; }

		[ForeignKey("nutricionista_id")]
		public virtual nutricionistas nutricionistas { get; set; }

		public virtual programas programas { get; set; }
	}
}

public enum AppointmentsTypes
{
	CI,
	CF,
	CFI
}