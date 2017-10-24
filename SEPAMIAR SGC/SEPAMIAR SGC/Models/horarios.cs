namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class horarios
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		[Display(Name = "Sala")]
		public int sala_id { get; set; }

		[Display(Name = "Programa")]
		public int programa_id { get; set; }

		[Display(Name ="Lun")]
		public bool lunes { get; set; }

		[Display(Name = "Mar")]
		public bool martes { get; set; }

		[Display(Name = "Mie")]
		public bool miercoles { get; set; }

		[Display(Name = "Jue")]
		public bool jueves { get; set; }

		[Display(Name = "Vie")]
		public bool viernes { get; set; }

		[Display(Name = "Sab")]
		public bool sabado { get; set; }

		[Display(Name = "Dom")]
		public bool domingo { get; set; }

		[Display(Name = "Hora")]
		public TimeSpan hora { get; set; }

		[Display(Name = "F. Creación")]
		public DateTime created_at { get; set; }

		[Display(Name = "F. Edición")]
		public DateTime updated_at { get; set; }

		[Display(Name = "F. Eliminación")]
		public DateTime? deleted_at { get; set; }

		[ForeignKey("sala_id")]
		public virtual salas sala { get; set; }

		[ForeignKey("programa_id")]
		public virtual programas programa { get; set; }
	}
}