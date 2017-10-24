namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;
	using SEPAMIAR_SGC.Utilities;

	public class correos_reglas
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Nombre { get; set; }

		[EnumDataType(typeof(conditionalOperators))]
		public conditionalOperators Operador { get; set; }

		[Required]
		[StringLength(maximumLength: 1000, ErrorMessage = "No se registró la regla condicional.", MinimumLength =1)]
		public string Condicion { get; set; }

		[Required]
		public string Tabla { get; set; }

		[Required]
		public bool EsRegla { get; set; }
	}

	public enum conditionalOperators
	{
		AND,
		OR
	}
}
