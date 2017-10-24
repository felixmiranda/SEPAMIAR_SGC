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
    public class SolicitudPermisosController : Controller
    {
        private _SGCModel db = new _SGCModel();

        // GET: SolicitudPermisos
        public async Task<ActionResult> Index()
        {
            var solicitud_permisos = db.solicitud_permisos.Include(s => s.jefe).Include(s => s.solicitante);
            return View(await solicitud_permisos.ToListAsync());
        }

        // GET: SolicitudPermisos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitud_permisos solicitud_permisos = await db.solicitud_permisos.FindAsync(id);
            if (solicitud_permisos == null)
            {
                return HttpNotFound();
            }
            return View(solicitud_permisos);
        }

		public async Task<ActionResult> GetNotifications()
		{
			int id = (int)Session["UserId"];
			int notAttended = 0;
			string html = "";
			List<solicitud_permisos> notifications = await db.solicitud_permisos.Where(m => m.usuario_jefe == id).OrderBy(m => m.autorizado).ToListAsync();
			Dictionary<string, object> js = new Dictionary<string, object>();

			foreach (solicitud_permisos sp in notifications)
			{
				if (!sp.autorizado) notAttended++;

				html += "<div class=\"notification\">";
				html += "<div class=\"notification-title\">" + sp.solicitante.nombres + " " + sp.solicitante.apellidos + "</div>";
				html += "<p>Ha solicitado un cambio de " + sp.valor_defecto + " hacia " + sp.valor_modificado + "</p>";
				html += "<div class=\"notification-actions\">";
				html += "<a href=\"#\">Aprobado</a>";
				html += "<a href=\"#\">No Aprobado</a>";
				html += "</div>";
				html += "</div>";
            }

			js.Add("notAttended", notAttended);
			js.Add("html", html);

			return Json(js, JsonRequestBehavior.AllowGet);
		}

		// GET: SolicitudPermisos/Create
		public ActionResult Create()
        {
            ViewBag.usuario_jefe = new SelectList(db.usuarios, "id", "codigo");
            ViewBag.usuario_solicitante = new SelectList(db.usuarios, "id", "codigo");
            return View();
        }

        // POST: SolicitudPermisos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,usuario_jefe,usuario_solicitante,modelo_nombre,modelo_id,autorizado,created_at,updated_at,deleted_at")] solicitud_permisos solicitud_permisos)
        {
            if (ModelState.IsValid)
            {
                db.solicitud_permisos.Add(solicitud_permisos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.usuario_jefe = new SelectList(db.usuarios, "id", "codigo", solicitud_permisos.usuario_jefe);
            ViewBag.usuario_solicitante = new SelectList(db.usuarios, "id", "codigo", solicitud_permisos.usuario_solicitante);
            return View(solicitud_permisos);
        }

        // GET: SolicitudPermisos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitud_permisos solicitud_permisos = await db.solicitud_permisos.FindAsync(id);
            if (solicitud_permisos == null)
            {
                return HttpNotFound();
            }
            ViewBag.usuario_jefe = new SelectList(db.usuarios, "id", "codigo", solicitud_permisos.usuario_jefe);
            ViewBag.usuario_solicitante = new SelectList(db.usuarios, "id", "codigo", solicitud_permisos.usuario_solicitante);
            return View(solicitud_permisos);
        }

        // POST: SolicitudPermisos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,usuario_jefe,usuario_solicitante,modelo_nombre,modelo_id,autorizado,created_at,updated_at,deleted_at")] solicitud_permisos solicitud_permisos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitud_permisos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.usuario_jefe = new SelectList(db.usuarios, "id", "codigo", solicitud_permisos.usuario_jefe);
            ViewBag.usuario_solicitante = new SelectList(db.usuarios, "id", "codigo", solicitud_permisos.usuario_solicitante);
            return View(solicitud_permisos);
        }

        // GET: SolicitudPermisos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            solicitud_permisos solicitud_permisos = await db.solicitud_permisos.FindAsync(id);
            if (solicitud_permisos == null)
            {
                return HttpNotFound();
            }
            return View(solicitud_permisos);
        }

        // POST: SolicitudPermisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            solicitud_permisos solicitud_permisos = await db.solicitud_permisos.FindAsync(id);
            db.solicitud_permisos.Remove(solicitud_permisos);
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
