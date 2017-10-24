using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEPAMIAR_SGC.Controllers
{
	//[CustomAuthorizeAttribute]
    public class SistemasController : Controller
    {
		[CustomAuthorize(SGC_AccessCode = "sys_home")]
        // GET: Sistemas
        public ActionResult Index()
        {
            return View();
        }
    }
}