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
    public class ProspectosController : Controller
    {
        private _SGCModel db = new _SGCModel();

		// GET: Prospectos
		[CustomAuthorize(SGC_AccessCode = "ppts_index")]
		public async Task<ActionResult> Index()
        {
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("ppts"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			return View(await db.prospectos.ToListAsync());
        }

		public ActionResult SearchProspect(string Value)
		{
			prospectos p = db.prospectos.Where(m => m.documento_numero == Value).FirstOrDefault();

			if (p != null)
			{
				return RedirectToAction("CreateFromProspecto", "Clientes", new { prospectoId = p.id});
			}
			else
			{
				p = db.prospectos.Where(m => m.email_personal == Value).FirstOrDefault();
				if (p != null)
				{
					return RedirectToAction("CreateFromProspecto", "Clientes", new { prospectoId = p.id });
				}
				else
				{
					return RedirectToAction("Create", "Clientes");
				}
			}
		}

		// GET: Prospectos/Details/5
		[CustomAuthorize(SGC_AccessCode = "ppts_details")]
		public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            prospectos prospectos = await db.prospectos.FindAsync(id);
            if (prospectos == null)
            {
                return HttpNotFound();
            }
            return View(prospectos);
        }

		// GET: Prospectos/Create
		[CustomAuthorize(SGC_AccessCode = "ppts_create")]
		public ActionResult Create()
        {
            return View();
        }

        // POST: Prospectos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,nombres,apellidos,genero,fecha_nacimiento,documento_tipo,telefono,documento_numero,email_personal,centros_laboral,cargo_laboral,email_empresa,created_at,updated_at,deleted_at")] prospectos prospectos)
        {
            if (ModelState.IsValid)
            {
				prospectos.created_at = DateTimeOffset.Now.DateTime;
				prospectos.updated_at = DateTimeOffset.Now.DateTime;

                db.prospectos.Add(prospectos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(prospectos);
        }

		//AACC
		[HttpPost]
		public JsonResult CreateProspects(ProspectsReferredByClient rf)
		{
			prospectos p_1 = new prospectos();
			prospectos p_2 = new prospectos();
			p_1.nombres = rf.p1_nombres;
			p_1.apellidos = rf.p1_apellidos;
			p_1.telefono = rf.p1_celular;
			p_1.email_personal = rf.p1_email;
			p_1.created_at = DateTimeOffset.Now.Date;
			p_1.updated_at = DateTimeOffset.Now.Date;

			p_2.nombres = rf.p2_nombres;
			p_2.apellidos = rf.p2_apellidos;
			p_2.telefono = rf.p2_celular;
			p_2.email_personal = rf.p2_email;
			p_2.created_at = DateTimeOffset.Now.Date;
			p_2.updated_at = DateTimeOffset.Now.Date;

			List<prospectos> lstProspects = new List<prospectos>();
			lstProspects.Add(p_1);
			lstProspects.Add(p_2);

			db.prospectos.Add(lstProspects[0]);
			int c = db.SaveChanges();

			Dictionary<string, object> js = new Dictionary<string, object>();

			if (c > 0)
			{				
				js.Add("message", "Se han creado " + c.ToString() + " prospectos.");
				js.Add("status", HttpStatusCode.OK);
				js.Add("p1_id", p_1.id);
				/*js.Add("p2_id", p_2.id);*/

				return Json(js, JsonRequestBehavior.AllowGet);
			}

			js.Add("message", "Falló la inserción de datos.");
			js.Add("status", HttpStatusCode.Conflict);
			

			return Json(js, JsonRequestBehavior.AllowGet);
		}

		// GET: Prospectos/Edit/5
		[CustomAuthorize(SGC_AccessCode = "ppts_update")]
		public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            prospectos prospectos = await db.prospectos.FindAsync(id);
            if (prospectos == null)
            {
                return HttpNotFound();
            }
            return View(prospectos);
        }

        // POST: Prospectos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,nombres,apellidos,genero,fecha_nacimiento,documento_tipo,telefono,documento_numero,email_personal,centros_laboral,cargo_laboral,email_empresa,created_at,updated_at,deleted_at")] prospectos prospectos)
        {
            if (ModelState.IsValid)
            {
				prospectos.updated_at = DateTimeOffset.Now.DateTime;
                db.Entry(prospectos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(prospectos);
        }

		// GET: Prospectos/Delete/5
		[CustomAuthorize(SGC_AccessCode = "ppts_delete")]
		public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            prospectos prospectos = await db.prospectos.FindAsync(id);
            if (prospectos == null)
            {
                return HttpNotFound();
            }
            return View(prospectos);
        }

        // POST: Prospectos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            prospectos prospectos = await db.prospectos.FindAsync(id);
			prospectos.deleted_at = DateTimeOffset.Now.DateTime;
            db.prospectos.Remove(prospectos);
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
