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

namespace SEPAMIAR_SGC.Controllers
{
	public class PermisosController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: Permisos
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("access"))
				.ToList();

			ViewBag.Permisos = userAccesses;


			return View(await db.permisos.OrderBy(m=>m.codigo_interno).ToListAsync());
		}

		// GET: Permisos/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			permisos permisos = await db.permisos.FindAsync(id);
			if (permisos == null)
			{
				return HttpNotFound();
			}
			return View(permisos);
		}

		// GET: Permisos/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Permisos/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "id,codigo_interno,nombre,created_at,updated_at,deleted_at")] permisos permisos)
		{

			permisos.created_at = DateTimeOffset.Now.Date;
			permisos.updated_at = DateTimeOffset.Now.Date;

			if (ModelState.IsValid)
			{
				db.permisos.Add(permisos);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(permisos);
		}

		// GET: Permisos/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			permisos permisos = await db.permisos.FindAsync(id);
			if (permisos == null)
			{
				return HttpNotFound();
			}
			return View(permisos);
		}

		// POST: Permisos/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id,nombre,created_at,updated_at,deleted_at")] permisos permisos)
		{
			if (ModelState.IsValid)
			{
				db.Entry(permisos).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(permisos);
		}

		// GET: Permisos/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			permisos permisos = await db.permisos.FindAsync(id);
			if (permisos == null)
			{
				return HttpNotFound();
			}
			return View(permisos);
		}

		// POST: Permisos/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			permisos permisos = await db.permisos.FindAsync(id);
			db.permisos.Remove(permisos);
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
