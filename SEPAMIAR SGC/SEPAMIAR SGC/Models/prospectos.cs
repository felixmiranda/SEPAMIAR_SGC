namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class prospectos
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		[Required]
		[Display(Name = "Nombres")]
		public string nombres { get; set; }

		[Required]
		[Display(Name = "Apellidos")]
		public string apellidos { get; set; }

		[Display(Name = "Genero")]
		public genero genero { get; set; }

		[Column(TypeName = "date")]
		[Display(Name ="Fecha de Nacimiento")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime fecha_nacimiento { get; set; }

		[Display(Name = "Tipo de Documento")]
		public documento_tipo documento_tipo { get; set; }

		[StringLength(10)]
		[Display(Name = "Nro de Documento")]
		public string documento_numero { get; set; }

		[Required]
		[Display(Name = "E-mail Personal")]
		public string email_personal { get; set; }

		[Display(Name = "Centro Laboral")]
		public string centros_laboral { get; set; }

		[Display(Name = "Cargo Laboral")]
		public string cargo_laboral { get; set; }

		[Display(Name = "E-mail Empresa")]
		public string email_empresa { get; set; }

		[Display(Name = "Teléfono")]
		public string telefono { get; set; }

		[Display(Name = "Fecha Registro")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime created_at { get; set; }

		public DateTime updated_at { get; set; }

		public DateTime? deleted_at { get; set; }
	}
}