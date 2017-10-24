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
using RestSharp;
using RestSharp.Authenticators;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using SEPAMIAR_SGC.Utilities;

namespace SEPAMIAR_SGC.Controllers
{
	public class ClientesController : Controller
	{
		private _SGCModel db = new _SGCModel();
		private static string ErrorMessage;

		// GET: Clientes
		[CustomAuthorize(SGC_AccessCode = "client_index")]
		public async Task<ActionResult> Index()
		{
			List<permisos> userAccesses = db.usuario_permisos
				.Where(m => m.usuario_id == SEPAMIAR_SGC.Seguridad.SessionPersister.UserId)
				.Select(m => m.permisos)
				.Where(m => m.codigo_interno.Contains("client"))
				.ToList();

			ViewBag.Permisos = userAccesses;

			return View(await db.clientes.ToListAsync());
		}

		public ActionResult SearchClient(int Value)
		{
			string clientCode = Value.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			if (c > 0)
			{
				return RedirectToAction("Create", "ProgramaClientes", new { clienteId = c });
			}
			else
			{
				return RedirectToAction("Index", "ProgramaClientes");
			}
		}

		public ActionResult SearchClientNewAppointment(int Value)
		{
			string clientCode = Value.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			if (c > 0)
			{
				return RedirectToAction("Create", "Citas", new { clienteId = c });
			}
			else
			{
				return RedirectToAction("Index", "Citas");
			}
		}

		public ActionResult GetClient(int Value)
		{
			string clientCode = Value.ToString("00000#");
			clientes c = db.clientes.Where(m => m.codigo == clientCode).FirstOrDefault();

			if (c != null)
			{
				return RedirectToAction("Global", "Clientes", new { Id = c.id_alt });
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		public ActionResult RenewClient(int Value)
		{
			string clientCode = Value.ToString("00000#");
			int c = db.clientes.Where(m => m.codigo == clientCode).Select(m => m.id_alt).FirstOrDefault();

			if (c > 0)
			{
				return RedirectToAction("Create", "ProgramaClientes", new { clienteId = c, feInit = DateTimeOffset.Now.DateTime });
			}
			else
			{
				return RedirectToAction("Index", "ProgramaClientes");
			}
		}

		// GET: Clientes/Details/5
		[CustomAuthorize(SGC_AccessCode = "client_details")]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clientes clientes = await db.clientes.FindAsync(id);
			if (clientes == null)
			{
				return HttpNotFound();
			}
			return View(clientes);
		}

		// GET: Clientes/Create
		[CustomAuthorize(SGC_AccessCode = "client_create")]
		public ActionResult Create()
		{
			if (ErrorMessage != null)
			{
				ViewBag.ErrorMessage = ErrorMessage;
			}

			return View();
		}

		// POST: Clientes/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpGet]
		public ActionResult CreateFromProspecto(prospectos prospecto)
		{
			if (ErrorMessage != null)
			{
				ViewBag.ErrorMessage = ErrorMessage;
			}

			if (prospecto.email_personal == null)
			{
				return Create();
			}

			Random r = new Random();
			clientes cl = new clientes();
			//byte?[] b = new byte?[0];
			cl.codigo = (db.clientes.Count() + 1).ToString("00000#");
			cl.nombres = prospecto.nombres;
			cl.apellidos = prospecto.apellidos;
			cl.email = prospecto.email_personal;
			cl.password = r.Next(1000, 9999);
			cl.foto = null;
			cl.centro_laboral = prospecto.centros_laboral;
			cl.cargo_laboral = prospecto.cargo_laboral;
			cl.email_empresa = prospecto.email_empresa;
			cl.fecha_nacimiento = prospecto.fecha_nacimiento;
			cl.genero = prospecto.genero;
			cl.documento_tipo = prospecto.documento_tipo;
			cl.documento_numero = prospecto.documento_numero;
			cl.created_at = DateTimeOffset.Now.Date;
			cl.updated_at = DateTimeOffset.Now.Date;

			return View(cl);
		}

		[HttpPost]
		public async Task<ActionResult> Create([Bind(Exclude = "id_alt"
			, Include = "codigo,nombres,apellidos,email,password,foto,centro_laboral,"+
			"cargo_laboral,email_empresa,fecha_nacimiento,genero,documento_tipo,documento_numero,"+
			"direccion,distrito,telefono,celular,ce_nombres,ce_apellidos,"+
			"ce_telefono,ce_celular,ce_email,como_se_entero,activo,acceso_autorizado,created_at,updated_at,deleted_at")]clientes cliente, HttpPostedFileBase file)
		{


			Random r = new Random();

			if (file != null)
			{
				Image img = Image.FromStream(file.InputStream, true, true);
				ImageCodecInfo pngInfo = ImageCodecInfo.GetImageEncoders().Where(codecInfo => codecInfo.MimeType == "image/png").First();

				using (Stream s = new MemoryStream())
				{
					using (EncoderParameters encParams = new EncoderParameters(1))
					{
						encParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionLZW);
						//quality should be in the range [0..100]
						img.Save(s, pngInfo, encParams);
						byte[] bNonNull = new byte[s.Length];
						s.Position = 0;
						s.Read(bNonNull, 0, (int)s.Length);
						cliente.foto = bNonNull;
					}
				}
			}

			cliente.codigo = (db.clientes.Count() + 1).ToString("00000#");
			cliente.password = r.Next(1000, 9999);

			cliente.acceso_autorizado = false;
			cliente.activo = false;

			cliente.created_at = DateTimeOffset.Now.Date;
			cliente.updated_at = DateTimeOffset.Now.Date;

			db.clientes.Add(cliente);

			try
			{
				await db.SaveChangesAsync();

				List<string> mails = new List<string>();
				mails.Add(cliente.email);
				mails.Add(cliente.email_empresa);

				Mail.Send(mails, "Bienvenido a Personal Training!",
					"Bienvenido a Personal Training! Tu código de cliente es: " + cliente.codigo + " y tu clave es: " + cliente.password + "/n/n" +
					"Recuerda que con estos datos podrás descargar e ingresar a nuestra app desde tu móvil y revisar tu programa activo, progreso semanal, dietas y mucho más!");

				return RedirectToAction("Create", "ProgramaClientes", new { clienteId = cliente.id_alt, feInit = CurrentDate.getNow() });
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
				return View(cliente);
			}
		}


