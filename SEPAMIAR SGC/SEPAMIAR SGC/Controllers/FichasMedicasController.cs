using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using SEPAMIAR_SGC.Models;
using SEPAMIAR_SGC.Utilities;
using SEPAMIAR_SGC.Seguridad;

namespace SEPAMIAR_SGC.Controllers
{
	public class FichasMedicasController : Controller
	{
		private _SGCModel db = new _SGCModel();
		int controlsCount = 0;
		int foodRegimesCount = 0;

		// GET: FichasMedicas
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("mhist"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			var fichas_medicas = db.fichas_medicas.Include(f => f.clientes).Include(f => f.programas).Include(f => f.usuario_creador);
			return View(await fichas_medicas.ToListAsync());
		}

		public ActionResult SearchClientMedicalHistory(int Value)
		{
			string clientCode = Value.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			if (c > 0)
			{
				return RedirectToAction("Create", "FichasMedicas", new { clienteId = c });
			}
			else
			{
				return RedirectToAction("Index", "FichasMedicas");
			}
		}

		// GET: FichasMedicas/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			fichas_medicas fichas_medicas = await db.fichas_medicas.FindAsync(id);
			if (fichas_medicas == null)
			{
				return HttpNotFound();
			}
			return View(fichas_medicas);
		}

		// GET: FichasMedicas/Create
		public ActionResult Create(int clienteId)
		{
			clientes cl = db.clientes.Where(c => c.id_alt == clienteId).FirstOrDefault();
			DateTime today = CurrentDate.getNow();
			programa_clientes prgCl = db.programa_clientes.Where(m => m.cliente_id == cl.id_alt && m.fecha_inicio <= today && m.fecha_fin >= today).FirstOrDefault();

			fichas_medicas fm = db.fichas_medicas.Where(m => m.cliente_id == cl.id_alt).Include(m => m.controles).Include(m => m.regimen_alimentacion).Include(m => m.cardio).Include(m => m.laboratorio).OrderByDescending(m => m.created_at).FirstOrDefault();

			ViewBag.currentCardioControl = null;
			if (fm != null && fm.cardio.Count > 0)
			{
				CardioInfo lastCardioInfoInserted = fm.cardio.OrderByDescending(m => m.fecha).First();

				TimeSpan dateDif = CurrentDate.getNow().Subtract(lastCardioInfoInserted.fecha);

				if (dateDif <= new TimeSpan(180, 0, 0, 0))
				{
					ViewBag.currentCardioControl = lastCardioInfoInserted;
				}
			}

			ViewBag.currentLabControl = null;
			if (fm != null && fm.laboratorio.Count > 0)
			{
				LabResults lastLabInfoInserted = fm.laboratorio.OrderByDescending(m => m.fecha).First();

				TimeSpan dateDif = CurrentDate.getNow().Subtract(lastLabInfoInserted.fecha);

				if (dateDif <= new TimeSpan(180, 0, 0, 0))
				{
					ViewBag.currentLabControl = lastLabInfoInserted;
				}
			}

			if (fm != null && fm.controles.Count > 0)
			{
				List<ControlesNutricionales> cn = fm.controles;
				List<string> cnRows = new List<string>();
				foreach (ControlesNutricionales c in cn)
				{
					string row = GetNutritionalControlString(c: c);
					cnRows.Add(row);
					controlsCount++;
				}
				controlsCount = 0;
				ViewBag.controlsLst = cnRows;
			}

			if (fm != null && fm.regimen_alimentacion.Count > 0)
			{
				List<FeedingRegime> fr = fm.regimen_alimentacion;
				List<string> frList = new List<string>();
				foreach (FeedingRegime f in fr)
				{
					string row = GetFoodRegimeString(f: f);
					frList.Add(row);
					foodRegimesCount++;
				}
				foodRegimesCount = 0;
				ViewBag.regimenLst = frList;
			}

			ViewBag.clientId = cl.id_alt;
			ViewBag.programId = prgCl.programa_id;

			return View();
		}

