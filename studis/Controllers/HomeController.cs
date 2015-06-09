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

            //uh.padiLetnikDelno(58, 181);
            //uh.poloziLetnikPod8(55, 7);
            //uh.poloziLetnikDelno(48, 204);
            //uh.kreirajProfesorje();
            //uh.kreirajFiktivneRoke();

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

                    List<bool> vzp_predmetnik = new List<bool>();
   
                    foreach (var v in ViewBag.vpisi)
                    {
                        if (uh.jePredmetnikVzpostavljen(v))
                        {
                            vzp_predmetnik.Add(true);
                        }
                        else
                        {
                            vzp_predmetnik.Add(false);
                        }
                    }
                    ViewBag.vzpostavljen = vzp_predmetnik;
                }
            }
            
            return View();
   
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult VPisNiPotrjen()
        {
            return View();
        }
    }
}
