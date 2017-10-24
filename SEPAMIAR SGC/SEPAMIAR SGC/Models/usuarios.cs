namespace SEPAMIAR_SGC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usuarios()
        {
			usuario_permisos = new HashSet<usuario_permisos>();
        }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name = "ID")]
		public int id { get; set; }

        [Required]
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

        [Required]
        [StringLength(255)]
		[Display(Name = "Contraseña")]
		public string password { get; set; }

		[Display(Name = "Foto")]
		public string foto { get; set; }

        [Required]
        [EnumDataType(typeof(UserProfiles))]
		[Display(Name = "Tipo")]
		public UserProfiles tipo { get; set; }

		[Display(Name = "Local")]
		public int? localesId { get; set; }

		public bool activo { get; set; }

		[Display(Name = "F. Creación")]
		[DataType(DataType.DateTime)]
		public DateTime created_at { get; set; }

		[Display(Name = "F. Edición")]
		[DataType(DataType.DateTime)]
		public DateTime updated_at { get; set; }

		[Display(Name = "F. Eliminación")]
		public DateTime? deleted_at { get; set; }

        public virtual nutricionistas nutricionistas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usuario_permisos> usuario_permisos { get; set; }

        public virtual vendedores vendedores { get; set; }

		[ForeignKey("localesId")]
		public virtual locales locales { get; set; }

		//Attributes

		public string getFoto()
		{
			return string.IsNullOrEmpty(this.foto) ? "/Images/user-default-photo.png" : "data:image/png;base64," + this.foto;
		}
    }

	public enum UserProfiles
	{
		Default,
		Nutricionista,
		Vendedor,
		Entrenador,
		Cajero,
		Sistemas,
		Pesos_y_Medidas,
		Cardiología,
		Marketing
	}
}
