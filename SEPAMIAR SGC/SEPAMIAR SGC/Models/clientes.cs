namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class clientes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public clientes()
        {
            citas = new HashSet<citas>();
            fichas_medicas = new HashSet<fichas_medicas>();
        }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id_alt { get; set; }

		[StringLength(255)]
		[Display(Name = "Código")]
		public string codigo { get; set; }

		[Required]
		[StringLength(255)]
		[Display(Name = "Nombres")]
		public string nombres { get; set; }

		[Required]
		[StringLength(255)]
		[Display(Name = "Apellidos")]
		public string apellidos { get; set; }

		[Required]
		[StringLength(255)]
		[Display(Name = "E-mail")]
		public string email { get; set; }

		[Display(Name = "Contraseña")]
		[Range(1000, 9999)]
		public int password { get; set; }

		[Display(Name = "Foto frontal del cliente")]
		[Column(TypeName = "image")]
		public byte[] foto { get; set; }

		[Required]
		[Display(Name = "Centro Laboral")]
		public string centro_laboral { get; set; }

		[Required]
		[Display(Name = "Cargo")]
		public string cargo_laboral { get; set; }

		[Required]
		[Display(Name = "E-mail Empresa")]
		public string email_empresa { get; set; }

        [Column(TypeName = "date")]
		[Display(Name = "Fecha de Nacimiento")]
		public DateTime fecha_nacimiento { get; set; }

        [Required]
		[Display(Name = "Genero")]
        public genero genero { get; set; }

        [Required]
		[Display(Name = "Tipo de Documento")]
		public documento_tipo documento_tipo { get; set; }

        [Required]
        [StringLength(11)]
		[Display(Name = "Nro Documento")]
		public string documento_numero { get; set; }

        [StringLength(255)]
		[Display(Name = "Dirección")]
		public string direccion { get; set; }

        [StringLength(255)]
		[Display(Name = "Distrito")]
		public string distrito { get; set; }

        [StringLength(25)]
		[Display(Name = "Teléfono")]
		public string telefono { get; set; }

        [StringLength(25)]
		[Display(Name = "Celular")]
		public string celular { get; set; }

		[Display(Name = "Nombres")]
		public string ce_nombres { get; set; }

		[Display(Name = "Apellidos")]
		public string ce_apellidos { get; set; }

		[Display(Name = "Teléfono")]
		public string ce_telefono { get; set; }

		[Display(Name = "Celular")]
		public string ce_celular { get; set; }

		[Display(Name = "E-mail")]
		public string ce_email { get; set; }

		[Display(Name = "¿Cómo se enteró de Personal Training?")]
		public fuente como_se_entero { get; set; }

		[Display(Name = "Activo")]
		public bool activo { get; set; }

		[Display(Name = "Acceso Autorizado")]
		public bool acceso_autorizado { get; set; }

		[Display(Name ="Nutricionista Asignado")]
		public int? nutricionista_asignado { get; set; }

		[Display(Name = "F. Creación")]
		public DateTime created_at { get; set; }

		[Display(Name = "F. Edición")]
		public DateTime updated_at { get; set; }

		[Display(Name = "F. Eliminación")]
		public DateTime? deleted_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<citas> citas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fichas_medicas> fichas_medicas { get; set; }

		[ForeignKey("nutricionista_asignado")]
		public nutricionistas nutricionistas { get; set; }

		public List<string> GetUserMails()
		{
			List<string> oLst = new List<string>();
			oLst.Add(email);
			oLst.Add(email_empresa);

			return oLst;			 
		}

		
	}

	public enum fuente
	{
		Amistades, PaséPorElLocal, RedesSociales, Corporativos, Medios, Eventos, Otros
	}

	public enum genero
	{
		Masculino, Femenino
	}

	public enum documento_tipo
	{
		DNI, CE
	}
}
