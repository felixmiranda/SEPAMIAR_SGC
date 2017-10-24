namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class nutricionistas
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public nutricionistas()
		{
			citas = new HashSet<citas>();
		}

		[Key]
		[Display(Name = "Código")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int usuario_id { get; set; }

		[Display(Name = "Es Jefe?")]
		public bool? jefe { get; set; }

		public DateTime created_at { get; set; }

		public DateTime updated_at { get; set; }

		public DateTime? deleted_at { get; set; }

		[Display(Name = "Está activo?")]
		public bool activo { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<citas> citas { get; set; }

		public virtual usuarios usuarios { get; set; }
	}
}
