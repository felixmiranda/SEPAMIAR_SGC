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
    public class SemanasPreciosController : Controller
    {
        private _SGCModel db = new _SGCModel();

        // GET: SemanasPrecios
        public async Task<ActionResult> Index()
        {
            var semanas_precios = db.semanas_precios.Include(s => s.locales);
            return View(await semanas_precios.ToListAsync());
        }

        // GET: SemanasPrecios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            semanas_precios semanas_precios = await db.semanas_precios.FindAsync(id);
            if (semanas_precios == null)
            {
                return HttpNotFound();
            }
            return View(semanas_precios);
        }

        // GET: SemanasPrecios/Create
        public ActionResult Create()
        {
            ViewBag.localesId = new SelectList(db.locales, "id", "nombre");
            return View();
        }

        // POST: SemanasPrecios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,cantidad_semanas,dias_congelamiento,precio,localesId,created_at,updated_at,deleted_at")] semanas_precios semanas_precios)
        {
            if (ModelState.IsValid)
            {
                db.semanas_precios.Add(semanas_precios);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.localesId = new SelectList(db.locales, "id", "nombre", semanas_precios.localesId);
            return View(semanas_precios);
        }

        // GET: SemanasPrecios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            semanas_precios semanas_precios = await db.semanas_precios.FindAsync(id);
            if (semanas_precios == null)
            {
                return HttpNotFound();
            }
            ViewBag.localesId = new SelectList(db.locales, "id", "nombre", semanas_precios.localesId);
            return View(semanas_precios);
        }

        // POST: SemanasPrecios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,cantidad_semanas,dias_congelamiento,precio,localesId,created_at,updated_at,deleted_at")] semanas_precios semanas_precios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(semanas_precios).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.localesId = new SelectList(db.locales, "id", "nombre", semanas_precios.localesId);
            return View(semanas_precios);
        }

        // GET: SemanasPrecios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            semanas_precios semanas_precios = await db.semanas_precios.FindAsync(id);
            if (semanas_precios == null)
            {
                return HttpNotFound();
            }
            return View(semanas_precios);
        }

        // POST: SemanasPrecios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            semanas_precios semanas_precios = await db.semanas_precios.FindAsync(id);
            db.semanas_precios.Remove(semanas_precios);
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

		public async Task<ActionResult> GetFreezingInfo(int id)
		{
			semanas_precios s = await db.semanas_precios.Where(m => m.id == id).FirstOrDefaultAsync();

			Dictionary<string, object> json = new Dictionary<string, object>();
			json.Add("weeks", s.cantidad_semanas);
			json.Add("days", s.dias_congelamiento);
			json.Add("price", s.precio);

			return Json(json, JsonRequestBehavior.AllowGet);
		}
    }
}
