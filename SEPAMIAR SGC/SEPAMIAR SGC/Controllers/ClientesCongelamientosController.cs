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
using System.Diagnostics;
using CryptoFramework;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace SEPAMIAR_SGC.Controllers
{
	public class ClientesCongelamientosController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: ClientesCongelamientos
		public async Task<ActionResult> Index()
		{
			var clientes_congelamientos = db.clientes_congelamientos.Include(c => c.clientes);
			return View(await clientes_congelamientos.ToListAsync());
		}

		// GET: ClientesCongelamientos/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clientes_congelamientos clientes_congelamientos = await db.clientes_congelamientos.FindAsync(id);
			if (clientes_congelamientos == null)
			{
				return HttpNotFound();
			}
			return View(clientes_congelamientos);
		}

		// GET: ClientesCongelamientos/Create
		public ActionResult Create()
		{
			ViewBag.clienteId = new SelectList(db.clientes, "id_alt", "codigo");
			return View();
		}

		// POST: ClientesCongelamientos/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[System.Web.Mvc.HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "id,clienteId,fecha_desde,fecha_hasta,dias_congelados")] clientes_congelamientos clientes_congelamientos)
		{
			if (ModelState.IsValid)
			{
				db.clientes_congelamientos.Add(clientes_congelamientos);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewBag.clienteId = new SelectList(db.clientes, "id_alt", "codigo", clientes_congelamientos.clienteId);
			return View(clientes_congelamientos);
		}

		// GET: ClientesCongelamientos/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clientes_congelamientos clientes_congelamientos = await db.clientes_congelamientos.FindAsync(id);
			if (clientes_congelamientos == null)
			{
				return HttpNotFound();
			}
			ViewBag.clienteId = new SelectList(db.clientes, "id_alt", "codigo", clientes_congelamientos.clienteId);
			return View(clientes_congelamientos);
		}

		// POST: ClientesCongelamientos/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[System.Web.Mvc.HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id,clienteId,fecha_desde,fecha_hasta,dias_congelados")] clientes_congelamientos clientes_congelamientos)
		{
			if (ModelState.IsValid)
			{
				db.Entry(clientes_congelamientos).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.clienteId = new SelectList(db.clientes, "id_alt", "codigo", clientes_congelamientos.clienteId);
			return View(clientes_congelamientos);
		}

		// GET: ClientesCongelamientos/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clientes_congelamientos clientes_congelamientos = await db.clientes_congelamientos.FindAsync(id);
			if (clientes_congelamientos == null)
			{
				return HttpNotFound();
			}
			return View(clientes_congelamientos);
		}

		// POST: ClientesCongelamientos/Delete/5
		[System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			clientes_congelamientos clientes_congelamientos = await db.clientes_congelamientos.FindAsync(id);
			db.clientes_congelamientos.Remove(clientes_congelamientos);
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

		[System.Web.Mvc.HttpGet]
		public async Task<ActionResult> M_GetAvailableFreezingDays(string c)
		{
			clientes cl = db.clientes.Where(m => m.codigo == c).FirstOrDefault();
			DateTime today = CurrentDate.getNow();
			Mobile oMobile = new Mobile();
			if (cl != null)
			{
				//select * from [dbo].[venta_usuarios] where cliente_id = 11 and fecha_inicio >= '2016-09-29 00:00:00.000' order by fecha_inicio
				venta_usuarios user_sales = await db.venta_usuarios.Where(m => m.cliente_id == cl.id_alt && m.fecha_inicio <= today && m.fecha_fin >= today).FirstOrDefaultAsync();

				Dictionary<string, object> oDict = new Dictionary<string, object>();
				oDict.Add("days", user_sales.cantidad_dias_congelamiento);

				return Json(
							oMobile.GetDictForJSON(
								message: "",
								data: oDict,
								code: MobileResponse.Success),
							JsonRequestBehavior.AllowGet);
			}

			return Json(
				oMobile.GetDictForJSON(
					message: "Error en la solicitud",
					data: null,
					code: MobileResponse.Error),
				JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public async Task<ActionResult> M_NewFreezing(string d)
		{
			Mobile oMobile = new Mobile();
			Crypto oCrypto = new Crypto();

			string decodedString = d.Replace(oldChar: '-', newChar: '+').Replace(oldChar: '_', newChar: '/');

			JavaScriptSerializer oJsonSerializer = new JavaScriptSerializer();
			Dictionary<string, object> oDict = oJsonSerializer.Deserialize<Dictionary<string, object>>(input: oCrypto.DecryptData(pData: decodedString));

			if (oDict["client_code"] as string != "")
			{
				string client_code = oDict["client_code"] as string;
				clientes cl = db.clientes.Where(M => M.codigo == client_code).FirstOrDefault();
				if (cl != null)
				{
					clientes_congelamientos cl_frz = new clientes_congelamientos();
					cl_frz.clienteId = cl.id_alt;
					cl_frz.fecha_desde = DateTime.Parse(oDict["from_date"] as string);
					cl_frz.fecha_hasta = DateTime.Parse(oDict["to_date"] as string);
					cl_frz.dias_congelados = (int)oDict["total_days"];

					db.clientes_congelamientos.Add(cl_frz);
					int savedLines = await db.SaveChangesAsync();

					if (savedLines > 0)
					{
						return Json(
							oMobile.GetDictForJSON(
								message: "Se creó el congelamiento.",
								data: null,
								code: MobileResponse.Success),
							JsonRequestBehavior.AllowGet);
					}
				}
			}

			return Json(
				oMobile.GetDictForJSON(
					message: "No se creó el congelamiento.",
					data: null,
					code: MobileResponse.Error),
				JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public async Task<ActionResult> M_GetClientFreezingDates(string id)
		{
			Mobile oMobile = new Mobile();

			int clientId = db.clientes.Where(c => c.codigo == id).Select(c => c.id_alt).FirstOrDefault();

			if (clientId > 0)
			{
				DateTime startDate = CurrentDate.getToday().AddDays(-30);

				List<clientes_congelamientos> freezingLst = db.clientes_congelamientos.Where(f => f.clienteId == clientId && f.fecha_desde > startDate).ToList();

				if (freezingLst != null)
				{
					Dictionary<string, object> oDict = new Dictionary<string, object>();
					int freezingCount = 0;
					foreach (clientes_congelamientos c_c in freezingLst)
					{
						Dictionary<string, object> dict = new Dictionary<string, object>();
						dict.Add("from", c_c.fecha_desde);
						dict.Add("to", c_c.fecha_hasta);
						dict.Add("days", c_c.dias_congelados);

						oDict.Add("E_" + freezingCount.ToString(), dict);

						freezingCount++;
					}
					return Json(
							oMobile.GetDictForJSON(
								message: "",
								data: oDict,
								code: MobileResponse.Success),
							JsonRequestBehavior.AllowGet);
				}
			}

			return Json(
				oMobile.GetDictForJSON(
					message: "No se pudieron obtener los congelamientos.",
					data: null,
					code: MobileResponse.Error),
				JsonRequestBehavior.AllowGet);
		}
	}


}
