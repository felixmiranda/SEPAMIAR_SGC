namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;
	
	public partial class clientes_notas
	{
		public int id { get; set; }
		public int idCliente { get; set; }
		public string nota { get; set; }
		public int usuarioId { get; set; }

		[ForeignKey("idCliente")]
		public virtual clientes clientes { get; set; }

		[ForeignKey("usuarioId")]
		public virtual usuarios usuarios { get; set; }

		[Display(Name = "F. Creación")]
		public DateTime created_at { get; set; }

		[Display(Name = "F. Edición")]
		public DateTime updated_at { get; set; }

		[Display(Name = "F. Eliminación")]
		public DateTime? deleted_at { get; set; }
	}
}
