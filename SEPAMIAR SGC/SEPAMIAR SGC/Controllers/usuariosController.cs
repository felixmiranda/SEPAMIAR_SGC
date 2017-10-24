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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SEPAMIAR_SGC.Utilities;
using SEPAMIAR_SGC.Seguridad;

namespace SEPAMIAR_SGC.Controllers
{
	public class UsuariosController : Controller
	{
		private _SGCModel db = new _SGCModel();
		
		// GET: usuarios
		[CustomAuthorize(SGC_AccessCode = "usr_index")]
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("usr"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			var usuarios = db.usuarios.Include(u => u.nutricionistas).Include(u => u.vendedores);
			return View(await usuarios.ToListAsync());
		}

		// GET: usuarios/Details/5
		[CustomAuthorize(SGC_AccessCode = "usr_details")]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			usuarios usuarios = await db.usuarios.FindAsync(id);
			if (usuarios == null)
			{
				return HttpNotFound();
			}
			return View(usuarios);
		}

		// GET: usuarios/Create
		[CustomAuthorize(SGC_AccessCode = "usr_create")]
		public ActionResult Create()
		{
			ViewBag.localsId = new SelectList(db.locales, "id", "nombre");
			return View();
		}

		// POST: usuarios/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(usuarios usuario,
			HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
				DateTime today = CurrentDate.getNow();
				usuario.created_at = today;
				usuario.updated_at = today;
				usuario.activo = true;

