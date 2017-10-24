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
using SEPAMIAR_SGC.Seguridad;

namespace SEPAMIAR_SGC.Controllers
{
	public class VentaUsuariosController : Controller
	{
		private _SGCModel db = new _SGCModel();
		private BackgroundJobs backJobs = new BackgroundJobs();

		// GET: VentaUsuarios
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("slsusr"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			int? userLocal = db.usuarios.Where(u => u.id == SessionPersister.UserId).Select(u => u.localesId).FirstOrDefault();
			DateTime today = CurrentDate.getToday();
			var venta_usuarios = db.venta_usuarios.Where(v=>v.created_at >= today && v.local_id == userLocal).Include(v => v.cliente).Include(v => v.local).OrderBy(m=>m.atendido).OrderByDescending(m=>m.created_at);
			return View(await venta_usuarios.ToListAsync());
		}

		// GET: VentaUsuarios/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			venta_usuarios venta_usuarios = await db.venta_usuarios.FindAsync(id);
			if (venta_usuarios == null)
			{
				return HttpNotFound();
			}
			return View(venta_usuarios);
		}

		// GET: VentaUsuarios/Create
		public ActionResult Create(int clientId, DateTime feInit)
		{
			clientes cliente = db.clientes.Where(m => m.id_alt == clientId).FirstOrDefault();

			ViewData["clienteData"] = cliente;
			ViewBag.feInit = feInit.ToString("yyyy-MM-dd");
			ViewBag.local_id = new SelectList(db.locales, "id", "nombre");
			ViewBag.semanas_id = new SelectList(db.semanas_precios, "id", "cantidad_semanas");
			ViewBag.campanasMkt_id = new SelectList(db.campana_marketing, "id", "nombre");

			return View();
		}

		// POST: VentaUsuarios/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "id,local_id,cliente_id,vendedor_id,fecha_inicio,fecha_fin,cantidad_semanas,cantidad_dias_congelamiento,semanas_precio_id,solicitud_permiso_congelamiento,tipo_ingreso,campana_marketing_id,numero_boleta,tipo_pago,monto,atendido,created_at,updated_at,deleted_at")] venta_usuarios venta_usuarios, int clientId)
		{
			bool usrIsBoss = db.vendedores.Where(m => m.usuario_id == venta_usuarios.vendedor_id).Select(m => m.jefe).FirstOrDefault() ?? false;

			if (!usrIsBoss && venta_usuarios.solicitud_permiso_congelamiento == true)
			{
				solicitud_permisos sp = new solicitud_permisos();
				sp.usuario_solicitante = venta_usuarios.vendedor_id;
				sp.usuario_jefe = db.vendedores.Where(m => m.jefe == true).Select(m => m.usuario_id).FirstOrDefault();
				sp.valor_modificado = venta_usuarios.cantidad_dias_congelamiento;
				sp.valor_defecto = db.congelamiento_semanas.Where(m => venta_usuarios.cantidad_dias_congelamiento >= m.desde).Where(m => venta_usuarios.cantidad_dias_congelamiento <= m.hasta).Select(m => m.cantidad_dias).FirstOrDefault();
				sp.created_at = DateTimeOffset.Now.Date;
				sp.updated_at = DateTimeOffset.Now.Date;

				await backJobs.AskForPermission(sp: sp);
			}
			venta_usuarios.cliente_id = clientId;

			venta_usuarios.created_at = DateTimeOffset.Now.DateTime;
			venta_usuarios.updated_at = DateTimeOffset.Now.DateTime;

			db.venta_usuarios.Add(venta_usuarios);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		// GET: VentaUsuarios/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			venta_usuarios venta_usuarios = await db.venta_usuarios.FindAsync(id);
			if (venta_usuarios == null)
			{
				return HttpNotFound();
			}
			ViewBag.cliente_id = new SelectList(db.clientes, "id_alt", "codigo", venta_usuarios.cliente_id);
			ViewBag.local_id = new SelectList(db.locales, "id", "nombre", venta_usuarios.local_id);
			return View(venta_usuarios);
		}

		// POST: VentaUsuarios/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id,local_id,cliente_id,vendedor_id,fecha_inicio,fecha_fin,cantidad_semanas,cantidad_dias_congelamiento,solicitud_permiso_congelamiento,tipo_ingreso,campana_marketing_id,numero_boleta,tipo_pago,monto,atendido,created_at,updated_at,deleted_at")] venta_usuarios venta_usuarios)
		{
			if (ModelState.IsValid)
			{
				venta_usuarios vu = db.venta_usuarios.Where(m => m.id == venta_usuarios.id).FirstOrDefault();
				vu.numero_boleta = venta_usuarios.numero_boleta;
				vu.updated_at = DateTimeOffset.Now.DateTime;
				vu.atendido = true;

				clientes cl = db.clientes.Where(m => m.id_alt == venta_usuarios.cliente_id).FirstOrDefault();
				cl.acceso_autorizado = true;
				cl.activo = true;

				db.Entry(cl).State = EntityState.Modified;
				db.Entry(vu).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			ViewBag.cliente_id = new SelectList(db.clientes, "id_alt", "codigo", venta_usuarios.cliente_id);
			ViewBag.local_id = new SelectList(db.locales, "id", "nombre", venta_usuarios.local_id);
			return View(venta_usuarios);
		}

		// GET: VentaUsuarios/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			venta_usuarios venta_usuarios = await db.venta_usuarios.FindAsync(id);
			if (venta_usuarios == null)
			{
				return HttpNotFound();
			}
			return View(venta_usuarios);
		}

		// POST: VentaUsuarios/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			venta_usuarios venta_usuarios = await db.venta_usuarios.FindAsync(id);
			db.venta_usuarios.Remove(venta_usuarios);
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
