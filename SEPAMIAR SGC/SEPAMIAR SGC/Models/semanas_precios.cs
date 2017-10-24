namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class semanas_precios
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		[Display(Name = "Cantidad de Semanas")]
		public int cantidad_semanas { get; set; }

		[Display(Name = "Dias de Congelamiento")]
		public int dias_congelamiento { get; set; }

		[Display(Name = "Precio")]
		[DataType(DataType.Currency)]
		public double precio { get; set; }

		public int localesId { get; set; }

		public DateTime created_at { get; set; }

		public DateTime updated_at { get; set; }

		public DateTime? deleted_at { get; set; }

		[ForeignKey("localesId")]
		public locales locales { get; set; }
	}
}
