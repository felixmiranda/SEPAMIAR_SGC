namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class campana_marketing
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string nombre { get; set; }
		public bool activo { get; set; }
		public double dscto { get; set; }
		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
		public DateTime? deleted_at { get; set; }
	}
}