		// POST: FichasMedicas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<string> Create(string json)
		{
			Dictionary<string, object> oDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			fichas_medicas oFM = new fichas_medicas();

			// C_1
			oFM.programa_id = int.Parse((string)oDict["programId"]);
			oFM.cliente_id = int.Parse((string)oDict["clientId"]);
			oFM.estatura = double.Parse((string)oDict["height"]);
			oFM.contextura = (Contextures)(int.Parse((string)oDict["contexture"]));
			oFM.peso_actual = double.Parse((string)oDict["actualWeight"]);
			oFM.peso_deseado = double.Parse((string)oDict["wishedWeight"]);
			oFM.peso_ideal = double.Parse((string)oDict["idealWeight"]);

			// C_2
			var controlsJArray = (Newtonsoft.Json.Linq.JArray)oDict["controls"];
			var controlsJArrayAsString = controlsJArray.ToString();
			List<Dictionary<string, object>> oControlsLst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(controlsJArrayAsString);
			List<ControlesNutricionales> controlesNutricionalesLst = new List<ControlesNutricionales>();
			foreach (Dictionary<string, object> c in oControlsLst)
			{
				ControlesNutricionales cn = new ControlesNutricionales();
				string format = "MM'/'dd'/'yyyy";
				System.Globalization.CultureInfo pe = new System.Globalization.CultureInfo("es-PE");
				cn.fecha = DateTime.ParseExact((string)c["date"], format, pe);
				cn.act = double.Parse((string)c["act"]);
				cn.mme = double.Parse((string)c["mme"]);
				cn.mgc = double.Parse((string)c["mgc"]);
				cn.mc = double.Parse((string)c["imc"]);
				cn.pgc = double.Parse((string)c["pgc"]);
				cn.rcc = double.Parse((string)c["rcc"]);
				cn.peso = double.Parse((string)c["weight"]);
				cn.created_at = CurrentDate.getNow();
				cn.updated_at = CurrentDate.getNow();
				controlesNutricionalesLst.Add(cn);
			}
			oFM.controles = controlesNutricionalesLst;

			// C_3
			oFM.frec_fumador = (Frecuencies)(int.Parse((string)oDict["smokeFrecuency"]));
			oFM.frec_bebedor = (Frecuencies)(int.Parse((string)oDict["drinkFrecuency"]));
			oFM.cortisona = (bool)oDict["cortisone"];
			oFM.esteroidesAnabolicos = (bool)oDict["anabolics"];
			oFM.anfetaminas = (bool)oDict["anfetamines"];
			oFM.marihuana = (bool)oDict["marihuana"];
			oFM.otra = (bool)oDict["other"];
			oFM.ninguna = (bool)oDict["none"];
			WomenMenstruationInfo m = new WomenMenstruationInfo();
			oFM.menstruacion_menopausia = m;

			int childQty = 0;
			int.TryParse((string)oDict["childQty"], out childQty);
			oFM.menstruacion_menopausia.cantidad_hijos = childQty;

			int birthType = 0;
			bool bt = int.TryParse((string)oDict["birthType"], out birthType);
			if (bt) oFM.menstruacion_menopausia.tipo_parto = (BirthTypes)birthType;

			int menopauseAge = 0;
			bool ma = int.TryParse((string)oDict["menopauseAge"], out menopauseAge);
			if (ma) oFM.menstruacion_menopausia.edad_menopausia = menopauseAge;

			oFM.menstruacion_menopausia.menopausia_activa = (bool)oDict["menopauseActive"];

			if (string.IsNullOrEmpty((string)oDict["hormoneTreatment"])) oFM.menstruacion_menopausia.tratamiento_hormonal = "";
			else oFM.menstruacion_menopausia.tratamiento_hormonal = (string)oDict["hormoneTreatment"];

			Background bg = new Background();
			oFM.antecedentes = bg;

			oFM.antecedentes.gastricos = (bool)oDict["bgGastric"];
			oFM.antecedentes.diabetes = (bool)oDict["bgDiabetes"];
			oFM.antecedentes.trigliceridos_altos = (bool)oDict["bgHighTriglicerids"];
			oFM.antecedentes.hipertiroidismo = (bool)oDict["bgHipertiroidism"];
			oFM.antecedentes.hipotiroidismo = (bool)oDict["bgHipotiroidism"];
			oFM.antecedentes.quistes = (bool)oDict["bgQuists"];
			oFM.antecedentes.tumores = (bool)oDict["bgTumors"];
			oFM.antecedentes.resistencia_insulina = (bool)oDict["bgInsulinResistant"];
			oFM.antecedentes.otras_dislipidemias = (bool)oDict["bgOtherDyslipidemias"];
			oFM.antecedentes.prostata = (bool)oDict["bgProstate"];
			oFM.antecedentes.renales = (bool)oDict["bgRenal"];
			oFM.antecedentes.hepaticos = (bool)oDict["bgHepatic"];
			oFM.antecedentes.presion_alta = (bool)oDict["bgHighPresure"];
			oFM.antecedentes.presion_baja = (bool)oDict["bgLowPresure"];
			oFM.antecedentes.colesterol_alto = (bool)oDict["bgHighCholesterol"];
			oFM.antecedentes.anemia = (bool)oDict["bgAnemia"];
			oFM.antecedentes.hernias = (bool)oDict["bgHernia"];
			oFM.antecedentes.estrenimiento = (bool)oDict["bgConstipation"];
			oFM.antecedentes.no_mencionada = (bool)oDict["bgNotMentioned"];

			TraumatologyProblems tp = new TraumatologyProblems();
			oFM.problemas_traumatologicos = tp;
			oFM.problemas_traumatologicos.dolores_articulares = (bool)oDict["traumaArticulationPain"];
			oFM.problemas_traumatologicos.osteoporosis = (bool)oDict["traumaOsteoporosis"];
			oFM.problemas_traumatologicos.problemas_columna = (bool)oDict["traumaColumnProblems"];

			if (string.IsNullOrEmpty((string)oDict["traumaColumnProblemsText"])) oFM.problemas_traumatologicos.problemas_columna_texto = "";
			else oFM.problemas_traumatologicos.problemas_columna_texto = (string)oDict["traumaColumnProblemsText"];

			oFM.problemas_traumatologicos.otras_lesiones = (bool)oDict["traumaOtherInjuries"];

			if (string.IsNullOrEmpty((string)oDict["traumaOtherInjuriesText"])) oFM.problemas_traumatologicos.otras_lesiones_texto = "";
			else oFM.problemas_traumatologicos.otras_lesiones_texto = (string)oDict["traumaOtherInjuriesText"];

			RespiratoryProblems rp = new RespiratoryProblems();
			oFM.problemas_respiratorios = rp;
			oFM.problemas_respiratorios.asma_cronica = (bool)oDict["respiratoryChronicAsthma"];
			oFM.problemas_respiratorios.tos_cronica = (bool)oDict["respiratoryChronicCough"];
			oFM.problemas_respiratorios.resfriados_continuos = (bool)oDict["respiratoryContinuosColds"];
			oFM.problemas_respiratorios.bronquitis = (bool)oDict["respiratoryBronquitis"];
			oFM.problemas_respiratorios.rinitis_alergica = (bool)oDict["respiratoryRinitis"];
			oFM.problemas_respiratorios.sinusitis = (bool)oDict["respiratorySinusitis"];
			oFM.problemas_respiratorios.amigdalitis = (bool)oDict["respiratoryAmigdalitis"];
			oFM.problemas_respiratorios.otros = (bool)oDict["respiratoryOther"];

			GeneralBackground gbg = new GeneralBackground();
			oFM.antecedentas_generales = gbg;
			oFM.antecedentas_generales.obesidad = (bool)oDict["gbgObesity"];
			oFM.antecedentas_generales.hipertension = (bool)oDict["gbgHipertension"];
			oFM.antecedentas_generales.diabetes = (bool)oDict["gbgDiabetes"];
			oFM.antecedentas_generales.cardiopatias = (bool)oDict["gbgCardiopathy"];
			oFM.antecedentas_generales.no_mencionada = (bool)oDict["gbgNotMentioned"];

			NutritionalAnamnesis na = new NutritionalAnamnesis();
			oFM.anamnesis_nutricional = na;
			oFM.anamnesis_nutricional.alimentos_fuera_de_casa = (bool)oDict["anamnesisFoodOutOfHome"];
			oFM.anamnesis_nutricional.frecAlimentosFuera = (Frecuencies)(int.Parse((string)oDict["anamnesisFoodOutOfHomeFrec"]));
			oFM.anamnesis_nutricional.tipo_alimentos_fuera = (string)oDict["anamnesisFoodOutOfHomeType"];

			int waterQty = 0;
			int.TryParse((string)oDict["anamnesisWaterGlassQty"], out waterQty);
			oFM.anamnesis_nutricional.cantidad_vasos_agua_dia = waterQty;

			oFM.anamnesis_nutricional.alimentos_preferencia = (string)oDict["anamnesisPreferredFood"];
			oFM.anamnesis_nutricional.alimentos_no_preferencia = (string)oDict["anamnesisNonPreferredFood"];
			oFM.anamnesis_nutricional.alimentos_daninos = (string)oDict["anamnesisUnsafeFood"];

			// C_4
			var foodRegimeJArray = (Newtonsoft.Json.Linq.JArray)oDict["foodRegimes"];
			var foodRegimeJArrayAsString = foodRegimeJArray.ToString();
			List<Dictionary<string, object>> oFoodRegimeLst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(foodRegimeJArrayAsString);
			List<FeedingRegime> foodRegimesLst = new List<FeedingRegime>();
			foreach (Dictionary<string, object> f in oFoodRegimeLst)
			{
				FeedingRegime fr = new FeedingRegime();
				fr.tipo_comida = (FoodTypes)int.Parse((string)f["foodType"]);
				fr.hora = TimeSpan.Parse((string)f["hour"]);
				fr.detalles = (string)f["detail"];
				foodRegimesLst.Add(fr);
			}
			oFM.regimen_alimentacion = foodRegimesLst;

			oFM.created_at = CurrentDate.getNow();
			oFM.created_by = SessionPersister.UserId;
			oFM.updated_at = CurrentDate.getNow();
			oFM.updated_by = SessionPersister.UserId;

			if (ModelState.IsValid)
			{
				db.fichas_medicas.Add(oFM);
				await db.SaveChangesAsync();
				return "Ok";
			}

			return "Error al guardar información.";
		}

