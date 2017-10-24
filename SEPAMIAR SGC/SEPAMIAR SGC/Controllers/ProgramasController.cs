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
	public class ProgramasController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: Programas
		[ActionName("Index")]
		[CustomAuthorize(SGC_AccessCode = "prg_index")]
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("prg"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			return View(await db.programas.ToListAsync());
		}

		public JsonResult GetActiveProgramForClient(int? value)
		{
			string clientCode = value?.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			if (c>0)
			{
				DateTime today = CurrentDate.getNow();
				int p = db.programa_clientes.Where(m => m.cliente_id == c && m.fecha_inicio <= today && m.fecha_fin >= today).Select(m => m.id).FirstOrDefault();
				if (p>0)
				{
					Dictionary<string, object> oDict = new Dictionary<string, object>();
					oDict.Add("pId", p);
					return Json(oDict, JsonRequestBehavior.AllowGet);
				}
			}

			return null;
		}

		// GET: Programas/Details/5
		[CustomAuthorize(SGC_AccessCode = "prg_details")]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			programas programas = await db.programas.FindAsync(id);
			if (programas == null)
			{
				return HttpNotFound();
			}
			return View(programas);
		}

		// GET: Programas/Create
		[CustomAuthorize(SGC_AccessCode = "prg_create")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: Programas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind(Include = "id,nombre,semanas")] programas programa)
		{
			programa.created_at = DateTimeOffset.Now.DateTime;
			programa.updated_at = DateTimeOffset.Now.DateTime;

			if (ModelState.IsValid)
			{
				db.programas.Add(programa);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(programa);
		}

		// GET: Programas/Edit/5
		[CustomAuthorize(SGC_AccessCode = "prg_update")]
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			programas programas = await db.programas.FindAsync(id);
			if (programas == null)
			{
				return HttpNotFound();
			}
			return View(programas);
		}

		// POST: Programas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(programas programa)
		{
			if (ModelState.IsValid)
			{
				programas p = db.programas.Find(programa.id);
				p.nombre = programa.nombre;
				p.semanas = p.semanas;
				p.updated_at = DateTimeOffset.Now.DateTime;

				db.Entry(p).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(programa);
		}

		// GET: Programas/Delete/5
		[CustomAuthorize(SGC_AccessCode = "prg_delete")]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			programas programas = await db.programas.FindAsync(id);
			if (programas == null)
			{
				return HttpNotFound();
			}
			return View(programas);
		}

		// POST: Programas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			programas programa = await db.programas.FindAsync(id);
			programa.deleted_at = DateTimeOffset.Now.DateTime;

			db.Entry(programa).State = EntityState.Modified;
			//db.programas.Remove(programas);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		// GET: Programas/Restore/5
		[ActionName("Restore")]
		public async Task<ActionResult> Restore(int id)
		{
			programas programa = await db.programas.FindAsync(id);
			programa.deleted_at = null;

			db.Entry(programa).State = EntityState.Modified;
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

		[HttpGet]
		public async Task<ActionResult> Customize(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			programas programas = await db.programas.FindAsync(id);
			if (programas == null)
			{
				return HttpNotFound();
			}
			return View(programas);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Customize(programas programa)
		{
			if (ModelState.IsValid)
			{
				if (Request.Files[0].ContentLength > 0)
				{
					for (int i = 0; i < Request.Files.Count; i++)
					{
						HttpPostedFileBase file = Request.Files[i] as HttpPostedFileBase;

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

							byte[] b = new byte[s.Length];
							s.Position = 0;
							s.Read(b, 0, b.Length);

							if (i == 0)
							{
								programa.imagen = Convert.ToBase64String(b);
							}
							else if (i == 1)
							{
								programa.icono = Convert.ToBase64String(b);
							}
						}
					}
				}

				programa.updated_at = CurrentDate.getNow();

				db.Entry(programa).State = EntityState.Modified;

				int c = await db.SaveChangesAsync();

				if (c > 0)
				{
					return RedirectToAction("Index");
				}

			}
			return View(programa);
		}

		/*
		 ****************************************
		 *										*
		 *	AQUI EMPIEZAN LAS LOGICAS PARA LA	*
		 *	APP PT.								*
		 *										*
		 ****************************************		 
		 */

		[HttpGet]
		public async Task<ActionResult> M_GetPrograms()
		{
			Mobile oMobile = new Mobile();
			List<programas> programsLst = await db.programas.Where(p => p.deleted_at == null).ToListAsync();

			Dictionary<string, object> oDict = new Dictionary<string, object>();

			if (programsLst.Count > 0)
			{
				for (int i = 0; i < programsLst.Count; i++)
				{
					Dictionary<string, object> dict = new Dictionary<string, object>();
					dict.Add("nombre", programsLst[i].nombre);
					dict.Add("frase", programsLst[i].frase);
					if (programsLst[i].imagen != null)
					{
						dict.Add("imagen", programsLst[i].imagen);
					}
					else
					{
						dict.Add("imagen", "");
					}
					dict.Add("descripcion", programsLst[i].descripcion);
					dict.Add("fuerza", programsLst[i].fuerza);
					dict.Add("resistencia", programsLst[i].resistencia);
					dict.Add("intensidad", programsLst[i].intensidad);
					if (programsLst[i].icono != null)
					{
						dict.Add("icono", programsLst[i].icono);
					}
					else
					{
						dict.Add("icono", "");
					}

					oDict.Add("p_" + i.ToString(), dict);
				}

				return Json(
					oMobile.GetDictForJSON(
						message: oDict.Count.ToString() + " programas activos.",
						data: oDict,
						code: MobileResponse.Success),
					JsonRequestBehavior.AllowGet);
			}

			return Json(
				oMobile.GetDictForJSON(
					message: "No hay programas activos.",
					data: null,
					code: MobileResponse.Error),
				JsonRequestBehavior.AllowGet);
		}
	}

}

