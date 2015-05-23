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
        [Authorize(Roles = "Referent, Študent")]
        public ActionResult Izpis(int? vpisna)
        {
            // akcija za referenta
            if (vpisna != null && User.IsInRole("Referent"))
            {
                try
                {
                    // vrni študentove študijske programe
                    var tmp = db.vpis.Where(v => v.vpisnaStevilka == vpisna).Select(v => v.studijskiProgram).ToList();
                    var seznam = tmp.Distinct();
                    var items = db.sifrant_studijskiprogram.Where(p => seznam.Contains(p.id));
                    ViewBag.Program = new SelectList(items, "id", "naziv");
                    ViewBag.Vpisna = vpisna;
                }
                catch
                {
                    TempData["Napaka"] = "Študent nima nobenega vpisnega lista!";
                    return RedirectToAction("Napaka");
                } 
            }
            else
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
                    ViewBag.Vpisna = student.vpisnaStevilka;
                }
                catch
                {
                    TempData["Napaka"] = "Študent nima nobenega vpisnega lista!";
                    return RedirectToAction("Napaka");
                }
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Referent, Študent")]
        public ActionResult Izpis(sifrant_studijskiprogram model, int vpisna, string polaganja)
        {
            // samo zadnje polaganje
            if (polaganja == "Zadnje polaganje")
            {
                // pridobi vse vpisne liste za študenta, kjer je študijski program = selected
                var vpisi = db.vpis.Where(v => v.vpisnaStevilka == vpisna && v.studijskiProgram == model.id).ToList();
                ViewBag.Vpisi = vpisi;

                // pridobi izvajanja
                List<izvajanje> izvajanja = new List<izvajanje>();
                {
                    foreach (var vpis in vpisi) 
                    {
                        foreach (var izv in vpis.izvajanjes.ToList())
                        {
                            izvajanja.Add(izv);
                        }
                        
                    }
                }
                ViewBag.Izvajanja = izvajanja;

                // pridobi izpitne roke in vrni izpitni roke, ki so bili (zadnji) opravljani
                List<izpitnirok> roki = new List<izpitnirok>();
                {
                    foreach (var izv in izvajanja)
                    {
                        var counter = 0;
                        foreach (var r in izv.izpitniroks.ToList())
                        {
                            if (r.prijavanaizpits.Where(p => p.stanje == 2).ToList() != null)
                            {
                                counter++;
                            }                       
                        }

                        if (counter > 1)
                        {
                            roki.Add(izv.izpitniroks.ToList().Last());
                        }
                    }
                }
                ViewBag.Roki = roki;

                // pridobi opravljanja izpitov
                List<prijavanaizpit> prijave = new List<prijavanaizpit>();
                {
                    foreach (var v in vpisi)
                    {
                        foreach (var p in v.prijavanaizpits.ToList())
                        {
                            prijave.Add(p);
                        }       
                    }
                }
                ViewBag.Prijave = prijave;

                // zadnje opravljanje
                ViewBag.Izbira = 0;
            } 

            // vsa polaganja
            else
            {
                // pridobi vse vpisne liste za študenta, kjer je študijski program = selected

                // pridobi izvajanja

                // pridobi vse izpitne roke, ki so bili opravljani

                // pridobi opravljanja izpitov

                // vsa opravljanja
                ViewBag.Izbira = 1;
            }

            return View();
        }

        [Authorize(Roles = "Referent, Študent")]
        public ActionResult Napaka()
        {
            ViewBag.Message = TempData["Napaka"];
            return View();
        }
    }
}