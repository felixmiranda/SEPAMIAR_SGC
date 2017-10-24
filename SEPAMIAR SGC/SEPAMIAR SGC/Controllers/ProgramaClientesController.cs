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
	public class ProgramaClientesController : Controller
	{
		private _SGCModel db = new _SGCModel();
		private static string ErrorMessage;

		// GET: ProgramaClientes
		public async Task<ActionResult> Index()
		{
			var programa_usuarios = db.programa_clientes.Include(p => p.cliente).Include(p => p.horario).Include(p => p.programa);
			return View(await programa_usuarios.ToListAsync());
		}

		// GET: ProgramaClientes/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			programa_clientes programa_clientes = await db.programa_clientes.FindAsync(id);
			if (programa_clientes == null)
			{
				return HttpNotFound();
			}
			return View(programa_clientes);
		}

		// GET: ProgramaClientes/Create
		public ActionResult Create(int clienteId, DateTime feInit)
		{
			if (ErrorMessage != null)
			{
				ViewBag.ErrorMessage = ErrorMessage;
			}

			// AACC
			// Carga el cliente para que pueda mostrar el nombre en la vista y pueda proceder a asignar el programa.

			clientes cliente = db.clientes.Where(m => m.id_alt == clienteId).FirstOrDefault();
			ViewData["clienteData"] = cliente;
			ViewBag.feInit = feInit.ToString("yyyy-MM-dd");

			ViewBag.locales = db.locales.ToList().Select(x => new SelectListItem()
			{
				Value = x.id.ToString(),
				Text = x.nombre
			});
			ViewBag.horario_id = new SelectList(db.horarios, "id", "hora");
			ViewBag.programa_id = new SelectList(db.programas, "id", "nombre");
			return View();
		}

		// POST: ProgramaClientes/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "id,programa_id,cliente_id,horario_id,fecha_inicio,fecha_fin,created_at,updated_at,deleted_at")] programa_clientes programa_clientes, int clientId)
		{

			programa_clientes prg_cl = new programa_clientes();
			prg_cl.cliente_id = clientId;
			prg_cl.horario_id = programa_clientes.horario_id;
			prg_cl.programa_id = programa_clientes.programa_id;
			prg_cl.fecha_inicio = programa_clientes.fecha_inicio;
			prg_cl.fecha_fin = programa_clientes.fecha_fin;
			prg_cl.created_at = DateTimeOffset.Now.DateTime;
			prg_cl.updated_at = DateTimeOffset.Now.DateTime;

			db.programa_clientes.Add(prg_cl);

			try
			{
				db.SaveChanges();

				return RedirectToAction("Create", "VentaUsuarios", new { clientId = clientId, feInit = prg_cl.fecha_inicio });
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;

				return View(clientId);
			}

		}

		// GET: ProgramaClientes/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			programa_clientes programa_clientes = await db.programa_clientes.FindAsync(id);
			if (programa_clientes == null)
			{
				return HttpNotFound();
			}
			ViewBag.cliente_id = new SelectList(db.clientes, "id_alt", "codigo", programa_clientes.cliente_id);
			ViewBag.horario_id = new SelectList(db.horarios, "id", "id", programa_clientes.horario_id);
			ViewBag.programa_id = new SelectList(db.programas, "id", "nombre", programa_clientes.programa_id);
			return View(programa_clientes);
		}

		// POST: ProgramaClientes/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id,programa_id,cliente_id,horario_id,fecha_inicio,fecha_fin,created_at,updated_at,deleted_at")] programa_clientes programa_clientes)
		{
			if (ModelState.IsValid)
			{
				db.Entry(programa_clientes).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.cliente_id = new SelectList(db.clientes, "id_alt", "codigo", programa_clientes.cliente_id);
			ViewBag.horario_id = new SelectList(db.horarios, "id", "id", programa_clientes.horario_id);
			ViewBag.programa_id = new SelectList(db.programas, "id", "nombre", programa_clientes.programa_id);
			return View(programa_clientes);
		}

		// GET: ProgramaClientes/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			programa_clientes programa_clientes = await db.programa_clientes.FindAsync(id);
			if (programa_clientes == null)
			{
				return HttpNotFound();
			}
			return View(programa_clientes);
		}

		// POST: ProgramaClientes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			programa_clientes programa_clientes = await db.programa_clientes.FindAsync(id);
			db.programa_clientes.Remove(programa_clientes);
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
