namespace SEPAMIAR_SGC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vendedores
    {
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

		public virtual usuarios usuarios { get; set; }
    }
}
