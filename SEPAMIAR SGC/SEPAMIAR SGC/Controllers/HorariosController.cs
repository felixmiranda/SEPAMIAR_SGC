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
    public class HorariosController : Controller
    {
        private _SGCModel db = new _SGCModel();

        // GET: Horarios
		[CustomAuthorize(SGC_AccessCode ="sch_index")]
        public async Task<ActionResult> Index()
        {
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("sch"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			var horarios = db.horarios.Include(h => h.programa).Include(h => h.sala);
            return View(await horarios.ToListAsync());
        }

		// GET: Horarios/Details/5
		[CustomAuthorize(SGC_AccessCode = "sch_details")]
		public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            horarios horarios = await db.horarios.FindAsync(id);
            if (horarios == null)
            {
                return HttpNotFound();
            }
            return View(horarios);
        }

		// GET: Horarios/Create
		[CustomAuthorize(SGC_AccessCode = "sch_create")]
		public ActionResult Create()
        {
            ViewBag.programa_id = new SelectList(db.programas, "id", "nombre");
            ViewBag.sala_id = new SelectList(db.salas, "id", "nombre");
            return View();
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,sala_id,programa_id,lunes,martes,miercoles,jueves,viernes,sabado,domingo,hora,created_at,updated_at,deleted_at")] horarios horarios)
        {
            if (ModelState.IsValid)
            {
                db.horarios.Add(horarios);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.programa_id = new SelectList(db.programas, "id", "nombre", horarios.programa_id);
            ViewBag.sala_id = new SelectList(db.salas, "id", "nombre", horarios.sala_id);
            return View(horarios);
        }

		// GET: Horarios/Edit/5
		[CustomAuthorize(SGC_AccessCode = "sch_update")]
		public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            horarios horarios = await db.horarios.FindAsync(id);
            if (horarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.programa_id = new SelectList(db.programas, "id", "nombre", horarios.programa_id);
            ViewBag.sala_id = new SelectList(db.salas, "id", "nombre", horarios.sala_id);
            return View(horarios);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,sala_id,programa_id,lunes,martes,miercoles,jueves,viernes,sabado,domingo,hora,created_at,updated_at,deleted_at")] horarios horarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(horarios).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.programa_id = new SelectList(db.programas, "id", "nombre", horarios.programa_id);
            ViewBag.sala_id = new SelectList(db.salas, "id", "nombre", horarios.sala_id);
            return View(horarios);
        }

		// GET: Horarios/Delete/5
		[CustomAuthorize(SGC_AccessCode = "sch_delete")]
		public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            horarios horarios = await db.horarios.FindAsync(id);
            if (horarios == null)
            {
                return HttpNotFound();
            }
            return View(horarios);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            horarios horarios = await db.horarios.FindAsync(id);
            db.horarios.Remove(horarios);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

		[HttpGet]
		public JsonResult SearchByPrograma(int programa_id, int local_id)
		{
			Dictionary<string, object> js = new Dictionary<string, object>();
			List<horarios> horarios = db.horarios.Where(h => h.programa_id == programa_id && h.sala.local.id == local_id).ToList();
			string html = "";

			if (horarios.Count > 0)
			{
				foreach (horarios horario in horarios)
				{
					html += "<option value='" + horario.id + "'>" + horario.hora + "</option>";
				}

				js.Add("state", "success");
				js.Add("html", html);
			}
			else
			{
				js.Add("state", "error");
				js.Add("message", "No hay horarios asignados a este programa");
			}

			return Json(js, JsonRequestBehavior.AllowGet);
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
