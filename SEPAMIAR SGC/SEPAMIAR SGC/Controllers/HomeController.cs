using SEPAMIAR_SGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEPAMIAR_SGC.Controllers
{
    public class HomeController : Controller
    {
		private _SGCModel db = new _SGCModel();

		// GET: Home
		[CustomAuthorize(permiso_id = "1")]
		public ActionResult Index()
        {
            return View();
        }
    }
}