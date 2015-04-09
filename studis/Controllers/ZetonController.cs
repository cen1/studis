using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Controllers
{
    public class ZetonController : Controller
    {
        // GET: Zeton
        [Authorize(Roles = "Referent")]
        public ActionResult Index()
        {
            return View();
        }
    }
}