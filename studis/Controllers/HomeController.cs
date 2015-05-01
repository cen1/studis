using studis.Models;
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
            ViewBag.Message = "Pozdravljeni v v sistemu studis";

            UserHelper uh = new UserHelper();
            var usr = uh.FindByName(User.Identity.Name);

            if (usr != null)
            {
                var s = usr.students;
                if (s.Count() == 0)
                {
                    ViewBag.student = "Niste še vpisani kot študent.";
                }
                else
                {
                    ViewBag.vpisnast = usr.students.First().vpisnaStevilka;
                    ViewBag.zetoni = usr.students.First().zetons.Where(a => a.porabljen == false);
                    ViewBag.vpisi = usr.students.First().vpis;
                    ViewBag.uh = new UserHelper();
                }
            }

            return View();
   
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
