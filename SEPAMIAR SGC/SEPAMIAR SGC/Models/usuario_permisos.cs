namespace SEPAMIAR_SGC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class usuario_permisos
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name = "ID")]
		public int id { get; set; }

		[Display(Name ="Usuario")]
		public int usuario_id { get; set; }

		[Display(Name ="Permiso")]
		public int permiso_id { get; set; }

		[Display(Name = "Fecha Asignación")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime created_at { get; set; }

		[Display(Name = "Fecha Actualización")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime updated_at { get; set; }

        public DateTime? deleted_at { get; set; }

		[ForeignKey("permiso_id")]
		public virtual permisos permisos { get; set; }

		[ForeignKey("usuario_id")]
		public virtual usuarios usuarios { get; set; }
    }
}