		// GET: Clientes/Edit/5
		[CustomAuthorize(SGC_AccessCode = "client_update")]
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clientes clientes = await db.clientes.FindAsync(id);
			if (clientes == null)
			{
				return HttpNotFound();
			}
			return View(clientes);
		}

		// POST: Clientes/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit([Bind(Include = "id_alt,codigo,nombres,apellidos,email,password,foto,centro_laboral,"+
			"cargo_laboral,email_empresa,fecha_nacimiento,genero,documento_tipo,documento_numero,"+
			"direccion,distrito,telefono,celular,ce_nombres,ce_apellidos,"+
			"ce_telefono,ce_celular,ce_email,como_se_entero,activo,acceso_autorizado,created_at,updated_at,deleted_at")]clientes clientes, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
				if (file != null)
				{
					Image img = Image.FromStream(file.InputStream, true, true);
					ImageCodecInfo pngInfo = ImageCodecInfo.GetImageEncoders().Where(codecInfo => codecInfo.MimeType == "image/png").First();

					using (Stream s = new MemoryStream())
					{
						using (EncoderParameters encParams = new EncoderParameters(1))
						{
							encParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionLZW);
							//quality should be in the range [0..100]
							img.Save(s, pngInfo, encParams);
							byte[] bNonNull = new byte[s.Length];
							s.Position = 0;
							s.Read(bNonNull, 0, (int)s.Length);
							clientes.foto = bNonNull;
						}
					}
				}

				clientes.updated_at = DateTimeOffset.Now.Date;

				db.Entry(clientes).State = EntityState.Modified;
				await db.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(clientes);
		}

		// GET: Clientes/Delete/5
		[CustomAuthorize(SGC_AccessCode = "client_details")]
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			clientes clientes = await db.clientes.FindAsync(id);
			if (clientes == null)
			{
				return HttpNotFound();
			}
			return View(clientes);
		}

		// POST: Clientes/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			clientes clientes = await db.clientes.FindAsync(id);
			db.clientes.Remove(clientes);
			await db.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		[CustomAuthorize(SGC_AccessCode = "client_general")]
		public async Task<ActionResult> Global(int id)
		{
			int daysInMonth = DateTime.DaysInMonth(CurrentDate.getNow().Year, CurrentDate.getNow().Month);

			DateTime today = CurrentDate.getNow();
			DateTime firstDay = new DateTime(today.Year, today.Month, 1);
			DateTime lastDay = new DateTime(today.Year, today.Month, daysInMonth);

			// Info General
			clientes client = await db.clientes
				.Where(m => m.id_alt == id)
				.Include(m => m.nutricionistas.usuarios)
				.FirstOrDefaultAsync();

			// Info Programa
			programa_clientes program = await db.programa_clientes
				.Where(m => m.cliente_id == client.id_alt && m.fecha_inicio <= today && m.fecha_fin >= today)
				.Include(m => m.programa)
				.Include(m => m.horario.sala.local)
				.OrderBy(m => m.fecha_fin)
				.FirstOrDefaultAsync();
			ViewBag.ProgramId = program != null ? program.id.ToString() : "";
			ViewBag.ProgramName = program != null ? program.programa.nombre : "";
			ViewBag.ProgramInitDate = program != null ? program.fecha_inicio.ToShortDateString() : "";
			ViewBag.ProgramEndDate = program != null ? program.fecha_fin.ToShortDateString() : "";
			ViewBag.ProgramSchedule = program != null ? program.horario.hora.ToString() : "";
			ViewBag.Gym = program != null ? program.horario.sala.local.nombre : "";

			// Info Notas del cliente
			List<clientes_notas> notes = await db.clientes_notas.Where(n => n.idCliente == id).OrderByDescending(n => n.created_at).Take(15).ToListAsync();
			ViewBag.ClientNotes = notes;

			//// Info Calendario.
			//List<pesos_medidas> clientPyM = await db.pesos_medidas.Where(m => m.clienteId == id && m.created_at >= firstDay && m.created_at <= lastDay).ToListAsync();
			//List<clientes_asistencia> clientAssistance = await db.clientes_asistencia.Where(m => m.clienteId == id && m.fecha >= firstDay && m.fecha <= lastDay).ToListAsync();
			//List<clientes_congelamientos> clientFreezing = await db.clientes_congelamientos.Where(m => m.clienteId == id && m.fecha_desde >= firstDay).ToListAsync();
			//List<citas> clientConsults = await db.citas.Where(m => m.cliente_id == id && m.fecha >= firstDay && m.fecha <= lastDay).ToListAsync();

			// Info Nutricion.

			ViewBag.AssignedNutritionist = client.nutricionistas.usuarios.nombres + " " + client.nutricionistas.usuarios.apellidos;
			if (program != null)
			{
				// Info Ficha Medica
				fichas_medicas medicalHistory = await db.fichas_medicas.Where(m => m.programa_id == program.id).Include(m => m.cardio).FirstOrDefaultAsync();
				ViewBag.MedicalId = "No ha sido creada.";
				ViewBag.Cardio = "No se han ingresado datos.";
				if (medicalHistory != null)
				{
					int clientCurrMedicalHistoryIdCode = 10000000 + medicalHistory.id;
					ViewBag.MedicalId = clientCurrMedicalHistoryIdCode.ToString("FS#");

					if (medicalHistory.cardio != null)
					{
						CardioInfo lastCardioResults = medicalHistory.cardio.OrderByDescending(m => m.fecha).FirstOrDefault();
						if (lastCardioResults.cardiologia_aprobacion == CardioSuittable.Apto)
						{
							ViewBag.Cardio = CardioSuittable.Apto.ToString();
						}
						else
						{
							ViewBag.Cardio = CardioSuittable.NoApto.ToString();
						}
					}
				}

				// Info Dieta
				clientes_dietas clientDiet = await db.clientes_dietas.Where(m => m.programa_clientes_id == program.id).OrderByDescending(m => m.created_at).FirstOrDefaultAsync();
				if (clientDiet != null)
				{
					int dietCode = 10000000 + clientDiet.id;
					ViewBag.DietCode = dietCode.ToString("D#");
				}
			}

			// Info Ventas

			venta_usuarios usrSales = await db.venta_usuarios
				.Where(m => m.cliente_id == client.id_alt)
				.OrderByDescending(m => m.created_at)
				.FirstOrDefaultAsync();
			if (usrSales != null)
			{
				usuarios usr = await db.vendedores.Where(v => v.usuario_id == usrSales.vendedor_id).Include(v => v.usuarios).Select(v => v.usuarios).FirstOrDefaultAsync();
				ViewBag.Seller = usr.nombres + " " + usr.apellidos;
				ViewBag.LastTicket = usrSales.numero_boleta;
				ViewBag.LastRenewal = usrSales.fecha_inicio.ToShortDateString();

				semanas_precios weeks = db.semanas_precios.Where(s => s.id == usrSales.semanas_precio_id).FirstOrDefault();

				int boughtWeeks = weeks.cantidad_semanas;
				ViewBag.BoughtWeeks = boughtWeeks.ToString();
				int usedWeeks = await GetUsedWeeks(ClientId: client.id_alt, LastSaleId: usrSales.id);
				ViewBag.UsedWeeks = usedWeeks;
				ViewBag.PendingWeeks = boughtWeeks - usedWeeks;
			}

			//ViewBag.venta_usuario = db.venta_usuarios.Where(vu => vu.cliente_id == id).FirstOrDefault();
			//ViewBag.programa_cliente = db.programa_clientes.Where(pc => pc.cliente_id == id).FirstOrDefault();

			//ViewBag.calPyM = clientPyM;
			//ViewBag.calAsist = clientAssistance;
			//ViewBag.calFrz = clientFreezing;
			//ViewBag.calCitas = clientConsults;
			//

			//ViewBag.cardioApt = cardioApt;
			//ViewBag.foto = Convert.ToBase64String(cliente.foto);

			return View(client);
		}

		public async Task<int> GetUsedWeeks(int ClientId, int LastSaleId)
		{
			venta_usuarios usrSales = db.venta_usuarios.Where(v => v.id == LastSaleId).FirstOrDefault();
			List<programa_clientes> prgClient = await db.programa_clientes
				.Where(p => p.cliente_id == ClientId && p.fecha_inicio >= usrSales.fecha_inicio && p.fecha_fin <= usrSales.fecha_fin)
				.Include(p => p.programa)
				.ToListAsync();

			int usedWeeks = 0;

			if (prgClient != null)
			{
				foreach (programa_clientes p in prgClient)
				{
					usedWeeks += p.programa.semanas;
				}
			}

			return usedWeeks;
		}

		public int GetAvailableWeeks(int ClientId, int LastSaleId)
		{


			return 0;
		}

		[HttpGet]
		public ActionResult AssignNutritionist(int id)
		{
			string clientCode = id.ToString("00000#");
			clientes clientes = db.clientes.Where(m => m.codigo == clientCode).FirstOrDefault();

			var nutritionistLst = new SelectList(db.nutricionistas.Include(m => m.usuarios).ToList(), "usuario_id", "usuarios.nombres");
			ViewBag.NutritionistsLst = nutritionistLst;

			if (clientes == null)
			{
				return RedirectToAction("Index", "ProgramaClientes");
			}
			return View(clientes);
		}

		[HttpPost]
		public async Task<ActionResult> AssignNutritionist([Bind(Include = "id_alt, nutricionista_asignado")]clientes clientes)
		{
			clientes cl = db.clientes.Where(m => m.id_alt == clientes.id_alt).FirstOrDefault();

			cl.nutricionista_asignado = clientes.nutricionista_asignado;
			cl.updated_at = CurrentDate.getNow();

			db.Entry(cl).State = EntityState.Modified;

			int i = await db.SaveChangesAsync();

			if (i > 0)
			{
				return RedirectToAction("Global", new { Id = clientes.id_alt });
			}

			var nutritionistLst = new SelectList(db.nutricionistas.Include(m => m.usuarios).ToList(), "usuario_id", "usuarios.nombres");
			ViewBag.NutritionistsLst = nutritionistLst;

			return View(clientes);
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
		public async Task<ActionResult> M_GetNewKey(string c)
		{
			Mobile oMobile = new Mobile();
			clientes cl = db.clientes.Where(m => m.codigo == c).FirstOrDefault();

			if (cl != null)
			{
				Random r = new Random();
				int key = r.Next(1000, 9999);
				cl.password = key;
				db.Entry(cl).State = EntityState.Modified;

				int s = await db.SaveChangesAsync();

				if (s > 0)
				{
					RestClient client = new RestClient();
					client.BaseUrl = new Uri("https://api.mailgun.net/v3");
					client.Authenticator =
							new HttpBasicAuthenticator("api",
													   "key-44f7133c62e338cd2e8b2286c0285eac");
					RestRequest request = new RestRequest();
					request.AddParameter("domain",
										 "sgc.personaltraining.com.pe", ParameterType.UrlSegment);
					request.Resource = "{domain}/messages";
					request.AddParameter("from", "Personal Training Perú <app@sgc.personaltraining.com.pe>");
					request.AddParameter("to", cl.email);
					request.AddParameter("to", cl.email_empresa);
					request.AddParameter("subject", "Personal Training Perú App - Nueva Contraseña App");

					string body = "Estimado cliente(a), su nueva clave para la aplicación es: " + key.ToString();

					request.AddParameter("text", body);
					request.Method = Method.POST;
					IRestResponse response = client.Execute(request);

					if (response.StatusCode == HttpStatusCode.OK)
					{
						Dictionary<string, object> oDict = new Dictionary<string, object>();
						oDict.Add("key", key);
						return Json(
							oMobile.GetDictForJSON(
								message: "Clave cambiada correctamente.",
								data: oDict,
								code: MobileResponse.Success),
							JsonRequestBehavior.AllowGet);
					}
				}
			}
			return Json(
				oMobile.GetDictForJSON(
					message: "Error de validación de cliente.",
					data: null,
					code: MobileResponse.Success),
				JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult M_LogInClient(string c, int k)
		{
			Mobile oMobile = new Mobile();

			clientes cl = db.clientes.Where(m => m.codigo == c).Include(m => m.nutricionistas.usuarios).FirstOrDefault();
			int daysInMonth = DateTime.DaysInMonth(CurrentDate.getNow().Year, CurrentDate.getNow().Month);

			DateTime firstDay = new DateTime(CurrentDate.getNow().Year, CurrentDate.getNow().Month, 1);
			DateTime lastDay = new DateTime(CurrentDate.getNow().Year, CurrentDate.getNow().Month, daysInMonth);

			if (cl != null && k == cl.password)
			{
				DateTime today = CurrentDate.getNow();
				programa_clientes clientPrg = db.programa_clientes.Where(m => m.cliente_id == cl.id_alt && m.fecha_fin >= today).Include(m => m.programa).OrderBy(m => m.fecha_fin).FirstOrDefault();
				venta_usuarios clientSls = db.venta_usuarios.Where(m => m.cliente_id == cl.id_alt && m.fecha_fin >= today).OrderBy(m => m.fecha_fin).FirstOrDefault();

				if (clientPrg == null || clientSls == null)
				{
					return Json(
								oMobile.GetDictForJSON(
									message: "Cliente no cuenta con programa asignado o boleta vigente en el sistema.",
									data: null,
									code: MobileResponse.Success),
								JsonRequestBehavior.AllowGet);
				}

				citas clientAppointments = db.citas.Where(m => m.cliente_id == cl.id_alt && m.fecha > today).OrderBy(m => m.fecha).FirstOrDefault();

				Dictionary<string, object> oDict = new Dictionary<string, object>();
				oDict.Add(key: "name", value: cl.nombres);
				oDict.Add(key: "last_name", value: cl.apellidos);
				oDict.Add(key: "code", value: cl.id_alt);
				oDict.Add(key: "img", value: Convert.ToBase64String(cl.foto));
				oDict.Add(key: "curr_program", value: clientPrg.programa.nombre);
				oDict.Add(key: "curr_nutritionist", value: cl.nutricionistas.usuarios.nombres ?? null);
				oDict.Add(key: "curr_ticket", value: clientSls.numero_boleta);
				oDict.Add(key: "upcoming_nut_appointment_date", value: clientAppointments != null ? clientAppointments.fecha.ToShortDateString() : null);
				oDict.Add(key: "upcoming_membership_renewal_date", value: clientSls != null ? clientSls.fecha_fin.ToShortDateString() : null);


				return Json(
					oMobile.GetDictForJSON(
						message: "Inicio de sesión correcto.",
						data: oDict,
						code: MobileResponse.Success),
					JsonRequestBehavior.AllowGet);
			}

			return Json(
				oMobile.GetDictForJSON(
					message: "Error de inicio de sesión.",
					data: null,
					code: MobileResponse.Error),
				JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public async Task<ActionResult> M_UpdateDates(string c)
		{
			Mobile oMobile = new Mobile();
			clientes cl = db.clientes.Where(m => m.codigo == c).FirstOrDefault();
			if (cl != null)
			{
				DateTime today = CurrentDate.getNow();
				venta_usuarios clientSls = await db.venta_usuarios.Where(m => m.cliente_id == cl.id_alt && m.fecha_fin > today).OrderBy(m => m.fecha_fin).FirstOrDefaultAsync();
				citas clientAppointments = await db.citas.Where(m => m.cliente_id == cl.id_alt && m.fecha > today).OrderBy(m => m.fecha).FirstOrDefaultAsync();

				Dictionary<string, object> oDict = new Dictionary<string, object>();
				oDict.Add(key: "upcoming_nut_appointment_date", value: clientAppointments != null ? clientAppointments.fecha.ToShortDateString() : null);
				oDict.Add(key: "upcoming_membership_renewal_date", value: clientSls != null ? clientSls.fecha_fin.ToShortDateString() : null);

				return Json(
					oMobile.GetDictForJSON(
						message: "Usuario validado",
						data: oDict,
						code: MobileResponse.Success),
					JsonRequestBehavior.AllowGet);
			}

			return Json(
				oMobile.GetDictForJSON(
					message: "Error de validación de usuario.",
					data: null,
					code: MobileResponse.Error),
				JsonRequestBehavior.AllowGet);
		}


	}
}