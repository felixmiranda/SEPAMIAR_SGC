namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class clientes_dietas
	{
		[Key]
		[Display(Name = "ID")]
		public int id { get; set; }

		[Display(Name = "Programa ID")]
		public int programa_clientes_id { get; set; }

		public List<DailyDiet> dieta_diaria { get; set; }

		[ForeignKey("programa_clientes_id")]
		public programa_clientes programa_clientes { get; set; }

		[Display(Name = "Creado")]
		public DateTime created_at { get; set; }

		[Display(Name = "Actualizado")]
		public DateTime updated_at { get; set; }

	}

	public class DailyDiet
	{
		public int id { get; set; }

		[Range(1,7)]
		[Display(Name = "Dia Semana")]
		public int dia { get; set; }

		[Display(Name = "Tipo de Comida")]
		public FoodTypes tipo_comida { get; set; }

		[Display(Name = "Texto")]
		public string texto { get; set; }

		[Range(0,3)]
		[Display(Name ="Opción")]
		public int opcion { get; set; }

		public int clientes_dietas_id { get; set; }

		[ForeignKey("clientes_dietas_id")]
		public virtual clientes_dietas clientes_dietas { get; set; }
	}
}
