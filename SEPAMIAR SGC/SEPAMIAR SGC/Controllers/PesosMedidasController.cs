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
using SEPAMIAR_SGC.Seguridad;

namespace SEPAMIAR_SGC.Controllers
{
	public class PesosMedidasController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: PesosMedidas
		public async Task<ActionResult> Index(int? clientCode)
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("pymreg"))
				.ToList();

			ViewBag.Permisos = userAccesses;
			ViewBag.clientName = "";
			IQueryable<pesos_medidas> pesos_medidas;

			if (clientCode != null)
			{
				pesos_medidas = db.pesos_medidas.Where(c => c.clienteId == clientCode).Include(c => c.clientes).OrderByDescending(c => c.created_at).Take(15);

				if (pesos_medidas.ToList().Count > 0)
				{
					ViewBag.clientName = pesos_medidas.First().clientes.nombres;
					ViewBag.clientId = pesos_medidas.First().clientes.id_alt;

					return View(await pesos_medidas.ToListAsync());
				}
				else
				{
					return RedirectToAction("Create",new { clientId = clientCode });
				}
			}

			pesos_medidas = db.pesos_medidas.Include(p => p.clientes).Take(15);

			return View(await pesos_medidas.ToListAsync());
		}

		public ActionResult GetRegistriesForClient(int? value)
		{
			if (value == null)
			{
				return RedirectToAction("Index");
			}

			string clientCode = value?.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			return RedirectToAction("Index", new { clientCode = c });
		}

		// GET: PesosMedidas/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			pesos_medidas pesos_medidas = await db.pesos_medidas.FindAsync(id);
			if (pesos_medidas == null)
			{
				return HttpNotFound();
			}
			return View(pesos_medidas);
		}

		// GET: PesosMedidas/Create
		public ActionResult Create(int clientId)
		{
			ViewBag.clienteId = clientId;
			return View();
		}

		// POST: PesosMedidas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(pesos_medidas pesos_medidas)
		{
			if (ModelState.IsValid)
			{
				pesos_medidas pym = new pesos_medidas();
				Medidas measurements = new Medidas();
				pym.clienteId = pesos_medidas.clienteId;
				pym.peso_1 = pesos_medidas.peso_1;
				pym.peso_2 = pesos_medidas.peso_2;
				pym.peso_3 = pesos_medidas.peso_3;
				pym.porc_grasa_corporal = pesos_medidas.porc_grasa_corporal;
				pym.medidas = measurements;
				pym.created_at = CurrentDate.getNow();
				pym.created_by = SessionPersister.UserId;
				pym.updated_at = CurrentDate.getNow();
				pym.updated_by = SessionPersister.UserId;

				db.pesos_medidas.Add(pym);
				await db.SaveChangesAsync();
				return RedirectToAction("Index", new { clientCode = pym.clienteId });
			}

			ViewBag.clienteId = pesos_medidas.clienteId;
			return View(pesos_medidas);
		}

		// GET: PesosMedidas/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			pesos_medidas pesos_medidas = await db.pesos_medidas.Where(p => p.id == id).Include(p => p.clientes).FirstOrDefaultAsync();
			if (pesos_medidas == null)
			{
				return HttpNotFound();
			}
			return View(pesos_medidas);
		}

		// POST: PesosMedidas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(pesos_medidas pesos_medidas)
		{

			pesos_medidas.updated_at = CurrentDate.getNow();
			pesos_medidas.updated_by = SessionPersister.UserId;
			db.Entry(pesos_medidas).State = EntityState.Modified;
			int p = await db.SaveChangesAsync();

			if (p > 0)
			{
				return RedirectToAction("Index", new { clientCode = pesos_medidas.clienteId });
			}

			pesos_medidas.clientes = db.pesos_medidas.Where(m => m.id == pesos_medidas.id).Include(m => m.clientes).Select(m => m.clientes).FirstOrDefault();
			ViewBag.clienteId = pesos_medidas.clienteId;
			return View(pesos_medidas);
		}

		// GET: PesosMedidas/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			pesos_medidas pesos_medidas = await db.pesos_medidas.FindAsync(id);
			if (pesos_medidas == null)
			{
				return HttpNotFound();
			}
			return View(pesos_medidas);
		}

		// POST: PesosMedidas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			pesos_medidas pesos_medidas = await db.pesos_medidas.FindAsync(id);
			db.pesos_medidas.Remove(pesos_medidas);
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

		/*
		 ****************************************
		 *										*
		 *	AQUI EMPIEZAN LAS LOGICAS PARA LA	*
		 *	APP PT.								*
		 *										*
		 ****************************************		 
		 */

		[HttpGet]
		public async Task<ActionResult> M_GetWeeklyEvaluations(string c)
		{
			Mobile oMobile = new Mobile();
			int clId = await db.clientes.Where(m => m.codigo == c).Select(m => m.id_alt).FirstOrDefaultAsync();
			DateTime today = CurrentDate.getNow();

			if (clId > 0)
			{
				programa_clientes currClientProgram = db.programa_clientes.Where(m => m.cliente_id == clId && m.fecha_fin >= today).OrderBy(m => m.fecha_fin).FirstOrDefault();

				if (currClientProgram != null)
				{
					List<pesos_medidas> pymLst = await db.pesos_medidas.Where(m => m.clienteId == clId && m.created_at >= currClientProgram.fecha_inicio).OrderBy(m => m.created_at).ToListAsync();

					if (pymLst.Count > 0)
					{
						Dictionary<string, object> oDict = new Dictionary<string, object>();
						for (int i = 0; i < pymLst.Count; i++)
						{
							pesos_medidas pym = new pesos_medidas();
							Medidas measurements = new Medidas();
							pym.medidas = measurements;

							pym.medidas.cuello = pymLst[i].medidas.cuello;
							pym.medidas.biceps = pymLst[i].medidas.biceps;
							pym.medidas.gluteos = pymLst[i].medidas.gluteos;
							pym.medidas.hombros = pymLst[i].medidas.hombros;
							pym.medidas.muneca = pymLst[i].medidas.muneca;
							pym.medidas.muslo = pymLst[i].medidas.muslo;
							pym.medidas.torax = pymLst[i].medidas.torax;
							pym.medidas.cintura = pymLst[i].medidas.cintura;
							pym.medidas.pantorrilla = pymLst[i].medidas.pantorrilla;
							pym.peso_1 = pymLst[i].peso_1;
							pym.peso_2 = pymLst[i].peso_2;
							pym.peso_3 = pymLst[i].peso_3;

							oDict.Add("E_" + i.ToString(), pym);
						}

						return Json(
							oMobile.GetDictForJSON(
								message: oDict.Count.ToString() + " registrados para cliente.",
								data: oDict,
								code: MobileResponse.Success),
							JsonRequestBehavior.AllowGet);
					}
				}
			}

			return Json(
				oMobile.GetDictForJSON(
					message: "Error de validación de cliente.",
					data: null,
					code: MobileResponse.Error),
				JsonRequestBehavior.AllowGet);
		}
	}
}
