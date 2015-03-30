using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Glavna stran Studis";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
