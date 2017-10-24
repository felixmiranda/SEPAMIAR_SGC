namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class permisos
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public permisos()
		{
			usuario_permisos = new HashSet<usuario_permisos>();
		}

		public int id { get; set; }

		[Required]
		[StringLength(255)]
		[Display(Name = "Nombre")]
		public string nombre { get; set; }

		[Required]
		[StringLength(20)]
		[Display(Name = "Código")]
		public string codigo_interno { get; set; }

		[Display(Name = "F. Creación")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime created_at { get; set; }

		[Display(Name = "F. Edición")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime updated_at { get; set; }

		public DateTime? deleted_at { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<usuario_permisos> usuario_permisos { get; set; }
	}
}
