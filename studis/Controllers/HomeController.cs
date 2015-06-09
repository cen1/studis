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
            //uh.poloziLetnik(50, 213);
            //uh.poloziLetnikPod8(68, 213);
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
                    var vpisna = usr.students.First().vpisnaStevilka;
                    ViewBag.vpisnast = vpisna;
                    ViewBag.zetoni = usr.students.First().zetons.Where(a => a.porabljen == false);
                    var vpisi = usr.students.First().vpis;
                    ViewBag.vpisi = vpisi;

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
                    StudentHelper sh = new StudentHelper();
                    ViewBag.Ponavljanje = sh.trenutniVpis(vpisna).vrstaVpisa == 2;
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
