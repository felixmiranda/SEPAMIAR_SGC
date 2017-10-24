using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEPAMIAR_SGC.Controllers
{
    public class EntrenadorController : Controller
    {
		// GET: Entrenador
		[CustomAuthorize(SGC_AccessCode = "trainer_home")]
		public ActionResult Index()
        {
            return View();
        }
    }
}