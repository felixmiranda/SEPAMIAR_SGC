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
    public class LaboratorioResultadosController : Controller
    {
        private _SGCModel db = new _SGCModel();

        // GET: LaboratorioResultados
        public async Task<ActionResult> Index(int clientId)
        {
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("labs"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			var table = db.fichas_medicas.Where(m => m.cliente_id == clientId).Include(m => m.laboratorio).OrderByDescending(m => m.created_at).Select(m=>m.laboratorio);

			clientes cl = db.clientes.Where(m => m.id_alt == clientId).FirstOrDefault();
			ViewBag.clientName = cl.nombres;
			ViewBag.clientId = clientId;

			return View(await table.FirstOrDefaultAsync());
        }

		public ActionResult SearchClientLabResults(int Value)
		{
			string clientCode = Value.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			if (c > 0)
			{
				return RedirectToAction("Index", new { clientId = c });
			}
			else
			{
				return null;
			}
		}

        // GET: LaboratorioResultados/Create
        public ActionResult Create(int clientId)
        {
			ViewBag.clientId = clientId;

			return View();
        }

        // POST: LaboratorioResultados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int clientId, LabResults labResults)
        {
            if (ModelState.IsValid)
            {
				fichas_medicas fm = db.fichas_medicas.Where(m => m.cliente_id == clientId).Include(m => m.laboratorio).OrderByDescending(m => m.created_at).FirstOrDefault();
				fm.laboratorio.Add(labResults);
				fm.updated_at = Utilities.CurrentDate.getNow();

				db.Entry(fm).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index",new { clientId = clientId });
            }

            return View(labResults);
        }

        // GET: LaboratorioResultados/Edit/5
        public async Task<ActionResult> Edit(int? id, int clientId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabResults labResults = await db.LabResults.FindAsync(id);
			ViewBag.clientId = clientId;
			if (labResults == null)
            {
                return HttpNotFound();
            }
            return View(labResults);
        }

        // POST: LaboratorioResultados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int clientId, LabResults labResults)
        {
            if (ModelState.IsValid)
            {
                db.Entry(labResults).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { clientId = clientId });
			}
            return View(labResults);
        }

        //// GET: LaboratorioResultados/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LabResults labResults = await db.LabResults.FindAsync(id);
        //    if (labResults == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(labResults);
        //}

        //// POST: LaboratorioResultados/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    LabResults labResults = await db.LabResults.FindAsync(id);
        //    db.LabResults.Remove(labResults);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

		// GET: LaboratorioResultados/Details/5
		//public async Task<ActionResult> Details(int? id)
		//      {
		//          if (id == null)
		//          {
		//              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//          }
		//          LabResults labResults = await db.LabResults.FindAsync(id);
		//          if (labResults == null)
		//          {
		//              return HttpNotFound();
		//          }
		//          return View(labResults);
		//      }
	}
}
