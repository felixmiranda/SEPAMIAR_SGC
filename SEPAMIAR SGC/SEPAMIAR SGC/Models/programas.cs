namespace SEPAMIAR_SGC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class programas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public programas()
        {
            citas = new HashSet<citas>();
            fichas_medicas = new HashSet<fichas_medicas>();
        }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

        [Required]
        [StringLength(255)]
		[Display(Name = "Nombre")]
        public string nombre { get; set; }

		[Required]
		[Display(Name = "Semanas")]
		public int semanas { get; set; }

		[Display(Name = "F. Creaci�n")]
		public DateTime created_at { get; set; }

		[Display(Name = "F. Edici�n")]
		public DateTime updated_at { get; set; }

		[Display(Name = "F. Eliminaci�n")]
		public DateTime? deleted_at { get; set; }

		[Display(Name ="App Frase")]
		[StringLength(100,ErrorMessage ="Excede el m�ximo de caract�res")]
		public string frase { get; set; }
		
		[Display(Name ="App Imagen Principal")]
		public string imagen { get; set; }

		[Display(Name ="App Descripci�n")]
		public string descripcion { get; set; }

		[Display(Name = "App Fuerza")]
		[Range(0,5)]
		public int fuerza { get; set; }

		[Display(Name = "App Resistencia")]
		[Range(0, 5)]
		public int resistencia { get; set; }

		[Display(Name = "App Intensidad")]
		[Range(0, 5)]
		public int intensidad { get; set; }

		[Display(Name = "App Icono")]
		public string icono { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<citas> citas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fichas_medicas> fichas_medicas { get; set; }
    }
}
