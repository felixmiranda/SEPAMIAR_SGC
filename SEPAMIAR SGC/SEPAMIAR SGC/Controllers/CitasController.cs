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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SEPAMIAR_SGC.Controllers
{
	public class CitasController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: Citas
		public async Task<ActionResult> Index(int? clientCode)
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("appointment"))
				.ToList();

			ViewBag.Permisos = userAccesses;
			IQueryable<citas> citas;

			if (clientCode == null)
			{
				citas = db.citas.Include(c => c.clientes).Include(c => c.nutricionistas).Include(c => c.programas).OrderByDescending(m => m.fecha).Take(15);
				return View(await citas.ToListAsync());
			}

			citas = db.citas.Where(m => m.cliente_id == clientCode).Include(c => c.clientes).Include(c => c.nutricionistas).Include(c => c.programas).OrderByDescending(m => m.fecha).Take(15);
			return View(await citas.ToListAsync());
		}

		public ActionResult GetAppointmentsForClient(int? value)
		{
			if (value == null)
			{
				return RedirectToAction("Index");
			}

			string clientCode = value?.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			return RedirectToAction("Index", new { clientCode = c });
		}

		[HttpGet]
		public async Task<ActionResult> Reschedule(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			citas citas = await db.citas.FindAsync(id);
			if (citas == null)
			{
				return HttpNotFound();
			}
			ViewBag.fecha = citas.fecha.ToShortDateString();

			return View(citas);
		}

		[HttpPost]
		public async Task<ActionResult> Reschedule(citas citas)
		{
			citas c = db.citas.Where(m => m.id == citas.id).FirstOrDefault();

			c.fecha = citas.fecha;
			c.hora = citas.hora;
			c.motivo_reprogramacion = citas.motivo_reprogramacion;
			c.updated_at = CurrentDate.getNow();

			db.Entry(c).State = EntityState.Modified;
			int i = await db.SaveChangesAsync();

			if (i > 0)
			{
				clientes cl = db.clientes.Where(m => m.id_alt == c.cliente_id).FirstOrDefault();

				Mail.Send(cl.GetUserMails(), "Cita nutricional reprogramada",
					"Estimado Cliente," +
					"/n/n" +
					"Su cita nutricional ha sido reprogramada al día " + c.fecha.ToShortDateString() + " a las " + c.hora.ToString("hh':'mm") + ".");

				return RedirectToAction("Index", new { clientCode = c.cliente_id });
			}

			return View(citas);
		}

		// GET: Citas/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			citas citas = await db.citas.FindAsync(id);
			if (citas == null)
			{
				return HttpNotFound();
			}
			return View(citas);
		}

		// GET: Citas/Create
		public ActionResult Create(int clienteId)
		{
			clientes cl = db.clientes.Where(m => m.id_alt == clienteId).FirstOrDefault();
			DateTime today = CurrentDate.getNow();
			programa_clientes prgCl = db.programa_clientes.Where(m => m.cliente_id == cl.id_alt && m.fecha_inicio <= today && m.fecha_fin >= today).FirstOrDefault();

			ViewBag.clientId = cl.id_alt;
			ViewBag.clientCode = cl.codigo;
			ViewBag.clientName = cl.nombres + " " + cl.apellidos;

			ViewBag.programId = prgCl.programa.id;
			ViewBag.programName = prgCl.programa.nombre;
			ViewBag.programInit = prgCl.fecha_inicio.ToShortDateString();
			ViewBag.programEnd = prgCl.fecha_fin.ToShortDateString();

			ViewBag.nutricionista_id = new SelectList(db.nutricionistas, "usuario_id", "usuarios.nombres");
			return View();
		}

		// POST: Citas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(citas citas)
		{
			citas c = new citas();
			c.cliente_id = citas.cliente_id;
			c.fecha = citas.fecha;
			c.hora = citas.hora;
			c.nutricionista_id = citas.nutricionista_id;
			c.programa_id = citas.programa_id;
			c.tipo = citas.tipo;
			c.created_at = CurrentDate.getNow();
			c.updated_at = CurrentDate.getNow();

			db.citas.Add(c);
			int i = await db.SaveChangesAsync();

			if (i > 0)
			{
				clientes cl = db.clientes.Where(m => m.id_alt == citas.cliente_id).FirstOrDefault();

				Mail.Send(cl.GetUserMails(), "Cita nutricional",
					"Estimado Cliente," +
					"/n/n" +
					"Se ha programado su cita nutricional para el día " + c.fecha.ToShortDateString() + " a las " + c.hora.ToString("hh':'mm") + ".");

				return RedirectToAction("Index");
			}

			return RedirectToAction("Create", new { clienteId = citas.cliente_id });
		}

		// GET: Citas/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			citas citas = await db.citas.FindAsync(id);
			if (citas == null)
			{
				return HttpNotFound();
			}
			clientes cl = db.clientes.Where(m => m.id_alt == citas.cliente_id).FirstOrDefault();
			ViewBag.clientId = cl.id_alt;
			ViewBag.clientCode = cl.codigo;
			ViewBag.clientName = cl.nombres + " " + cl.apellidos;

			DateTime today = CurrentDate.getNow();
			programa_clientes prgCl = db.programa_clientes.Where(m => m.cliente_id == cl.id_alt && m.fecha_inicio <= today && m.fecha_fin >= today).FirstOrDefault();
			ViewBag.programId = prgCl.programa.id;
			ViewBag.programName = prgCl.programa.nombre;
			ViewBag.programInit = prgCl.fecha_inicio.ToShortDateString();
			ViewBag.programEnd = prgCl.fecha_fin.ToShortDateString();


			ViewBag.nutricionista_id = new SelectList(db.nutricionistas, "usuario_id", "usuario_id", citas.nutricionista_id);
			return View(citas);
		}

		// POST: Citas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id,cliente_id,nutricionista_id,programa_id,fecha,hora,tipo,created_at,updated_at,deleted_at")] citas citas)
		{
			if (Request.Files[0].ContentLength > 0)
			{
				HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
				
				Image img = Image.FromStream(file.InputStream, true, true);
				ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders().Where(codecInfo => codecInfo.MimeType == "image/jpeg").First();

				using (Stream s = new MemoryStream())
				{
					using (EncoderParameters encParams = new EncoderParameters(1))
					{
						encParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionLZW);
						//quality should be in the range [0..100]
						img.Save(s, jpgInfo, encParams);
					}

					byte[] b = new byte[s.Length];
					s.Position = 0;
					s.Read(b, 0, b.Length);

					citas.foto_avance = Convert.ToBase64String(b);
				}				
			}

			if (ModelState.IsValid)
			{

				citas.updated_at = CurrentDate.getNow();
				db.Entry(citas).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}


			clientes cl = db.clientes.Where(m => m.id_alt == citas.cliente_id).FirstOrDefault();
			ViewBag.clientId = cl.id_alt;
			ViewBag.clientCode = cl.codigo;
			ViewBag.clientName = cl.nombres + " " + cl.apellidos;

			DateTime today = CurrentDate.getNow();
			programa_clientes prgCl = db.programa_clientes.Where(m => m.cliente_id == cl.id_alt && m.fecha_inicio <= today && m.fecha_fin >= today).FirstOrDefault();
			ViewBag.programId = prgCl.programa.id;
			ViewBag.programName = prgCl.programa.nombre;
			ViewBag.programInit = prgCl.fecha_inicio.ToShortDateString();
			ViewBag.programEnd = prgCl.fecha_fin.ToShortDateString();

			return View(citas);
		}

		// GET: Citas/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			citas citas = await db.citas.FindAsync(id);
			if (citas == null)
			{
				return HttpNotFound();
			}
			return View(citas);
		}

		// POST: Citas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			citas citas = await db.citas.FindAsync(id);
			db.citas.Remove(citas);
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
