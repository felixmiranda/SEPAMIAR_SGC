namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class clientes_congelamientos
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		public int clienteId { get; set; }

		public DateTime fecha_desde { get; set; }

		public DateTime fecha_hasta { get; set; }

		public int dias_congelados { get; set; }

		[ForeignKey("clienteId")]
		public clientes clientes { get; set; }
	}
}
