namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class solicitud_permisos
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name = "ID")]
		public int id { get; set; }
		[Required]
		public int usuario_jefe { get; set; }
		[Required]
		public int usuario_solicitante { get; set; }
		public string modelo_nombre { get; set; }
		public int modelo_id { get; set; }
		public object valor_defecto { get; set; }
		public object valor_modificado { get; set; }
		public bool autorizado { get; set; }

		[Display(Name ="F.Solicitud")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime created_at { get; set; }

		[Display(Name = "F.Aprobación")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime updated_at { get; set; }

		public DateTime? deleted_at { get; set; }

		[ForeignKey("usuario_jefe")]
		public virtual usuarios jefe { get; set; }

		[ForeignKey("usuario_solicitante")]
		public virtual usuarios solicitante { get; set; }
	}
}