		// GET: FichasMedicas/Edit/5
		public ActionResult Edit(int id)
		{
			fichas_medicas fm = db.fichas_medicas.Where(m => m.id == id).Include(m => m.controles).Include(m => m.regimen_alimentacion).Include(m => m.cardio).Include(m => m.laboratorio).OrderByDescending(m => m.created_at).FirstOrDefault();

			DateTime today = CurrentDate.getNow();
			programa_clientes prgCl = db.programa_clientes.Where(m => m.cliente_id == fm.cliente_id && m.fecha_inicio <= today && m.fecha_fin >= today).FirstOrDefault();

			ViewBag.currentCardioControl = null;
			if (fm != null && fm.cardio.Count > 0)
			{
				CardioInfo lastCardioInfoInserted = fm.cardio.OrderByDescending(m => m.fecha).First();

				TimeSpan dateDif = CurrentDate.getNow().Subtract(lastCardioInfoInserted.fecha);

				if (dateDif <= new TimeSpan(180, 0, 0, 0))
				{
					ViewBag.currentCardioControl = lastCardioInfoInserted;
				}
			}

			ViewBag.currentLabControl = null;
			if (fm != null && fm.laboratorio.Count > 0)
			{
				LabResults lastLabInfoInserted = fm.laboratorio.OrderByDescending(m => m.fecha).First();

				TimeSpan dateDif = CurrentDate.getNow().Subtract(lastLabInfoInserted.fecha);

				if (dateDif <= new TimeSpan(180, 0, 0, 0))
				{
					ViewBag.currentLabControl = lastLabInfoInserted;
				}
			}

			if (fm != null && fm.controles.Count > 0)
			{
				List<ControlesNutricionales> cn = fm.controles;
				List<string> cnRows = new List<string>();
				foreach (ControlesNutricionales c in cn)
				{
					string row = GetNutritionalControlString(c: c);
					cnRows.Add(row);
					controlsCount++;
				}
				controlsCount = 0;
				ViewBag.controlsLst = cnRows;
			}
			ViewBag.regimeInitialCount = 0;
			if (fm != null && fm.regimen_alimentacion.Count > 0)
			{
				List<FeedingRegime> fr = fm.regimen_alimentacion;
				List<string> frList = new List<string>();
				foreach (FeedingRegime f in fr)
				{
					string row = GetFoodRegimeString(f: f);
					frList.Add(row);
					foodRegimesCount++;
				}
				foodRegimesCount = 0;
				ViewBag.regimenLst = frList;
				ViewBag.regimeInitialCount = frList.Count;
			}

			ViewBag.programId = prgCl.programa_id;

			return View(fm);
		}

