using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Controllers
{
    public class KartotecniListController : Controller
    {
        // GET: KartotecniList
        public ActionResult Izpis()
        {
            // napolni in vrni možnosti za dropdown list


            return View();

            /*
             * Redirect za napako *
             TempData["Napaka"] = "Študent nima vzpostavljenega predmetnika!";
             return RedirectToAction("Napaka"); 
             */
        }

        [HttpPost]
        public ActionResult Izpis(string Selektor, string Selektor2)
        {
            // pripravi in vrni podatke za izpis

            return View();
        }

        public ActionResult Napaka()
        {
            ViewBag.Message = TempData["Napaka"];
            return View();
        }
    }
}