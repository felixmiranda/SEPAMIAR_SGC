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
    public class CongelamientoSemanasController : Controller
    {
        private _SGCModel db = new _SGCModel();

        // GET: CongelamientoSemanas
		[CustomAuthorize(SGC_AccessCode ="frz_index")]
        public async Task<ActionResult> Index()
        {
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("frz"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			return View(await db.congelamiento_semanas.ToListAsync());
        }

		// GET: CongelamientoSemanas/Details/5
		[CustomAuthorize(SGC_AccessCode = "frz_details")]
		public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            congelamiento_semanas congelamiento_semanas = await db.congelamiento_semanas.FindAsync(id);
            if (congelamiento_semanas == null)
            {
                return HttpNotFound();
            }
            return View(congelamiento_semanas);
        }

		// GET: CongelamientoSemanas/Create
		[CustomAuthorize(SGC_AccessCode = "frz_create")]
		public ActionResult Create()
        {
            return View();
        }

        // POST: CongelamientoSemanas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,desde,hasta,cantidad_dias,created_at,updated_at,deleted_at")] congelamiento_semanas congelamiento_semanas)
        {
            if (ModelState.IsValid)
            {
				congelamiento_semanas.created_at = DateTimeOffset.Now.DateTime;
				congelamiento_semanas.updated_at = DateTimeOffset.Now.DateTime;

                db.congelamiento_semanas.Add(congelamiento_semanas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(congelamiento_semanas);
        }

		// GET: CongelamientoSemanas/Edit/5
		[CustomAuthorize(SGC_AccessCode = "frz_update")]
		public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            congelamiento_semanas congelamiento_semanas = await db.congelamiento_semanas.FindAsync(id);
            if (congelamiento_semanas == null)
            {
                return HttpNotFound();
            }
            return View(congelamiento_semanas);
        }

        // POST: CongelamientoSemanas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,desde,hasta,cantidad_dias,created_at,updated_at,deleted_at")] congelamiento_semanas congelamiento_semanas)
        {
            if (ModelState.IsValid)
            {
				congelamiento_semanas.updated_at = DateTimeOffset.Now.DateTime;

                db.Entry(congelamiento_semanas).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(congelamiento_semanas);
        }

		// GET: CongelamientoSemanas/Delete/5
		[CustomAuthorize(SGC_AccessCode = "frz_delete")]
		public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            congelamiento_semanas congelamiento_semanas = await db.congelamiento_semanas.FindAsync(id);
            if (congelamiento_semanas == null)
            {
                return HttpNotFound();
            }
            return View(congelamiento_semanas);
        }

        // POST: CongelamientoSemanas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            congelamiento_semanas congelamiento_semanas = await db.congelamiento_semanas.FindAsync(id);

			congelamiento_semanas.deleted_at = DateTimeOffset.Now.DateTime;

            //db.congelamiento_semanas.Remove(congelamiento_semanas);
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
