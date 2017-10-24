using SEPAMIAR_SGC.Models;
using SEPAMIAR_SGC.Seguridad;
using SEPAMIAR_SGC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEPAMIAR_SGC.Controllers
{
	public class AccountController : Controller
	{
		private _SGCModel db = new _SGCModel();

		// GET: Account
		public ActionResult Login()
		{
			return View();
		}

		// GET: Account/Unauthotize
		public ActionResult Unauthorize()
		{
			return View("Unauthorize");
		}

		// POST: Account/Login
		[HttpPost]
		public ActionResult Login(UsuariosViewModel uvm)
		{
			if (!string.IsNullOrEmpty(uvm.usuario.email) && !string.IsNullOrEmpty(uvm.usuario.password))
			{
				usuarios usuario = db.usuarios.Where(u => u.email == uvm.usuario.email && u.password == uvm.usuario.password).FirstOrDefault();

				if (usuario != null)
				{
					SessionPersister.EmailUsuario = usuario.email;
					SessionPersister.NombreCliente = usuario.nombres + " " + usuario.apellidos;
					SessionPersister.AreaCliente = usuario.tipo;
					SessionPersister.UserId = usuario.id;

					if (usuario.foto != null)
					{
						SessionPersister.ImgBase64 = usuario.foto;
					}

					return RedirectToAction("Home");
				}
			}

			ViewBag.MessageLogIn = "Usuario inválido";
			return View("Login");
		}

		[HttpGet]
		public ActionResult LogOut()
		{
			SessionPersister.EmailUsuario = "";
			SessionPersister.NombreCliente = "";
			SessionPersister.AreaCliente = UserProfiles.Default;
			SessionPersister.ImgBase64 = "";

			return View("Login");
		}

		public ActionResult Home()
		{
			Models.UserProfiles usrProfile = db.usuarios.Where(m => m.id == SessionPersister.UserId).Select(m => m.tipo).FirstOrDefault();

			switch (usrProfile)
			{
				case UserProfiles.Default:
					goto default;
				case UserProfiles.Nutricionista:
					return RedirectToAction("Index", "nutricion");
				case UserProfiles.Vendedor:
					return RedirectToAction("Index", "ventas");
				case UserProfiles.Entrenador:
					return RedirectToAction("Index", "entrenador");
				case UserProfiles.Cajero:
					return RedirectToAction("Index", "cajero");
				case UserProfiles.Sistemas:
					return RedirectToAction("Index", "sistemas");
				case UserProfiles.Pesos_y_Medidas:
					return RedirectToAction("Index", "pym");
				case UserProfiles.Cardiología:
					return RedirectToAction("Index", "cardiologia");
				case UserProfiles.Marketing:
					return RedirectToAction("Index", "marketing");
				default:
					return RedirectToAction("Unauthorize");
			}
		}
	}
}