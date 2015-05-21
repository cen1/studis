using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;
using System.Web.Security;

namespace studis.Controllers
{
    public class KartotecniListController : Controller
    {
        public studisEntities db = new studisEntities();

        // GET: KartotecniList
        public ActionResult Izpis()
        {
            // pridobi študenta
            UserHelper uh = new UserHelper();
            var student = uh.FindByName(User.Identity.Name).students.FirstOrDefault();
            
            try
            {
                // vrni študentove študijske programe
                var tmp = db.vpis.Where(v => v.vpisnaStevilka == student.vpisnaStevilka).Select(v => v.studijskiProgram).ToList();
                var seznam = tmp.Distinct();
                var items = db.sifrant_studijskiprogram.Where(p => seznam.Contains(p.id));
                ViewBag.Program = new SelectList(items, "id", "naziv");
            }
            catch
            {
                TempData["Napaka"] = "Študent nima nobenega vpisnega lista!";
                return RedirectToAction("Napaka"); 
            }

            return View();
        }

        [HttpPost]
        public ActionResult Izpis(sifrant_studijskiprogram model, string polaganja)
        {
            if (polaganja == "Zadnje polaganje")
            {

            } 
            else
            {

            }

            // pridobi vse vpisne liste za študenta, kjer je študijski program = selected

            // pridobi izvajanje

            // pridobi izvajatelja

            // pridobi ocene

            // vrni vse podatke

            return View();
        }

        public ActionResult Napaka()
        {
            ViewBag.Message = TempData["Napaka"];
            return View();
        }
    }
}