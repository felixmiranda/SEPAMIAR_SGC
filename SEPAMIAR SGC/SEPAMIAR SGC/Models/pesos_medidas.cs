namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class pesos_medidas
	{
		public pesos_medidas()
		{
			peso_2 = 0.00;
			peso_3 = 0.00;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		[Display(Name = "Cliente Id")]
		public int clienteId { get; set; }

		[Display(Name ="Peso Lunes")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double peso_1 { get; set; }

		[Display(Name = "Peso Miercoles")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? peso_2 { get; set; }

		[Display(Name = "Peso Viernes")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? peso_3 { get; set; }

		[Display(Name = "% de Grasa")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:00%}")]
		public double? porc_grasa_corporal { get; set; }

		[Display(Name = "Medidas")]
		public Medidas medidas { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTimeOffset created_at { get; set; }

		public int created_by { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTimeOffset updated_at { get; set; }

		public int updated_by { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTimeOffset? deleted_at { get; set; }

		public int? deleted_by { get; set; }

		[ForeignKey("clienteId")]
		public clientes clientes { get; set; }
	}

	public class Medidas
	{
		public Medidas()
		{
			cuello = 0.00;
			hombros = 0.00;
			torax = 0.00;
			biceps = 0.00;
			muneca = 0.00;
			cintura = 0.00;
			gluteos = 0.00;
			muslo = 0.00;
			pantorrilla = 0.00;
		}

		[Display(Name = "Cuello")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? cuello { get; set; }

		[Display(Name = "Hombros")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? hombros { get; set; }

		[Display(Name = "Tórax (N)")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? torax { get; set; }

		[Display(Name = "Bíceps")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? biceps { get; set; }

		[Display(Name = "Muñeca")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? muneca { get; set; }

		[Display(Name = "Cintura")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? cintura { get; set; }

		[Display(Name = "Glúteos")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? gluteos { get; set; }

		[Display(Name = "Muslo (A)")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? muslo { get; set; }

		[Display(Name = "Pantorrilla")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
		public double? pantorrilla { get; set; }
	}
}
