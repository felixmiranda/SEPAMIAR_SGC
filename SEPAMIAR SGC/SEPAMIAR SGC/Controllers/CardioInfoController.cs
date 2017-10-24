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
	public class CardioInfoController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: CardioInfo
		public async Task<ActionResult> Index(int clientId)
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("cardio"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			var table = db.fichas_medicas.Where(m => m.cliente_id == clientId).Include(m => m.cardio).OrderByDescending(m => m.created_at).Select(m => m.cardio);

			clientes cl = db.clientes.Where(m => m.id_alt == clientId).FirstOrDefault();
			ViewBag.clientName = cl.nombres;
			ViewBag.clientId = clientId;

			return View(await table.FirstOrDefaultAsync());
		}

		public ActionResult SearchClientCardioResults(int Value)
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

		// GET: CardioInfo/Create
		public ActionResult Create(int clientId)
		{
			ViewBag.clientId = clientId;
			return View();
		}

		// POST: CardioInfo/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create( int clientId, CardioInfo cardioInfo)
		{
			if (ModelState.IsValid)
			{
				fichas_medicas fm = db.fichas_medicas.Where(m => m.cliente_id == clientId).Include(m => m.cardio).OrderByDescending(m => m.created_at).FirstOrDefault();
				fm.cardio.Add(cardioInfo);
				fm.updated_at = Utilities.CurrentDate.getNow();

				db.Entry(fm).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index", new { clientId = clientId });
			}

			return View(cardioInfo);
		}

		// GET: CardioInfo/Edit/5
		public async Task<ActionResult> Edit(int? id, int clientId)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			CardioInfo cardioInfo = await db.CardioInfo.FindAsync(id);
			ViewBag.clientId = clientId;
			if (cardioInfo == null)
			{
				return HttpNotFound();
			}
			return View(cardioInfo);
		}

		// POST: CardioInfo/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int clientId, CardioInfo cardioInfo)
		{
			if (ModelState.IsValid)
			{
				db.Entry(cardioInfo).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index", new { clientId = clientId });
			}
			return View(cardioInfo);
		}

		//// GET: CardioInfo/Details/5
		//public async Task<ActionResult> Details(int? id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	CardioInfo cardioInfo = await db.CardioInfo.FindAsync(id);
		//	if (cardioInfo == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(cardioInfo);
		//}

		//// GET: CardioInfo/Delete/5
		//public async Task<ActionResult> Delete(int? id)
		//      {
		//          if (id == null)
		//          {
		//              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//          }
		//          CardioInfo cardioInfo = await db.CardioInfo.FindAsync(id);
		//          if (cardioInfo == null)
		//          {
		//              return HttpNotFound();
		//          }
		//          return View(cardioInfo);
		//      }

		//      // POST: CardioInfo/Delete/5
		//      [HttpPost, ActionName("Delete")]
		//      [ValidateAntiForgeryToken]
		//      public async Task<ActionResult> DeleteConfirmed(int id)
		//      {
		//          CardioInfo cardioInfo = await db.CardioInfo.FindAsync(id);
		//          db.CardioInfo.Remove(cardioInfo);
		//          await db.SaveChangesAsync();
		//          return RedirectToAction("Index");
		//      }

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
