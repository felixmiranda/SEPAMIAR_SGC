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
using System.Globalization;

namespace SEPAMIAR_SGC.Controllers
{
	public class NutricionistasController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: Nutricionistas
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("nutri"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			var nutricionistas = db.nutricionistas.Include(n => n.usuarios);
			return View(await nutricionistas.ToListAsync());
		}

		// GET: Nutricionistas/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			nutricionistas nutricionistas = await db.nutricionistas.FindAsync(id);
			if (nutricionistas == null)
			{
				return HttpNotFound();
			}
			return View(nutricionistas);
		}

		// GET: Nutricionistas/Create
		public ActionResult Create()
		{
			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo");
			return View();
		}

		// POST: Nutricionistas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "usuario_id,jefe,created_at,updated_at,deleted_at")] nutricionistas nutricionistas)
		{
			if (ModelState.IsValid)
			{
				nutricionistas.created_at = CurrentDate.getNow();
				nutricionistas.updated_at = CurrentDate.getNow();

				db.nutricionistas.Add(nutricionistas);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", nutricionistas.usuario_id);
			return View(nutricionistas);
		}

		public async Task<JsonResult> GetScheduledDates(int id)
		{
			DateTime today = CurrentDate.getNow();
			DateTime dateLimit = CurrentDate.getNow().AddDays(5);
			var appointmentsLst = await db.citas.Where(m => m.nutricionista_id == id && m.fecha >= today && m.fecha <= dateLimit).OrderBy(m => m.fecha).ToListAsync();

			List<Dictionary<string, object>> oList = new List<Dictionary<string, object>>();

			foreach (citas c in appointmentsLst)
			{
				Dictionary<string, object> citaDict = new Dictionary<string, object>();
				citaDict.Add("fecha", c.fecha.ToShortDateString());
				citaDict.Add("hora", c.hora.Hours.ToString() + ":" + c.hora.Minutes.ToString());
				oList.Add(citaDict);
			}

			return Json(oList, JsonRequestBehavior.AllowGet);
		}

		// GET: Nutricionistas/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			nutricionistas nutricionistas = await db.nutricionistas.FindAsync(id);
			if (nutricionistas == null)
			{
				return HttpNotFound();
			}
			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", nutricionistas.usuario_id);
			return View(nutricionistas);
		}

		// POST: Nutricionistas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "usuario_id,jefe,created_at,updated_at,deleted_at")] nutricionistas nutricionistas)
		{
			if (ModelState.IsValid)
			{
				nutricionistas.updated_at = CurrentDate.getNow();

				db.Entry(nutricionistas).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", nutricionistas.usuario_id);
			return View(nutricionistas);
		}

		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			nutricionistas nutricionistas = await db.nutricionistas.FindAsync(id);
			if (nutricionistas == null)
			{
				return HttpNotFound();
			}
			return View(nutricionistas);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			nutricionistas nutricionistas = await db.nutricionistas.FindAsync(id);
			//db.locales.Remove(locales);
			nutricionistas.activo = false;
			nutricionistas.deleted_at = CurrentDate.getNow();
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public async Task<ActionResult> Restore(int id)
		{
			nutricionistas nutricionistas = await db.nutricionistas.FindAsync(id);
			//db.locales.Remove(locales);
			nutricionistas.activo = true;
			nutricionistas.deleted_at = null;
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
