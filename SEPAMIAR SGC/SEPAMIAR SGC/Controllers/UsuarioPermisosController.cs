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
	public class UsuarioPermisosController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: UsuarioPermisos
		[CustomAuthorize(SGC_AccessCode = "useraccess_index")]
		public async Task<ActionResult> Index(int v)
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("useraccess"))
				.ToList();

			ViewBag.Permisos = userAccesses;
			ViewBag.userId = v;

			var usuario_permisos = db.usuario_permisos.Include(u => u.permisos).Include(u => u.usuarios).Where(m => m.usuario_id == v).OrderBy(m => m.permisos.nombre);
			return View(await usuario_permisos.ToListAsync());
		}

		public ActionResult GetUserPasses(string value)
		{
			int uId = db.usuarios.Where(m => m.email == value).Select(m => m.id).FirstOrDefault();

			if (uId > 0)
			{
				return RedirectToAction("Index", new { v = uId });
			}

			Dictionary<string, object> jDict = new Dictionary<string, object>();
			jDict.Add("Message", "No se encontró al usuario.");

			return Json(jDict, JsonRequestBehavior.AllowGet);
		}

		// GET: UsuarioPermisos/Details/5
		[CustomAuthorize(SGC_AccessCode = "useraccess_details")]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			usuario_permisos usuario_permisos = await db.usuario_permisos.FindAsync(id);
			if (usuario_permisos == null)
			{
				return HttpNotFound();
			}
			return View(usuario_permisos);
		}

		// GET: UsuarioPermisos/Create
		[CustomAuthorize(SGC_AccessCode = "useraccess_create")]
		public ActionResult Create(int v)
		{
			ViewBag.permiso_id = new SelectList(db.permisos.OrderBy(p=>p.nombre), "id", "nombre");

			ViewBag.UserInfo = db.usuarios.Where(m => m.id == v).FirstOrDefault();

			return View();
		}

		// POST: UsuarioPermisos/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "id,usuario_id,permiso_id,created_at,updated_at,deleted_at")] usuario_permisos usuario_permisos)
		{
			usuario_permisos.created_at = DateTimeOffset.Now.Date;
			usuario_permisos.updated_at = DateTimeOffset.Now.Date;

			if (ModelState.IsValid)
			{
				db.usuario_permisos.Add(usuario_permisos);
				await db.SaveChangesAsync();
				return RedirectToAction("Index", new { v = usuario_permisos.usuario_id });
			}

			ViewBag.permiso_id = new SelectList(db.permisos, "id", "nombre", usuario_permisos.permiso_id);
			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", usuario_permisos.usuario_id);
			return View(usuario_permisos);
		}

		// GET: UsuarioPermisos/Edit/5
		[CustomAuthorize(SGC_AccessCode = "useraccess_edit")]
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			usuario_permisos usuario_permisos = await db.usuario_permisos.FindAsync(id);
			if (usuario_permisos == null)
			{
				return HttpNotFound();
			}
			ViewBag.permiso_id = new SelectList(db.permisos, "id", "nombre", usuario_permisos.permiso_id);
			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", usuario_permisos.usuario_id);
			return View(usuario_permisos);
		}

		// POST: UsuarioPermisos/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id,usuario_id,permiso_id,created_at,updated_at,deleted_at")] usuario_permisos usuario_permisos)
		{
			if (ModelState.IsValid)
			{
				usuario_permisos.updated_at = DateTimeOffset.Now.DateTime;

				db.Entry(usuario_permisos).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index", new { v = usuario_permisos.usuario_id });
			}
			ViewBag.permiso_id = new SelectList(db.permisos, "id", "nombre", usuario_permisos.permiso_id);
			ViewBag.usuario_id = new SelectList(db.usuarios, "id", "codigo", usuario_permisos.usuario_id);
			return View(usuario_permisos);
		}

		// GET: UsuarioPermisos/Delete/5
		[CustomAuthorize(SGC_AccessCode = "useraccess_delete")]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			usuario_permisos usuario_permisos = await db.usuario_permisos.FindAsync(id);
			if (usuario_permisos == null)
			{
				return HttpNotFound();
			}
			return View(usuario_permisos);
		}

		// POST: UsuarioPermisos/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			usuario_permisos usuario_permisos = await db.usuario_permisos.FindAsync(id);
			//db.usuario_permisos.Remove(usuario_permisos);
			usuario_permisos.deleted_at = DateTimeOffset.Now.DateTime;
			await db.SaveChangesAsync();
			return RedirectToAction("Index", new { v = usuario_permisos.usuario_id });
		}

		public async Task<ActionResult> Restore(int id)
		{
			usuario_permisos usuario_permisos = await db.usuario_permisos.FindAsync(id);
			//db.usuario_permisos.Remove(usuario_permisos);
			usuario_permisos.deleted_at = null;
			await db.SaveChangesAsync();
			return RedirectToAction("Index", new { v = usuario_permisos.usuario_id });
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
