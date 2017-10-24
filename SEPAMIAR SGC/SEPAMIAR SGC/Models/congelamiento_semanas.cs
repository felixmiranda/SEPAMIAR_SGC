namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class congelamiento_semanas
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		[Display(Name ="Semanas desde")]
		public int desde { get; set; }

		[Display(Name ="Hasta (d)")]
		public int hasta { get; set; }

		[Display(Name ="Dias Cong.")]
		public int cantidad_dias { get; set; }

		public DateTime created_at { get; set; }

		public DateTime updated_at { get; set; }

		public DateTime? deleted_at { get; set; }
	}
}