		public string GetNutritionalControlString(ControlesNutricionales c)
		{
			string rowInit = "<tr id='r-" + controlsCount.ToString() + "'>";
			string dateCell = "<td><input id='r-" + controlsCount.ToString() + "-fecha' type='text' name='Fecha' value='" + c.fecha.ToShortDateString() + "' class='form-control search mar-0 datepicker' /></td>";
			string actCell = "<td><input id='r-" + controlsCount.ToString() + "-act' type='text' name='act' value='" + c.act.ToString() + "' class='form-control search mar-0 '  /></td>";
			string mmeCell = "<td><input id='r-" + controlsCount.ToString() + "-mme' type='text' name='mme' value='" + c.mme.ToString() + "' class='form-control search mar-0 ' /></td>";
			string mgcCell = "<td><input id='r-" + controlsCount.ToString() + "-mgc' type='text' name='mgc' value='" + c.mgc.ToString() + "' class='form-control search mar-0 ' /></td>";
			string mcCell = "<td><input id='r-" + controlsCount.ToString() + "-mc' type='text' name='mc' value='" + c.mc.ToString() + "' class='form-control search mar-0 ' /></td>";
			string pgcCell = "<td><input id='r-" + controlsCount.ToString() + "-pgc' type='text' name='pgc' value='" + c.pgc.ToString() + "' class='form-control search mar-0 ' /></td>";
			string rccCell = "<td><input id='r-" + controlsCount.ToString() + "-rcc' type='text' name='rcc' value='" + c.rcc.ToString() + "' class='form-control search mar-0 ' /></td>";
			string pesoCell = "<td><input id='r-" + controlsCount.ToString() + "-peso' type='text' name='peso' value='" + c.peso.ToString() + "' class='form-control search mar-0 ' /></td>";
			string idCell = "<td style='display: none;'><input id='r-" + controlsCount.ToString() + "-id' type='text' name='id' value='" + c.id.ToString() + "' class='form-control search mar-0 ' /></td>";
			string created_atCell = "<td style='display: none;'><input id='r-" + controlsCount.ToString() + "-created-at' type='text' name='created_at' value='" + c.created_at.ToString() + "' class='form-control search mar-0 ' /></td>";
			string rowEnd = "</tr>";

			return rowInit + dateCell + actCell + mmeCell + mgcCell + mcCell + pgcCell + rccCell + pesoCell + idCell + created_atCell + rowEnd;
		}

