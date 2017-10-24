namespace SEPAMIAR_SGC.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class fichas_medicas
	{
		public fichas_medicas()
		{
			cardio = new List<CardioInfo>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }

		public int programa_id { get; set; }

		public int cliente_id { get; set; }

		[Display(Name = "Estatura")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double estatura { get; set; }

		[Display(Name = "Peso Actual")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double peso_actual { get; set; }

		[Display(Name = "Peso Deseado")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double peso_deseado { get; set; }

		[Display(Name = "Peso Ideal")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double peso_ideal { get; set; }

		[Display(Name = "Contextura")]
		[EnumDataType(typeof(Contextures))]
		public Contextures? contextura { get; set; }

		public ICollection<CardioInfo> cardio { get; set; }

		public List<ControlesNutricionales> controles { get; set; }

		public List<LabResults> laboratorio { get; set; }

		[EnumDataType(typeof(Frecuencies))]
		public Frecuencies frec_fumador { get; set; }

		[EnumDataType(typeof(Frecuencies))]
		public Frecuencies frec_bebedor { get; set; }

		[Display(Name = "Cortisona")]
		public bool cortisona { get; set; }

		[Display(Name = "Esteroides Anabólicos")]
		public bool esteroidesAnabolicos { get; set; }

		[Display(Name = "Anfetaminas")]
		public bool anfetaminas { get; set; }

		[Display(Name = "Mariahuana")]
		public bool marihuana { get; set; }

		[Display(Name = "Otra")]
		public bool otra { get; set; }

		[Display(Name = "Ninguna")]
		public bool ninguna { get; set; }

		public WomenMenstruationInfo menstruacion_menopausia { get; set; }

		public Background antecedentes { get; set; }

		public TraumatologyProblems problemas_traumatologicos { get; set; }

		public RespiratoryProblems problemas_respiratorios { get; set; }

		public GeneralBackground antecedentas_generales { get; set; }

		public NutritionalAnamnesis anamnesis_nutricional { get; set; }

		public List<FeedingRegime> regimen_alimentacion { get; set; }

		public DateTime created_at { get; set; }

		public int created_by { get; set; }

		public DateTime updated_at { get; set; }

		public int updated_by { get; set; }

		[ForeignKey("programa_id")]
		public programas programas { get; set; }

		[ForeignKey("cliente_id")]
		public clientes clientes { get; set; }

		[ForeignKey("created_by")]
		public usuarios usuario_creador { get; set; }

		[ForeignKey("updated_by")]
		public usuarios usuario_actualizador { get; set; }
	}

	public class FeedingRegime
	{
		public int id { get; set; }

		[Display(Name = "Tipo de Comida")]
		[EnumDataType(typeof(FoodTypes))]
		public FoodTypes tipo_comida { get; set; }

		[Display(Name = "Hora")]
		public TimeSpan hora { get; set; }

		[Display(Name = "Detalle")]
		public string detalles { get; set; }
	}

	public class NutritionalAnamnesis
	{

		[Display(Name = "Come fuera de casa?")]
		public bool alimentos_fuera_de_casa { get; set; }

		[Display(Name = "Frecuencia")]
		public Frecuencies frecAlimentosFuera { get; set; }

		[Display(Name = "Tipo de Comida")]
		public string tipo_alimentos_fuera { get; set; }

		[Display(Name = "Vasos de agua diarios")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public int cantidad_vasos_agua_dia { get; set; }

		[Display(Name = "Alimentos preferidos")]
		public string alimentos_preferencia { get; set; }

		[Display(Name = "Alimentos no preferidos")]
		public string alimentos_no_preferencia { get; set; }

		[Display(Name = "Alimentos dañinos")]
		public string alimentos_daninos { get; set; }
	}

	public class GeneralBackground
	{
		[Display(Name = "Obesidad")]
		public bool obesidad { get; set; }

		[Display(Name = "Hipertensión")]
		public bool hipertension { get; set; }

		[Display(Name = "Diabetes")]
		public bool diabetes { get; set; }

		[Display(Name = "Cardiopatías")]
		public bool cardiopatias { get; set; }

		[Display(Name = "No Mencionada")]
		public bool no_mencionada { get; set; }
	}

	public class RespiratoryProblems
	{
		[Display(Name = "Asma Crónica")]
		public bool asma_cronica { get; set; }

		[Display(Name = "Tos Crónica")]
		public bool tos_cronica { get; set; }

		[Display(Name = "Resfriados Continuos")]
		public bool resfriados_continuos { get; set; }

		[Display(Name = "Bronquitis")]
		public bool bronquitis { get; set; }

		[Display(Name = "Rinitis Alérgica")]
		public bool rinitis_alergica { get; set; }

		[Display(Name = "Sinusitis")]
		public bool sinusitis { get; set; }

		[Display(Name = "Amigdalitis")]
		public bool amigdalitis { get; set; }

		[Display(Name = "Otros")]
		public bool otros { get; set; }
	}

	public class TraumatologyProblems
	{
		[Display(Name = "Dolores Articulares (Artritis, Artrosis)")]
		public bool dolores_articulares { get; set; }

		[Display(Name = "Osteoporosis")]
		public bool osteoporosis { get; set; }

		[Display(Name = "Problemas de Columna")]
		public bool problemas_columna { get; set; }

		public string problemas_columna_texto { get; set; }

		[Display(Name = "Otras lesiones importantes")]
		public bool otras_lesiones { get; set; }

		public string otras_lesiones_texto { get; set; }
	}

	public class Background
	{
		[Display(Name = "Gástricos")]
		public bool gastricos { get; set; }

		[Display(Name = "Diabetes")]
		public bool diabetes { get; set; }

		[Display(Name = "Triglicéridos Altos")]
		public bool trigliceridos_altos { get; set; }

		[Display(Name = "Hipertiroidismo")]
		public bool hipertiroidismo { get; set; }

		[Display(Name = "Hipotiroidismo")]
		public bool hipotiroidismo { get; set; }

		[Display(Name = "Quistes")]
		public bool quistes { get; set; }

		[Display(Name = "Tumores")]
		public bool tumores { get; set; }

		[Display(Name = "Resistencia Insulina")]
		public bool resistencia_insulina { get; set; }

		[Display(Name = "Otras Dislipidemias")]
		public bool otras_dislipidemias { get; set; }

		[Display(Name = "Prostata")]
		public bool prostata { get; set; }

		[Display(Name = "Renales")]
		public bool renales { get; set; }

		[Display(Name = "Hepáticos")]
		public bool hepaticos { get; set; }

		[Display(Name = "Presión Alta")]
		public bool presion_alta { get; set; }

		[Display(Name = "Presión Baja")]
		public bool presion_baja { get; set; }

		[Display(Name = "Colesterol Alto")]
		public bool colesterol_alto { get; set; }

		[Display(Name = "Anemia")]
		public bool anemia { get; set; }

		[Display(Name = "Hernias")]
		public bool hernias { get; set; }

		[Display(Name = "Estreñimiento")]
		public bool estrenimiento { get; set; }

		[Display(Name = "No Mencionada")]
		public bool no_mencionada { get; set; }
	}

	public class WomenMenstruationInfo
	{
		[Display(Name = "Cantidad hijos")]
		public int? cantidad_hijos { get; set; }

		[Display(Name = "Tipo de parto")]
		[EnumDataType(typeof(BirthTypes))]
		public BirthTypes? tipo_parto { get; set; }

		[Display(Name = "Edad Menopausia")]
		public int? edad_menopausia { get; set; }

		[Display(Name = "Menopausia en Proceso")]
		public bool? menopausia_activa { get; set; }

		[Display(Name = "Tratamiento Hormonal")]
		public string tratamiento_hormonal { get; set; }

	}

	public class CardioInfo
	{
		public int id { get; set; }

		[Display(Name = "Presión Arterial Sistólica")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double presion_arterial_sistolica { get; set; }

		[Display(Name = "Presión Arterial Diastólica")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double presion_arterial_diastolica { get; set; }

		[Display(Name = "Resultado")]
		[EnumDataType(typeof(CardioSuittable))]
		public CardioSuittable cardiologia_aprobacion { get; set; }

		[Display(Name = "Último examen vigente")]
		public DateTime fecha { get; set; }

		public virtual fichas_medicas fichas_medicas { get; set; }
	}

	public class ControlesNutricionales
	{
		public int id { get; set; }

		[Display(Name = "Fecha")]
		public DateTime fecha { get; set; }

		[Display(Name = "ACT")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double act { get; set; }

		[Display(Name = "MME (Kg)")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double mme { get; set; }

		[Display(Name = "MGC (Kg)")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double mgc { get; set; }

		[Display(Name = "IMC (Kg/m2)")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double mc { get; set; }

		[Display(Name = "PGC (%)")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double pgc { get; set; }

		[Display(Name = "RCC")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double rcc { get; set; }

		[Display(Name = "Peso")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double peso { get; set; }

		public DateTime created_at { get; set; }

		public DateTime updated_at { get; set; }
	}

	public class LabResults
	{
		public int id { get; set; }

		[Display(Name = "Hemoglobina")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double hemoglobina { get; set; }

		[Display(Name = "Colesterol")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double colesterol { get; set; }

		[Display(Name = "Creatinina")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double creatinina { get; set; }

		[Display(Name = "Glucosa Basal")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double glucosa_basal { get; set; }

		[Display(Name = "Triglicéridos")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
		public double trigliceridos { get; set; }

		[Display(Name = "Último examen vigente")]
		public DateTime fecha { get; set; }
	}

	public enum FoodTypes
	{
		Desayuno, MediaMañana, Almuerzo, MediaTarde, Cena
	}

	public enum BirthTypes
	{
		Natural,
		Cesarea
	}

	public enum CardioSuittable
	{
		Apto,
		NoApto
	}

	public enum Contextures
	{
		Delgado,
	}

	public enum Frecuencies
	{
		Nunca,
		Casi_nunca,
		Fines_de_Semana,
		Algunas_veces_por_semana,
		Casi_a_diario,
		Diario
	}
}
