namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;
	using SEPAMIAR_SGC.Utilities;

	public class correos
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string NombreMail { get; set; }

		public string To { get; set; }

		public string Asunto { get; set; }

		public string Contenido { get; set; }

		public DateTime CreatedAt { get; set; }

		public int CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public int UpdatedBy { get; set; }
	}
	
}
