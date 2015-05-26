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
        public ActionResult Izpis(int? id)
        {
            // akcija za referenta
            if (id != null && User.IsInRole("Referent"))
            {
                try
                {
                    // vrni študentove študijske programe
                    var tmp = db.vpis.Where(v => v.vpisnaStevilka == id).Select(v => v.studijskiProgram);
                    var seznam = tmp.Distinct();
                    var items = db.sifrant_studijskiprogram.Where(p => seznam.Contains(p.id));
                    ViewBag.Program = new SelectList(items, "id", "naziv");
                    ViewBag.Vpisna = id;
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
                    var tmp = db.vpis.Where(v => v.vpisnaStevilka == student.vpisnaStevilka).Select(v => v.studijskiProgram);
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
        public ActionResult Izpis(int vpisna, string polaganja, sifrant_studijskiprogram model)
        {
            var tmp = db.vpis.Where(v => v.vpisnaStevilka == vpisna).Select(v => v.studijskiProgram);
            var seznam = tmp.Distinct();
            var items = db.sifrant_studijskiprogram.Where(p => seznam.Contains(p.id));
            ViewBag.Program = new SelectList(items, "id", "naziv");
            ViewBag.Vpisna = vpisna;

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

                // pridobi in vrni izpitne roke, ki so bili (zadnji) opravljani
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
                    foreach (var r in roki.OrderByDescending(r => r.datum))
                    {
                        foreach (var p in r.prijavanaizpits.Where(p => p.stanje == 2).ToList())
                        {
                            if (p.ocenas != null)
                            {
                                prijave.Add(p);
                            }
                        }       
                    }
                }
                ViewBag.Prijave = prijave;

                // zadnje opravljanje
                ViewBag.Izbira = 0;

                // vrni modula, ko je študent v 3. letniku BUNI
                if (model.id == 1000468)
                {
                    try
                    {
                        var vm = db.vpis.Where(v => v.vpisnaStevilka == vpisna && v.studijskiProgram == model.id && v.letnikStudija == 3).FirstOrDefault();

                        List<int> moduli = new List<int>();
                        {
                            foreach (var i in vm.izvajanjes)
                            {
                                if (i.predmet.modul != null)
                                {
                                    moduli.Add(Convert.ToInt32(i.predmet.modul.id));
                                }
                            }
                        }
                        var unique = moduli.Distinct();

                        List<string> final = new List<string>();
                        {
                            foreach (var u in unique)
                            {
                                if (moduli.Count(item => item == Convert.ToInt32(u)) > 1)
                                {
                                    final.Add(vm.izvajanjes.Where(i => i.predmet.modulId == u).Select(i => i.predmet.modul.ime).First());
                                }
                            }
                        }
                        ViewBag.Moduli = final;
                    }
                    catch { }
                }
            } 

            // vsa polaganja
            else
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

                // pridobi in vrni vse izpitni roke, ki so bili opravljani
                List<izpitnirok> roki = new List<izpitnirok>();
                {
                    foreach (var izv in izvajanja)
                    {
                        foreach (var r in izv.izpitniroks.ToList())
                        {
                            if (r.prijavanaizpits.Where(p => p.stanje == 2).ToList() != null)
                            {
                                roki.Add(r);
                            }
                        }
                    }
                }
                ViewBag.Roki = roki;

                // pridobi opravljanja izpitov
                List<prijavanaizpit> prijave = new List<prijavanaizpit>();
                {
                    foreach (var r in roki)
                    {
                        foreach (var p in r.prijavanaizpits.Where(p => p.stanje == 2).ToList())
                        {
                            if (p.ocenas != null)
                            {
                                prijave.Add(p);
                            }
                        }
                    }
                }
                ViewBag.Prijave = prijave;

                // vsa opravljanja
                ViewBag.Izbira = 1;

                // vrni modula, ko je študent v 3. letniku BUNI
                if (model.id == 1000468)
                {
                    try
                    {
                        var vm = db.vpis.Where(v => v.vpisnaStevilka == vpisna && v.studijskiProgram == model.id && v.letnikStudija == 3).FirstOrDefault();

                        List<int> moduli = new List<int>();
                        {
                            foreach (var i in vm.izvajanjes)
                            {
                                if (i.predmet.modul != null)
                                {
                                    moduli.Add(Convert.ToInt32(i.predmet.modul.id));
                                }
                            }
                        }
                        var unique = moduli.Distinct();

                        List<string> final = new List<string>();
                        {
                            foreach (var u in unique)
                            {
                                if (moduli.Count(item => item == Convert.ToInt32(u)) > 1)
                                {
                                    final.Add(vm.izvajanjes.Where(i => i.predmet.modulId == u).Select(i => i.predmet.modul.ime).First());
                                }
                            }
                        }
                        ViewBag.Moduli = final;
                    }
                    catch { }
                }
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