		public string GetFoodRegimeString(FeedingRegime f)
		{
			string frModalId = "fr-" + foodRegimesCount.ToString();
			string frModalIdSelectDiv = frModalId + "-Select";
			string frModalIdTimeDiv = frModalId + "-Time";
			string frModalIdDetailDiv = frModalId + "-Detail";
			string frModalIdIdDiv = frModalId + "-Id";

			string divStart = "<div id='" + frModalId + "' class='model food-regime'>";
			string divEnd = "</div>";

			string div_select_start = "<div id='" + frModalIdSelectDiv + "' class='mar-bot-10'>";
			int selectedValue = (int)f.tipo_comida;
			string opt_1 = selectedValue == 0 ? "selected='selected'" : "";
			string opt_2 = selectedValue == 1 ? "selected='selected'" : "";
			string opt_3 = selectedValue == 2 ? "selected='selected'" : "";
			string opt_4 = selectedValue == 3 ? "selected='selected'" : "";
			string opt_5 = selectedValue == 4 ? "selected='selected'" : "";
			string dropdown = "<select id='" + frModalIdSelectDiv + "-type' value='" + selectedValue + "' class='form-control search mar-0' name='tipo_comida'>" +
				"<option value='0' " + opt_1 + ">Desayuno</option>" +
				"<option value='1' " + opt_2 + ">MediaMañana</option>" +
				"<option value='2' " + opt_3 + ">Almuerzo</option>" +
				"<option value='3' " + opt_4 + ">MediaTarde</option>" +
				"<option value='4' " + opt_5 + ">Cena</option></select>";

			string div_time_start = "<div id='" + frModalIdTimeDiv + "' class='mar-bot-10'>";
			string timepicker = "<input id='" + frModalIdTimeDiv + "-hour' type='text' name='hora' value='" + f.hora.ToString("hh':'mm") + "' class='form-control search mar-0' />";

			string div_textarea_start = "<div id='" + frModalIdDetailDiv + "' class='mar-bot-10'>";
			string textarea = "<textarea class='form-control search mar-0' cols='20' id='" + frModalIdDetailDiv + "-detail' name='detalles' rows='2'>" + f.detalles + "</textarea>";

			string div_id_start = "<div id='" + frModalIdIdDiv + "' class='mar-bot-10'>";
			string idtextbox = "<textarea class='form-control search mar-0' cols='20' id='" + frModalIdIdDiv + "-id' name='id' rows='2' style='display: none;'>" + f.id + "</textarea>";

			return divStart + div_select_start + dropdown + divEnd + div_time_start + timepicker + divEnd + div_textarea_start + textarea + divEnd + div_id_start + idtextbox + divEnd + divEnd;
		}

