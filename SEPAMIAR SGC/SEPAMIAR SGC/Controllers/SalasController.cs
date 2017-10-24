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
    public class SalasController : Controller
    {
        private _SGCModel db = new _SGCModel();

        // GET: Salas
		[CustomAuthorize(SGC_AccessCode ="room_index")]
        public async Task<ActionResult> Index()
        {
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("room"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			var salas = db.salas.Include(s => s.local);
            return View(await salas.ToListAsync());
        }

		// GET: Salas/Details/5
		[CustomAuthorize(SGC_AccessCode = "room_details")]
		public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salas salas = await db.salas.FindAsync(id);
            if (salas == null)
            {
                return HttpNotFound();
            }
            return View(salas);
        }

		// GET: Salas/Create
		[CustomAuthorize(SGC_AccessCode = "room_create")]
		public ActionResult Create()
        {
            ViewBag.local_id = new SelectList(db.locales, "id", "nombre");
            return View();
        }

        // POST: Salas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,local_id,nombre,created_at,updated_at,deleted_at")] salas salas)
        {
            if (ModelState.IsValid)
            {
				salas.created_at = DateTimeOffset.Now.DateTime;
				salas.updated_at = DateTimeOffset.Now.DateTime;

				db.salas.Add(salas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.local_id = new SelectList(db.locales, "id", "nombre", salas.local_id);
            return View(salas);
        }

		// GET: Salas/Edit/5
		[CustomAuthorize(SGC_AccessCode = "room_update")]
		public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salas salas = await db.salas.FindAsync(id);
            if (salas == null)
            {
                return HttpNotFound();
            }
            ViewBag.local_id = new SelectList(db.locales, "id", "nombre", salas.local_id);
            return View(salas);
        }

        // POST: Salas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,local_id,nombre,created_at,updated_at,deleted_at")] salas salas)
        {
            if (ModelState.IsValid)
            {
				salas.updated_at = DateTimeOffset.Now.DateTime;

                db.Entry(salas).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.local_id = new SelectList(db.locales, "id", "nombre", salas.local_id);
            return View(salas);
        }

		// GET: Salas/Delete/5
		[CustomAuthorize(SGC_AccessCode = "room_delete")]
		public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            salas salas = await db.salas.FindAsync(id);
            if (salas == null)
            {
                return HttpNotFound();
            }
            return View(salas);
        }

        // POST: Salas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            salas salas = await db.salas.FindAsync(id);

			salas.deleted_at = DateTimeOffset.Now.DateTime;
			
			//db.salas.Remove(salas);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

		public async Task<ActionResult> Restore(int id)
		{
			salas salas = await db.salas.FindAsync(id);

			salas.deleted_at = null;

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
