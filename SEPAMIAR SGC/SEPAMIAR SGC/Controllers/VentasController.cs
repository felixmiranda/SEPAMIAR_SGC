using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEPAMIAR_SGC.Controllers
{
    public class VentasController : Controller
    {
		// GET: Ventas
		[CustomAuthorize(SGC_AccessCode = "sales_home")]
		public ActionResult Index()
        {
            return View();
        }
    }
}