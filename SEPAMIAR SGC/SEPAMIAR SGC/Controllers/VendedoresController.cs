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
    public class VendedoresController : Controller
    {
        private _SGCModel db = new _SGCModel();

		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("vendor"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			var vendedores = db.vendedores.Include(n => n.usuarios);
			return View(await vendedores.ToListAsync());
		}

		// GET: Nutricionistas/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			vendedores vendedores = await db.vendedores.FindAsync(id);
			if (vendedores == null)
			{
				return HttpNotFound();
			}
			return View(vendedores);
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
		public async Task<ActionResult> Create([Bind(Include = "usuario_id,jefe,created_at,updated_at,deleted_at")] vendedores vendedores)
		{
			if (ModelState.IsValid)
			{
				vendedores.created_at = CurrentDate.getNow();
				vendedores.updated_at = CurrentDate.getNow();

				db.vendedores.Add(vendedores);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", vendedores.usuario_id);
			return View(vendedores);
		}

		// GET: Nutricionistas/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			vendedores vendedores = await db.vendedores.FindAsync(id);
			if (vendedores == null)
			{
				return HttpNotFound();
			}
			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", vendedores.usuario_id);
			return View(vendedores);
		}

		// POST: Nutricionistas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "usuario_id,jefe,created_at,updated_at,deleted_at")] vendedores vendedores)
		{
			if (ModelState.IsValid)
			{
				vendedores.updated_at = CurrentDate.getNow();

				db.Entry(vendedores).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", vendedores.usuario_id);
			return View(vendedores);
		}

		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			vendedores vendedores = await db.vendedores.FindAsync(id);
			if (vendedores == null)
			{
				return HttpNotFound();
			}
			return View(vendedores);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			vendedores vendedores = await db.vendedores.FindAsync(id);
			//db.locales.Remove(locales);
			vendedores.activo = false;
			vendedores.deleted_at = CurrentDate.getNow();
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public async Task<ActionResult> Restore(int id)
		{
			vendedores vendedores = await db.vendedores.FindAsync(id);
			//db.locales.Remove(locales);
			vendedores.activo = true;
			vendedores.deleted_at = null;
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
