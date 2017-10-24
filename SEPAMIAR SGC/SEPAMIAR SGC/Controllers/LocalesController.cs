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

namespace SEPAMIAR_SGC.Controllers
{
	public class LocalesController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: Locales
		[CustomAuthorize(SGC_AccessCode = "local_index")]
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("local"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			return View(await db.locales.ToListAsync());
		}

		// GET: Locales/Details/5
		[CustomAuthorize(SGC_AccessCode = "local_details")]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			locales locales = await db.locales.FindAsync(id);
			if (locales == null)
			{
				return HttpNotFound();
			}
			return View(locales);
		}

		// GET: Locales/Create
		[CustomAuthorize(SGC_AccessCode = "local_create")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: Locales/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "id,nombre,distrito,created_at,updated_at,deleted_at")] locales locales)
		{
			if (ModelState.IsValid)
			{
				locales lc = new locales();
				lc.nombre = locales.nombre;
				lc.distrito = locales.distrito;

				lc.created_at = CurrentDate.getNow();
				lc.updated_at = CurrentDate.getNow();

				db.locales.Add(lc);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(locales);
		}

		// GET: Locales/Edit/5
		[CustomAuthorize(SGC_AccessCode = "local_update")]
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			locales locales = await db.locales.FindAsync(id);
			if (locales == null)
			{
				return HttpNotFound();
			}
			return View(locales);
		}

		// POST: Locales/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id,nombre,distrito,created_at,updated_at,deleted_at")] locales locales)
		{
			if (ModelState.IsValid)
			{
				locales.updated_at = DateTimeOffset.Now.DateTime;

				db.Entry(locales).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(locales);
		}

		// GET: Locales/Delete/5
		[CustomAuthorize(SGC_AccessCode = "local_delete")]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			locales locales = await db.locales.FindAsync(id);
			if (locales == null)
			{
				return HttpNotFound();
			}
			return View(locales);
		}

		// POST: Locales/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			locales locales = await db.locales.FindAsync(id);
			//db.locales.Remove(locales);
			locales.deleted_at = DateTimeOffset.Now.DateTime;
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public async Task<ActionResult> Restore(int id)
		{
			locales locales = await db.locales.FindAsync(id);
			//db.locales.Remove(locales);
			locales.deleted_at = null;
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
