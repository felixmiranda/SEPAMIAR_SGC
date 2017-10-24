using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEPAMIAR_SGC.Controllers
{
    public class PYMController : Controller
    {
		[CustomAuthorize(SGC_AccessCode = "pym_home")]
		// GET: Pesos y Medidas Home
		public ActionResult Index()
		{
			return View();
		}
	}
}