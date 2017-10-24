using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SEPAMIAR_SGC.Models;
using SEPAMIAR_SGC.Utilities;
using System.Text;

namespace SEPAMIAR_SGC.Controllers
{
	public class ClientesDietasController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: ClientesDietas
		public async Task<ActionResult> Index(int? clientCode)
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("cldiet"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			IQueryable<clientes_dietas> dietas;

			if (clientCode == null)
			{
				dietas = db.clientes_dietas.Include(c => c.programa_clientes).OrderByDescending(m => m.created_at).Take(15);
				return View(await dietas.ToListAsync());
			}

			dietas = db.clientes_dietas.Where(m => m.programa_clientes.cliente_id == clientCode).Include(m => m.programa_clientes).OrderByDescending(m => m.created_at).Take(15);
			return View(await dietas.ToListAsync());
		}

		public ActionResult GetDietsForClient(int? value)
		{
			if (value == null)
			{
				return RedirectToAction("Index");
			}

			string clientCode = value?.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			return RedirectToAction("Index", new { clientCode = c });
		}

		public ActionResult GetNewDiet(int Value, int Prog)
		{
			return RedirectToAction("Create", new { Value = Value, Prog = Prog });
		}

		// GET: ClientesDietas/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clientes_dietas clientes_dietas = await db.clientes_dietas.FindAsync(id);
			if (clientes_dietas == null)
			{
				return HttpNotFound();
			}
			return View(clientes_dietas);
		}

		// GET: ClientesDietas/Create
		public ActionResult Create(int Value, int Prog)
		{
			ViewBag.clientId = Value;
			ViewBag.programId = Prog;
			return View();
		}

		// POST: ClientesDietas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Create(string json)
		{
			List<Dictionary<string, object>> oDict = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

			clientes_dietas cd = new clientes_dietas();
			cd.created_at = CurrentDate.getNow();
			cd.updated_at = CurrentDate.getNow();
			cd.programa_clientes_id = int.Parse(oDict[oDict.Count - 1]["programID"].ToString());
			List<DailyDiet> dietDetails = new List<DailyDiet>();

			for (int i = 0; i < oDict.Count - 1; i++)
			{
				DailyDiet dietDetail = new DailyDiet();

				dietDetail.clientes_dietas_id = cd.id;
				dietDetail.opcion = int.Parse(oDict[i]["option"].ToString());
				dietDetail.tipo_comida = (FoodTypes)int.Parse(oDict[i]["foodType"].ToString());
				dietDetail.texto = oDict[i]["detail"].ToString();
				dietDetail.dia = int.Parse(oDict[i]["day"].ToString()) + 1;

				dietDetails.Add(dietDetail);
			}

			cd.dieta_diaria = dietDetails;

			db.clientes_dietas.Add(cd);

			int s = await db.SaveChangesAsync();

			if (s > 0)
			{
				return RedirectToAction("Index");
			}

			ViewBag.clientId = int.Parse(oDict[oDict.Count - 1]["clientId"].ToString());
			ViewBag.programId = int.Parse(oDict[oDict.Count - 1]["programID"].ToString());

			return View();
		}

		// GET: ClientesDietas/Edit/5
		public async Task<ActionResult> Edit(int id)
		{
			ViewBag.programId = id;

			clientes_dietas clientes_dietas = await db.clientes_dietas.Where(m => m.id == id).Include(m => m.programa_clientes).Include(m => m.dieta_diaria).FirstOrDefaultAsync();

			ViewBag.clientId = clientes_dietas.programa_clientes.cliente_id;

			int createdOptions = clientes_dietas.dieta_diaria.OrderByDescending(d => d.opcion).Select(d => d.opcion).FirstOrDefault();

			ViewBag.optionsCount = createdOptions;

			StringBuilder diets = new StringBuilder();

			for (int opt = 0; opt < createdOptions; opt++)
			{
				StringBuilder dietsDiv = new StringBuilder();

				var newDietOptionLabelRow = "<div class='row mar-bot-10'><label class='diet-title-sub'>Opción " + (opt + 1).ToString() + "</label></div>";
				dietsDiv.Append(newDietOptionLabelRow);

				var dietDivRowId = "opt-" + opt.ToString();
				var newDietDivRow = "<div id='" + dietDivRowId + "' class='row diet-row mar-bot-15'>";
				dietsDiv.Append(newDietDivRow);

				StringBuilder dietOptionRow = new StringBuilder();

				for (var i = 0; i < 7; i++)
				{
					var optColId = dietDivRowId + "-col-" + i.ToString();
					if (i == 0)
					{
						var newDietColumn = "<div id='" + optColId + "' class='col-2'>";
						dietOptionRow.Append(newDietColumn);
					}
					else
					{
						var newDietColumn = "<div id='" + optColId + "' class='col-2 mar-lef-10'>";
						dietOptionRow.Append(newDietColumn);
					}

					StringBuilder currentColumn = new StringBuilder();

					switch (i)
					{
						case 0:
							currentColumn.Append("<label class='diet-title'>Lun</label>");
							break;
						case 1:
							currentColumn.Append("<label class='diet-title'>Mar</label>");
							break;
						case 2:
							currentColumn.Append("<label class='diet-title'>Mie</label>");
							break;
						case 3:
							currentColumn.Append("<label class='diet-title'>Jue</label>");
							break;
						case 4:
							currentColumn.Append("<label class='diet-title'>Vie</label>");
							break;
						case 5:
							currentColumn.Append("<label class='diet-title'>Sab</label>");
							break;
						case 6:
							currentColumn.Append("<label class='diet-title'>Dom</label>");
							break;
						default:
							break;
					}

					for (var j = 0; j < 5; j++)
					{
						var currentBoxId = optColId + "-row-" + j.ToString();
						int dia = i + 1;
						int opc = opt + 1;
						FoodTypes tc = (FoodTypes)j;
						DailyDiet existingDiet = clientes_dietas.dieta_diaria.Where(c => c.dia == dia && c.tipo_comida == tc && c.opcion == opc).FirstOrDefault();
						switch (j)
						{
							case 0:

								if (existingDiet != null)
								{
									currentColumn.Append("<div class='model daily-diet'><label>Desayuno</label><textarea id='" + currentBoxId + "'>" + existingDiet.texto + "</textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='" + existingDiet.id + "'/>");
								}
								else
								{
									currentColumn.Append("<div class='model daily-diet'><label>Desayuno</label><textarea id='" + currentBoxId + "'></textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='0'/>");
								}

								break;
							case 1:

								if (existingDiet != null)
								{
									currentColumn.Append("<div class='model daily-diet'><label>Media Mañana</label><textarea id='" + currentBoxId + "'>" + existingDiet.texto + "</textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='" + existingDiet.id + "'/>");
								}
								else
								{
									currentColumn.Append("<div class='model daily-diet'><label>Media Mañana</label><textarea id='" + currentBoxId + "'></textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='0'/>");
								}
								break;
							case 2:

								if (existingDiet != null)
								{
									currentColumn.Append("<div class='model daily-diet'><label>Almuerzo</label><textarea id='" + currentBoxId + "'>" + existingDiet.texto + "</textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='" + existingDiet.id + "'/>");
								}
								else
								{
									currentColumn.Append("<div class='model daily-diet'><label>Almuerzo</label><textarea id='" + currentBoxId + "'></textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='0'/>");
								}
								break;
							case 3:

								if (existingDiet != null)
								{
									currentColumn.Append("<div class='model daily-diet'><label>Media Tarde</label><textarea id='" + currentBoxId + "'>" + existingDiet.texto + "</textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='" + existingDiet.id + "'/>");
								}
								else
								{
									currentColumn.Append("<div class='model daily-diet'><label>Media Tarde</label><textarea id='" + currentBoxId + "'></textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='0'/>");
								}
								break;
							case 4:
								if (existingDiet != null)
								{
									currentColumn.Append("<div class='model daily-diet'><label>Cena</label><textarea id='" + currentBoxId + "'>" + existingDiet.texto + "</textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='" + existingDiet.id + "'/>");
								}
								else
								{
									currentColumn.Append("<div class='model daily-diet'><label>Cena</label><textarea id='" + currentBoxId + "'></textarea></div>");
									currentColumn.Append("<input type='hidden' id='opt-" + opt + "-col-" + i + "-row-" + j + "-id' value='0'/>");
								}
								break;
							default:
								break;
						}
					}

					currentColumn.Append("</div>");

					dietOptionRow.Append(currentColumn.ToString());
				}

				dietOptionRow.Append("</div>");

				dietsDiv.Append(dietOptionRow.ToString());
				dietsDiv.Append("</div>");

				diets.Append(dietsDiv.ToString());
			}

			ViewBag.optionsHtml = diets.ToString();

			return View(clientes_dietas);
		}

		// POST: ClientesDietas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Edit(string json)
		{
			List<Dictionary<string, object>> oDict = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

			int dietId = int.Parse(oDict[oDict.Count - 1]["dietID"].ToString());

			IQueryable<clientes_dietas> cdQuery = db.clientes_dietas.Where(m => m.id == dietId).Include(m => m.dieta_diaria);
			clientes_dietas cd = cdQuery.FirstOrDefault();
			cd.updated_at = CurrentDate.getNow();

			List<DailyDiet> dietDetails = new List<DailyDiet>();

			for (int i = 0; i < oDict.Count - 1; i++)
			{
				int id = int.Parse(oDict[i]["id"].ToString());

				DailyDiet dd = null;

				if (id > 0)
				{
					dd = cd.dieta_diaria.Where(c => c.id == id).FirstOrDefault();
				}

				if (dd != null)
				{
					dd.clientes_dietas_id = cd.id;
					dd.opcion = int.Parse(oDict[i]["option"].ToString());
					dd.tipo_comida = (FoodTypes)int.Parse(oDict[i]["foodType"].ToString());
					dd.texto = oDict[i]["detail"].ToString();
					dd.dia = int.Parse(oDict[i]["day"].ToString()) + 1;

					dietDetails.Add(dd);
				}
				else
				{
					DailyDiet dietDetail = new DailyDiet();

					dietDetail.clientes_dietas_id = cd.id;
					dietDetail.opcion = int.Parse(oDict[i]["option"].ToString());
					dietDetail.tipo_comida = (FoodTypes)int.Parse(oDict[i]["foodType"].ToString());
					dietDetail.texto = oDict[i]["detail"].ToString();
					dietDetail.dia = int.Parse(oDict[i]["day"].ToString()) + 1;

					dietDetails.Add(dietDetail);
				}
			}

			cd.dieta_diaria = dietDetails;

			db.Entry(cd).State = EntityState.Modified;

			int s = await db.SaveChangesAsync();

			if (s > 0)
			{
				return RedirectToAction("Index");
			}

			ViewBag.clientId = int.Parse(oDict[oDict.Count - 1]["clientId"].ToString());
			ViewBag.programId = int.Parse(oDict[oDict.Count - 1]["programID"].ToString());

			return View();
		}

		// GET: ClientesDietas/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clientes_dietas clientes_dietas = await db.clientes_dietas.FindAsync(id);
			if (clientes_dietas == null)
			{
				return HttpNotFound();
			}
			return View(clientes_dietas);
		}

		// POST: ClientesDietas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			clientes_dietas clientes_dietas = await db.clientes_dietas.FindAsync(id);
			db.clientes_dietas.Remove(clientes_dietas);
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

		[HttpGet]
		public ActionResult M_GetClientDiets(string c)
		{
			clientes cl = db.clientes.Where(m => m.codigo == c).FirstOrDefault();
			Mobile oMobile = new Mobile();
			int today = (int)CurrentDate.getNow().DayOfWeek;

			if (today == 0)
			{
				today = 7;
			}

			if (cl != null)
			{
				programa_clientes clientPrg = db.programa_clientes
					.Where(m => m.cliente_id == cl.id_alt && m.fecha_fin >= DateTimeOffset.Now)
					.OrderBy(m => m.fecha_fin)
					.FirstOrDefault();
				List<DailyDiet> clientDiets = db.clientes_dietas
					.Where(m => m.programa_clientes_id == clientPrg.id)
					.Include(m => m.dieta_diaria)
					.Select(m => m.dieta_diaria)
					.FirstOrDefault();

				if (clientDiets != null && clientDiets.Count > 0)
				{
					Dictionary<string, object> oDict = new Dictionary<string, object>();
					int dietCount = 0;
					foreach (DailyDiet d in clientDiets)
					{
						if (d.dia == today)
						{
							Dictionary<string, object> dict = new Dictionary<string, object>();
							dict.Add("day", d.dia);
							dict.Add("food_type", d.tipo_comida);
							dict.Add("text", d.texto);
							dict.Add("option", d.opcion);

							oDict.Add("E_" + dietCount.ToString(), dict);

							dietCount++;
						}
					}
					return Json(
							oMobile.GetDictForJSON(
								message: "",
								data: oDict,
								code: MobileResponse.Success),
							JsonRequestBehavior.AllowGet);
				}
			}

			return Json(
				oMobile.GetDictForJSON(
					message: "Error en la solicitud",
					data: null,
					code: MobileResponse.Error),
				JsonRequestBehavior.AllowGet);
		}
	}
}
