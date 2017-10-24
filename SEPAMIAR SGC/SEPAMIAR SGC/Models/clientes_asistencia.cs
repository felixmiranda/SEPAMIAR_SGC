namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class clientes_asistencia
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		public int clienteId { get; set; }

		public DateTimeOffset fecha { get; set; }

		[ForeignKey("clienteId")]
		public clientes clientes { get; set; }
	}
}