		// POST: FichasMedicas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<string> Edit(string json)
		{
			Dictionary<string, object> oDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

			int fmID = int.Parse((string)oDict["fmId"]);

			fichas_medicas oFM = db.fichas_medicas.Where(f => f.id == fmID).Include(f => f.controles).Include(f => f.regimen_alimentacion).FirstOrDefault();

			// C_1
			#region C_1
			oFM.programa_id = int.Parse((string)oDict["programId"]);
			oFM.cliente_id = int.Parse((string)oDict["clientId"]);
			oFM.estatura = double.Parse((string)oDict["height"]);
			oFM.contextura = (Contextures)(int.Parse((string)oDict["contexture"]));
			oFM.peso_actual = double.Parse((string)oDict["actualWeight"]);
			oFM.peso_deseado = double.Parse((string)oDict["wishedWeight"]);
			oFM.peso_ideal = double.Parse((string)oDict["idealWeight"]);
			#endregion

			// C_2
			#region C_2
			var controlsJArray = (Newtonsoft.Json.Linq.JArray)oDict["controls"];
			var controlsJArrayAsString = controlsJArray.ToString();
			List<Dictionary<string, object>> oControlsLst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(controlsJArrayAsString);
			List<ControlesNutricionales> controlesNutricionalesLst = new List<ControlesNutricionales>();
			List<ControlesNutricionales> controlesGuardadosLst = oFM.controles;

			foreach (Dictionary<string, object> c in oControlsLst)
			{
				if (c.Count > 0)
				{
					ControlesNutricionales cn = new ControlesNutricionales();
					int id = 0;
					bool exists = int.TryParse((string)c["id"], out id);
					if (exists)
					{
						cn.id = id;
						cn.created_at = DateTime.Parse((string)c["created_at"]);
					}
					else
					{
						cn.created_at = CurrentDate.getNow();
					}
					string format = "MM'/'dd'/'yyyy";
					System.Globalization.CultureInfo pe = new System.Globalization.CultureInfo("es-PE");
					cn.fecha = DateTime.ParseExact((string)c["date"], format, pe);
					cn.act = double.Parse((string)c["act"]);
					cn.mme = double.Parse((string)c["mme"]);
					cn.mgc = double.Parse((string)c["mgc"]);
					cn.mc = double.Parse((string)c["imc"]);
					cn.pgc = double.Parse((string)c["pgc"]);
					cn.rcc = double.Parse((string)c["rcc"]);
					cn.peso = double.Parse((string)c["weight"]);
					cn.updated_at = CurrentDate.getNow();
					controlesNutricionalesLst.Add(cn);
				}
			}

			for (int c = 0; c < controlesNutricionalesLst.Count; c++)
			{
				if (controlesNutricionalesLst[c].id > 0)
				{
					controlesGuardadosLst[c].fecha = controlesNutricionalesLst[c].fecha;
					controlesGuardadosLst[c].act = controlesNutricionalesLst[c].act;
					controlesGuardadosLst[c].mme = controlesNutricionalesLst[c].mme;
					controlesGuardadosLst[c].mgc = controlesNutricionalesLst[c].mgc;
					controlesGuardadosLst[c].mc = controlesNutricionalesLst[c].mc;
					controlesGuardadosLst[c].pgc = controlesNutricionalesLst[c].pgc;
					controlesGuardadosLst[c].rcc = controlesNutricionalesLst[c].rcc;
					controlesGuardadosLst[c].peso = controlesNutricionalesLst[c].peso;
					controlesGuardadosLst[c].updated_at = controlesNutricionalesLst[c].updated_at;
				}
				else
				{
					controlesGuardadosLst.Add(controlesNutricionalesLst[c]);
				}
			}
			#endregion

			// C_3
			#region C_3
			oFM.frec_fumador = (Frecuencies)(int.Parse((string)oDict["smokeFrecuency"]));
			oFM.frec_bebedor = (Frecuencies)(int.Parse((string)oDict["drinkFrecuency"]));
			oFM.cortisona = (bool)oDict["cortisone"];
			oFM.esteroidesAnabolicos = (bool)oDict["anabolics"];
			oFM.anfetaminas = (bool)oDict["anfetamines"];
			oFM.marihuana = (bool)oDict["marihuana"];
			oFM.otra = (bool)oDict["other"];
			oFM.ninguna = (bool)oDict["none"];
			WomenMenstruationInfo m = new WomenMenstruationInfo();
			oFM.menstruacion_menopausia = m;

			int childQty = 0;
			int.TryParse((string)oDict["childQty"], out childQty);
			oFM.menstruacion_menopausia.cantidad_hijos = childQty;

			int birthType = 0;
			bool bt = int.TryParse((string)oDict["birthType"], out birthType);
			if (bt) oFM.menstruacion_menopausia.tipo_parto = (BirthTypes)birthType;

			int menopauseAge = 0;
			bool ma = int.TryParse((string)oDict["menopauseAge"], out menopauseAge);
			if (ma) oFM.menstruacion_menopausia.edad_menopausia = menopauseAge;

			oFM.menstruacion_menopausia.menopausia_activa = (bool)oDict["menopauseActive"];

			if (string.IsNullOrEmpty((string)oDict["hormoneTreatment"])) oFM.menstruacion_menopausia.tratamiento_hormonal = "";
			else oFM.menstruacion_menopausia.tratamiento_hormonal = (string)oDict["hormoneTreatment"];

			Background bg = new Background();
			oFM.antecedentes = bg;

			oFM.antecedentes.gastricos = (bool)oDict["bgGastric"];
			oFM.antecedentes.diabetes = (bool)oDict["bgDiabetes"];
			oFM.antecedentes.trigliceridos_altos = (bool)oDict["bgHighTriglicerids"];
			oFM.antecedentes.hipertiroidismo = (bool)oDict["bgHipertiroidism"];
			oFM.antecedentes.hipotiroidismo = (bool)oDict["bgHipotiroidism"];
			oFM.antecedentes.quistes = (bool)oDict["bgQuists"];
			oFM.antecedentes.tumores = (bool)oDict["bgTumors"];
			oFM.antecedentes.resistencia_insulina = (bool)oDict["bgInsulinResistant"];
			oFM.antecedentes.otras_dislipidemias = (bool)oDict["bgOtherDyslipidemias"];
			oFM.antecedentes.prostata = (bool)oDict["bgProstate"];
			oFM.antecedentes.renales = (bool)oDict["bgRenal"];
			oFM.antecedentes.hepaticos = (bool)oDict["bgHepatic"];
			oFM.antecedentes.presion_alta = (bool)oDict["bgHighPresure"];
			oFM.antecedentes.presion_baja = (bool)oDict["bgLowPresure"];
			oFM.antecedentes.colesterol_alto = (bool)oDict["bgHighCholesterol"];
			oFM.antecedentes.anemia = (bool)oDict["bgAnemia"];
			oFM.antecedentes.hernias = (bool)oDict["bgHernia"];
			oFM.antecedentes.estrenimiento = (bool)oDict["bgConstipation"];
			oFM.antecedentes.no_mencionada = (bool)oDict["bgNotMentioned"];

			TraumatologyProblems tp = new TraumatologyProblems();
			oFM.problemas_traumatologicos = tp;
			oFM.problemas_traumatologicos.dolores_articulares = (bool)oDict["traumaArticulationPain"];
			oFM.problemas_traumatologicos.osteoporosis = (bool)oDict["traumaOsteoporosis"];
			oFM.problemas_traumatologicos.problemas_columna = (bool)oDict["traumaColumnProblems"];

			if (string.IsNullOrEmpty((string)oDict["traumaColumnProblemsText"])) oFM.problemas_traumatologicos.problemas_columna_texto = "";
			else oFM.problemas_traumatologicos.problemas_columna_texto = (string)oDict["traumaColumnProblemsText"];

			oFM.problemas_traumatologicos.otras_lesiones = (bool)oDict["traumaOtherInjuries"];

			if (string.IsNullOrEmpty((string)oDict["traumaOtherInjuriesText"])) oFM.problemas_traumatologicos.otras_lesiones_texto = "";
			else oFM.problemas_traumatologicos.otras_lesiones_texto = (string)oDict["traumaOtherInjuriesText"];

			RespiratoryProblems rp = new RespiratoryProblems();
			oFM.problemas_respiratorios = rp;
			oFM.problemas_respiratorios.asma_cronica = (bool)oDict["respiratoryChronicAsthma"];
			oFM.problemas_respiratorios.tos_cronica = (bool)oDict["respiratoryChronicCough"];
			oFM.problemas_respiratorios.resfriados_continuos = (bool)oDict["respiratoryContinuosColds"];
			oFM.problemas_respiratorios.bronquitis = (bool)oDict["respiratoryBronquitis"];
			oFM.problemas_respiratorios.rinitis_alergica = (bool)oDict["respiratoryRinitis"];
			oFM.problemas_respiratorios.sinusitis = (bool)oDict["respiratorySinusitis"];
			oFM.problemas_respiratorios.amigdalitis = (bool)oDict["respiratoryAmigdalitis"];
			oFM.problemas_respiratorios.otros = (bool)oDict["respiratoryOther"];

			GeneralBackground gbg = new GeneralBackground();
			oFM.antecedentas_generales = gbg;
			oFM.antecedentas_generales.obesidad = (bool)oDict["gbgObesity"];
			oFM.antecedentas_generales.hipertension = (bool)oDict["gbgHipertension"];
			oFM.antecedentas_generales.diabetes = (bool)oDict["gbgDiabetes"];
			oFM.antecedentas_generales.cardiopatias = (bool)oDict["gbgCardiopathy"];
			oFM.antecedentas_generales.no_mencionada = (bool)oDict["gbgNotMentioned"];

			NutritionalAnamnesis na = new NutritionalAnamnesis();
			oFM.anamnesis_nutricional = na;
			oFM.anamnesis_nutricional.alimentos_fuera_de_casa = (bool)oDict["anamnesisFoodOutOfHome"];
			oFM.anamnesis_nutricional.frecAlimentosFuera = (Frecuencies)(int.Parse((string)oDict["anamnesisFoodOutOfHomeFrec"]));
			oFM.anamnesis_nutricional.tipo_alimentos_fuera = (string)oDict["anamnesisFoodOutOfHomeType"];

			int waterQty = 0;
			int.TryParse((string)oDict["anamnesisWaterGlassQty"], out waterQty);
			oFM.anamnesis_nutricional.cantidad_vasos_agua_dia = waterQty;

			oFM.anamnesis_nutricional.alimentos_preferencia = (string)oDict["anamnesisPreferredFood"];
			oFM.anamnesis_nutricional.alimentos_no_preferencia = (string)oDict["anamnesisNonPreferredFood"];
			oFM.anamnesis_nutricional.alimentos_daninos = (string)oDict["anamnesisUnsafeFood"];
			#endregion

			// C_4
			#region C_4
			var foodRegimeJArray = (Newtonsoft.Json.Linq.JArray)oDict["foodRegimes"];
			var foodRegimeJArrayAsString = foodRegimeJArray.ToString();
			List<Dictionary<string, object>> oFoodRegimeLst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(foodRegimeJArrayAsString);
			List<FeedingRegime> foodRegimesLst = new List<FeedingRegime>();
			List<FeedingRegime> foodRegimesGuardadosLst = oFM.regimen_alimentacion;

			foreach (Dictionary<string, object> f in oFoodRegimeLst)
			{
				if (f.Count > 0)
				{
					FeedingRegime fr = new FeedingRegime();
					int id = 0;
					bool exists = int.TryParse((string)f["id"], out id);
					if (exists)
					{
						fr.id = id;
					}
					fr.tipo_comida = (FoodTypes)int.Parse((string)f["foodType"]);
					fr.hora = TimeSpan.Parse((string)f["hour"]);
					fr.detalles = (string)f["detail"];
					foodRegimesLst.Add(fr);
				}
			}

			for (int c = 0; c < foodRegimesLst.Count; c++)
			{
				if (foodRegimesLst[c].id > 0)
				{
					foodRegimesGuardadosLst[c].id = foodRegimesLst[c].id;
					foodRegimesGuardadosLst[c].tipo_comida = foodRegimesLst[c].tipo_comida;
					foodRegimesGuardadosLst[c].hora = foodRegimesLst[c].hora;
					foodRegimesGuardadosLst[c].detalles = foodRegimesLst[c].detalles;
				}
				else
				{
					foodRegimesGuardadosLst.Add(foodRegimesLst[c]);
				}
			}
			oFM.regimen_alimentacion = foodRegimesLst;
			#endregion

			oFM.updated_at = CurrentDate.getNow();
			oFM.updated_by = SessionPersister.UserId;

			if (ModelState.IsValid)
			{
				db.Entry(oFM).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return "Ok";
			}

			return "Error al guardar información.";
		}

		// GET: FichasMedicas/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			fichas_medicas fichas_medicas = await db.fichas_medicas.FindAsync(id);
			if (fichas_medicas == null)
			{
				return HttpNotFound();
			}
			return View(fichas_medicas);
		}

		// POST: FichasMedicas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			fichas_medicas fichas_medicas = await db.fichas_medicas.FindAsync(id);
			db.fichas_medicas.Remove(fichas_medicas);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
