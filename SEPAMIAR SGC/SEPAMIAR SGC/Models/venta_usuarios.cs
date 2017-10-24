namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public class venta_usuarios
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Display(Name = "ID")]
		public int id { get; set; }

		public int local_id { get; set; }

		public int cliente_id { get; set; }

		public int vendedor_id { get; set; }

		public DateTime fecha_inicio { get; set; }

		public DateTime fecha_fin { get; set; }
		
		[Display(Name ="Semanas Contratadas")]
		public int semanas_precio_id { get; set; }

		public int cantidad_dias_congelamiento { get; set; }

		public bool solicitud_permiso_congelamiento { get; set; }

		[Display(Name ="Tipo de Ingreso")]
		public TipoIngreso tipo_ingreso { get; set; }

		public int? campana_marketing_id { get; set; }

		[StringLength(20)]
		public string numero_boleta { get; set; }

		[Display(Name = "Tipo de Pago")]
		public TipoPago tipo_pago { get; set; }

		[DataType(DataType.Currency)]
		[Display(Name = "Monto")]
		[DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:C}")]
		public double monto { get; set; }

		[Display(Name = "Atendido")]
		public bool atendido { get; set; }

		[Display(Name = "F. Creación")]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
		public DateTime created_at { get; set; }

		[Display(Name = "F. Edición")]
		public DateTime updated_at { get; set; }

		[Display(Name = "F. Eliminación")]
		public DateTime? deleted_at { get; set; }

		[ForeignKey("cliente_id")]
		public virtual clientes cliente { get; set; }

		[ForeignKey("local_id")]
		public virtual locales local { get; set; }
	}

	public enum TipoIngreso
	{
		RW, RS, INC
	}

	public enum TipoPago
	{
		Efectivo, Tarjeta, Depósito, Delivery
	}
}