using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEPAMIAR_SGC.Models;
using System.Threading.Tasks;
using SEPAMIAR_SGC.Utilities;
using System.Net;
using System.Data.Entity;

namespace SEPAMIAR_SGC.Controllers
{
    public class NutricionController : Controller
    {
		// GET: Nutricion
		public ActionResult Index()
        {
			return View();
        }
	}
}