				if (file != null)
				{
					byte[] bNonNull = new byte[file.ContentLength];

					Image img = Image.FromStream(file.InputStream, true, true);
					ImageCodecInfo pngInfo = ImageCodecInfo.GetImageEncoders().Where(codecInfo => codecInfo.MimeType == "image/png").First();

					using (Stream s = new MemoryStream())
					{
						using (EncoderParameters encParams = new EncoderParameters(1))
						{
							encParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionLZW);
							//quality should be in the range [0..100]
							img.Save(s, pngInfo, encParams);
						}
						s.Position = 0;
						s.Read(bNonNull, 0, (int)s.Length);
					}
					usuario.foto = Convert.ToBase64String(bNonNull);
				}

				usuario.deleted_at = null;

				db.usuarios.Add(usuario);
				int i = await db.SaveChangesAsync();

				if (i > 0)
				{
					usuarios createdUser = db.usuarios.Where(u => u.codigo == usuario.codigo).FirstOrDefault();

					usuario_permisos usrAccesses = new usuario_permisos();
					usrAccesses.usuario_id = createdUser.id;
					usrAccesses.created_at = today;
					usrAccesses.updated_at = today;

					switch (createdUser.tipo)
					{
						case UserProfiles.Default:
							goto default;
						case UserProfiles.Nutricionista:
							usrAccesses.permiso_id = db.permisos.Where(p => p.codigo_interno == "nut_home").Select(p => p.id).FirstOrDefault();
							break;
						case UserProfiles.Vendedor:
							usrAccesses.permiso_id = db.permisos.Where(p => p.codigo_interno == "sales_home").Select(p => p.id).FirstOrDefault();
							break;
						case UserProfiles.Entrenador:
							usrAccesses.permiso_id = db.permisos.Where(p => p.codigo_interno == "training_home").Select(p => p.id).FirstOrDefault();
							break;
						case UserProfiles.Cajero:
							goto default;
						case UserProfiles.Sistemas:
							usrAccesses.permiso_id = db.permisos.Where(p => p.codigo_interno == "sys_home").Select(p => p.id).FirstOrDefault();
							break;
						case UserProfiles.Pesos_y_Medidas:
							usrAccesses.permiso_id = db.permisos.Where(p => p.codigo_interno == "pym_home").Select(p => p.id).FirstOrDefault();
							break;
						case UserProfiles.Cardiología:
							goto default;
						case UserProfiles.Marketing:
							usrAccesses.permiso_id = db.permisos.Where(p => p.codigo_interno == "mkt_home").Select(p => p.id).FirstOrDefault();
							break;
						default:
							break;
					}

					db.usuario_permisos.Add(usrAccesses);

					i = await db.SaveChangesAsync();

					if (i > 0)
					{
						List<string> mails = new List<string>();
						mails.Add(usuario.email);
						string url = Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority;
						string body =
							"Estimado " + usuario.nombres + " " + usuario.apellidos +
							"\n \n" +
							"Se le ha creado un usuario para el sistema SGC de Personal Training, sus credenciales son las siguientes: \n \n" +
							"Usuario: " + usuario.email + "\n" +
							"Clave:" + usuario.password + "\n \n" +
							"Para ingresar al sistema, ingresar al siguiente enlace o copiarlo en su browser: " + url;
						mails.Add("andres@nameless.pe"); //solo para demo.
						Mail.Send(
							pTo: mails,
							pSubject: "Se le ha asignado un usuario en SGC",
							pBody: body);

						return RedirectToAction("Index");
					}
				}
				return View(usuario);
			}
			else
			{
				return View(usuario);
			}
		}

		// GET: usuarios/Edit/5
		[CustomAuthorize(SGC_AccessCode = "usr_update")]
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			usuarios usuarios = await db.usuarios.FindAsync(id);
			if (usuarios == null)
			{
				return HttpNotFound();
			}
			ViewBag.id = new SelectList(db.clientes, "usuario_id", "genero", usuarios.id);
			ViewBag.id = new SelectList(db.nutricionistas, "usuario_id", "usuario_id", usuarios.id);
			ViewBag.id = new SelectList(db.vendedores, "usuario_id", "usuario_id", usuarios.id);
			ViewBag.localsId = new SelectList(db.locales, "id", "nombre", usuarios.localesId);
			return View(usuarios);
		}

		// POST: usuarios/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Edit(usuarios usuario, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
				usuarios u = db.usuarios.Find(usuario.id);
				u.nombres = usuario.nombres;
				u.apellidos = usuario.apellidos;
				u.email = usuario.email;
				u.password = usuario.password;
				u.tipo = usuario.tipo;
				u.activo = usuario.activo;
				u.localesId = int.Parse(Request.Form["locales_id"]);
				u.updated_at = DateTimeOffset.Now.DateTime;

				if (file != null)
				{
					byte?[] bNull = new byte?[file.ContentLength];
					byte[] bNonNull = new byte[file.ContentLength];

					Image img = Image.FromStream(file.InputStream, true, true);
					ImageCodecInfo pngInfo = ImageCodecInfo.GetImageEncoders().Where(codecInfo => codecInfo.MimeType == "image/png").First();

					using (Stream s = new MemoryStream())
					{
						using (EncoderParameters encParams = new EncoderParameters(1))
						{
							encParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionLZW);
							//quality should be in the range [0..100]
							img.Save(s, pngInfo, encParams);
						}
						s.Position = 0;
						s.Read(bNonNull, 0, (int)s.Length);
					}
					u.foto = Convert.ToBase64String(bNonNull);
				}

				if (SessionPersister.EmailUsuario == u.email)
				{
					SessionPersister.AreaCliente = u.tipo;
					SessionPersister.ImgBase64 = u.foto;
				}

				db.Entry(u).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			/*ViewBag.id = new SelectList(db.clientes, "usuario_id", "genero", usuario.id);
            ViewBag.id = new SelectList(db.nutricionistas, "usuario_id", "usuario_id", usuario.id);
            ViewBag.id = new SelectList(db.vendedores, "usuario_id", "usuario_id", usuario.id);*/
			return View(usuario);
		}

		// GET: usuarios/Delete/5
		[CustomAuthorize(SGC_AccessCode = "usr_delete")]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			usuarios usuarios = await db.usuarios.FindAsync(id);
			if (usuarios == null)
			{
				return HttpNotFound();
			}
			return View(usuarios);
		}

		// POST: usuarios/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			usuarios usuario = await db.usuarios.FindAsync(id);
			usuario.deleted_at = DateTimeOffset.Now.DateTime;
			usuario.activo = false;
			//db.usuarios.Remove(usuario);

			db.Entry(usuario).State = EntityState.Modified;
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		// GET: usuarios/Restore/5
		[ActionName("Restore")]
		public async Task<ActionResult> Restore(int id)
		{
			usuarios usuario = await db.usuarios.FindAsync(id);
			usuario.deleted_at = null;
			usuario.activo = true;
			db.Entry(usuario).State = EntityState.Modified;
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
