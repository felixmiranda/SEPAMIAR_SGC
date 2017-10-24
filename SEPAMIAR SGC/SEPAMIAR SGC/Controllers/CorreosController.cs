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
using SEPAMIAR_SGC.ViewModel;
using CryptoFramework;
using System.Diagnostics;
using System.IO;

namespace SEPAMIAR_SGC.Controllers
{
	public class CorreosController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: Correos
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("mail"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			return View(await db.correos.ToListAsync());
		}

		// GET: Correos/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			correos correos = await db.correos.FindAsync(id);
			if (correos == null)
			{
				return HttpNotFound();
			}
			return View(correos);
		}

		// GET: Correos/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Correos/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(correos correos)
		{
			correos.CreatedAt = CurrentDate.getNow();
			correos.CreatedBy = SessionPersister.UserId;
			correos.UpdatedAt = CurrentDate.getNow();
			correos.UpdatedBy = SessionPersister.UserId;

			if (ModelState.IsValid)
			{
				db.correos.Add(correos);
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(correos);
		}

		// GET: Correos/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			correos correos = await db.correos.FindAsync(id);
			if (correos == null)
			{
				return HttpNotFound();
			}
			return View(correos);
		}

		// POST: Correos/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(correos correos)
		{
			correos.UpdatedAt = CurrentDate.getNow();
			correos.UpdatedBy = SessionPersister.UserId;

			if (ModelState.IsValid)
			{
				db.Entry(correos).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(correos);
		}

		[HttpGet]
		public async Task<ActionResult> Send(int? id, string Error)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			correos correos = await db.correos.FindAsync(id);
			var tablas = db.correos_reglas.Where(m => m.EsRegla == false).GroupBy(m => m.Tabla).ToList();
			List<correos_reglas> enumCR = new List<correos_reglas>();
			foreach (IGrouping<string, correos_reglas> g in tablas)
			{
				correos_reglas cr = new correos_reglas();
				cr.Nombre = g.ToList().First().Nombre;
				cr.Tabla = g.Key;
				enumCR.Add(cr);
			}

			ViewBag.tablas = enumCR;

			if (correos == null)
			{
				return HttpNotFound();
			}
			if (Error != null)
			{
				ViewBag.errorMessage = Error;
			}
			ViewBag.From = SessionPersister.NombreCliente;
			return View(correos);
		}

		[HttpGet]
		public async Task<JsonResult> MailRules(string t)
		{
			List<correos_reglas> cr = await db.correos_reglas.Where(m => m.EsRegla == true && m.Tabla == t).ToListAsync();
			Crypto oCrypto = new Crypto();

			foreach (correos_reglas c in cr)
			{
				c.Condicion = oCrypto.EncryptData(c.Condicion).Replace(oldChar: '+', newChar: '-').Replace(oldChar: '/', newChar: '_'); ;
			}

			return Json(cr, JsonRequestBehavior.AllowGet);
		}

		//.Replace(oldChar: '-', newChar: '+').Replace(oldChar: '_', newChar: '/'); ;

		[HttpGet]
		public async Task<JsonResult> GetClientMails(string json, string table)
		{
			List<Dictionary<string, object>> oRulesLst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
			Crypto oCrypto = new Crypto();

			string query = "SELECT * FROM " + table + " WHERE 1 = 1 ";

			foreach (Dictionary<string, object> oRule in oRulesLst)
			{
				var p = oRule["operator"];
				string s = p.ToString();
				conditionalOperators op = (conditionalOperators)int.Parse(s);

				query = query + op.ToString() + " " + oCrypto.DecryptData(((string)oRule["rule"]).Replace(oldChar: '-', newChar: '+').Replace(oldChar: '_', newChar: '/')) + " ";
			}

			var result = await db.Database.SqlQuery<ClientesGeneralViewModel>(query).ToListAsync();

			if (result.Count > 0)
			{
				List<string> mailsLst = new List<string>();

				foreach (ClientesGeneralViewModel c in result)
				{
					if (!string.IsNullOrEmpty(c.email))
					{
						mailsLst.Add(c.email);
					}
					if (!string.IsNullOrEmpty(c.email_empresa))
					{
						mailsLst.Add(c.email_empresa);
					}
				}

				return Json(mailsLst, JsonRequestBehavior.AllowGet);
			}

			return null;
		}

		[HttpPost]
		public ActionResult Send(correos correos)
		{
			List<string> mails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(correos.To);

			if (Request.Files[0].ContentLength > 0)
			{
				List<MailFile> mailAttachments = new List<MailFile>();
				for (int i = 0; i < Request.Files.Count; i++)
				{
					HttpPostedFileBase file = Request.Files[i] as HttpPostedFileBase;

					byte[] fileB = new byte[file.ContentLength];

					using (Stream s = file.InputStream)
					{
						s.Position = 0;
						s.Read(fileB, 0, (int)s.Length);
					}

					MailFile mf = new MailFile(
						Name: file.FileName,
						FileBytes: fileB,
						FileType: file.ContentType);
					mailAttachments.Add(mf);
				}

				bool sent = Mail.SendWithAttachments(mails, correos.Asunto, correos.Contenido, mailAttachments);

				if (sent)
				{
					return RedirectToAction("Index");
				}
			}
			else
			{
				bool sent = Mail.Send(mails, correos.Asunto, correos.Contenido);

				if (sent)
				{
					return RedirectToAction("Index");
				}
			}

			return RedirectToAction("Send", new { id = correos.Id, Error = "No se pudo enviar correo, intente de nuevo mas tarde." });
		}

		public struct ClientMails
		{
			public string mail { get; set; }
			public string mailEmpresa { get; set; }
		}

		//// GET: Correos/Delete/5
		//public async Task<ActionResult> Delete(int? id)
		//{
		//    if (id == null)
		//    {
		//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//    }
		//    correos correos = await db.correos.FindAsync(id);
		//    if (correos == null)
		//    {
		//        return HttpNotFound();
		//    }
		//    return View(correos);
		//}

		//// POST: Correos/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> DeleteConfirmed(int id)
		//{
		//    correos correos = await db.correos.FindAsync(id);
		//    db.correos.Remove(correos);
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
	}
}
