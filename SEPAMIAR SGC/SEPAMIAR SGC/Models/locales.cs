namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class locales
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]	
		public int id { get; set; }

		[Display(Name = "Nombre")]
		public string nombre { get; set; }

		[Display(Name = "Distrito")]
		public string distrito { get; set; }

		[Display(Name = "F. Creación")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime created_at { get; set; }

		[Display(Name = "F. Edición")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime updated_at { get; set; }

		[Display(Name = "F. Eliminación")]
		public DateTime? deleted_at { get; set; }
	}
}