using SEPAMIAR_SGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SEPAMIAR_SGC.Seguridad;

namespace SEPAMIAR_SGC.Controllers
{
	public class CustomAuthorizeAttribute : AuthorizeAttribute
	{
		private _SGCModel db = new _SGCModel();
		public string permiso_id { get; set; }
		public string SGC_AccessCode { get; set; }

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (string.IsNullOrEmpty(SessionPersister.EmailUsuario))
			{
				filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", ReturnUrl = filterContext.HttpContext.Request.RawUrl }));
			}
			else
			{
				_SGCModel db = new _SGCModel();
				string mail = db.usuarios.Where(u => u.email.Equals(SessionPersister.EmailUsuario)).FirstOrDefault().email;
				int id = db.usuarios.Where(u => u.email.Equals(SessionPersister.EmailUsuario)).FirstOrDefault().id;
				CustomPrincipal mp = new CustomPrincipal(id, mail);
				if (!mp.IsInRole(SGC_AccessCode))
					filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Unauthorize" }));
			}

		}
	}
}