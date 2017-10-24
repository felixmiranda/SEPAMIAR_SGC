namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class programa_clientes
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name = "ID")]
		public int id { get; set; }

		[Display(Name = "Programa")]
		public int programa_id { get; set; }

		public int cliente_id { get; set; }

		[Display(Name = "Horario")]
		public int horario_id { get; set; }

		[Display(Name = "Fecha de inicio")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime fecha_inicio { get; set; }

		[Display(Name = "Fecha de fin")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime fecha_fin { get; set; }

		public DateTime created_at { get; set; }

		public DateTime updated_at { get; set; }

		public DateTime? deleted_at { get; set; }

		public List<clientes_dietas> dietas { get; set; }

		[ForeignKey("programa_id")]
		public virtual programas programa { get; set; }

		[ForeignKey("cliente_id")]
		public virtual clientes cliente { get; set; }

		[ForeignKey("horario_id")]
		public virtual horarios horario { get; set; }
	}
}