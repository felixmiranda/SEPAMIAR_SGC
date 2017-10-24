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
using System.Web.Helpers;
using SEPAMIAR_SGC.Utilities;

namespace SEPAMIAR_SGC.Controllers
{
    public class ClientesNotasController : Controller
    {
        private _SGCModel db = new _SGCModel();

        // GET: ClientesNotas
        public async Task<ActionResult> Index()
        {
            var clientes_notas = db.clientes_notas.Include(c => c.clientes);
            return View(await clientes_notas.ToListAsync());
        }

		

        // GET: ClientesNotas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clientes_notas clientes_notas = await db.clientes_notas.FindAsync(id);
            if (clientes_notas == null)
            {
                return HttpNotFound();
            }
            return View(clientes_notas);
        }

        // GET: ClientesNotas/Create
        public ActionResult Create()
        {
            ViewBag.idCliente = new SelectList(db.clientes, "id_alt", "codigo");
            return View();
        }

		public JsonResult AddNota(Nota data)
		{
			clientes_notas nota = new clientes_notas();
			Dictionary<string, object> js = new Dictionary<string, object>();
			string html = "";


			nota.usuarioId = (int)Session["UserId"];
			nota.idCliente = data.cliente_id;
			nota.nota = data.nota;
			nota.created_at = DateTimeOffset.Now.DateTime;
			nota.updated_at = DateTimeOffset.Now.DateTime;
			nota.usuarios = db.usuarios.Where(u => u.id == nota.usuarioId).FirstOrDefault();

			db.clientes_notas.Add(nota);
			int results = db.SaveChanges();

			if (results > 0)
			{
				html += "<div class='client-notes-item'>";
				html += "<img src='" + nota.usuarios.getFoto() + "' class='img-circle pull-left' width='60' alt='' />";
				html += "<div class='pull-right'>" + nota.created_at + "</div>";
				html += "<h3>" + nota.usuarios.nombres + " " + nota.usuarios.apellidos + "</h3>";
				html += "<p>" + nota.nota + "</p>";
				html += "</div>";

                js.Add("state", "success");
				js.Add("html", html);
			}
			else
			{
				js.Add("state", "error");
				js.Add("message", "Error en la inserción");
			}

			return Json(js, JsonRequestBehavior.AllowGet);
		}

        // POST: ClientesNotas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,idCliente,nota")] clientes_notas clientes_notas)
        {
            if (ModelState.IsValid)
            {
                db.clientes_notas.Add(clientes_notas);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.idCliente = new SelectList(db.clientes, "id_alt", "codigo", clientes_notas.idCliente);
            return View(clientes_notas);
        }

        // GET: ClientesNotas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clientes_notas clientes_notas = await db.clientes_notas.FindAsync(id);
            if (clientes_notas == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCliente = new SelectList(db.clientes, "id_alt", "codigo", clientes_notas.idCliente);
            return View(clientes_notas);
        }

        // POST: ClientesNotas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,idCliente,nota")] clientes_notas clientes_notas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientes_notas).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.clientes, "id_alt", "codigo", clientes_notas.idCliente);
            return View(clientes_notas);
        }

        // GET: ClientesNotas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clientes_notas clientes_notas = await db.clientes_notas.FindAsync(id);
            if (clientes_notas == null)
            {
                return HttpNotFound();
            }
            return View(clientes_notas);
        }

        // POST: ClientesNotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            clientes_notas clientes_notas = await db.clientes_notas.FindAsync(id);
            db.clientes_notas.Remove(clientes_notas);
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
