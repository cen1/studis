using studis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace studis.Controllers
{
    [Authorize(Roles = "Študent, Referent")]
    public class VpisniListController : Controller
    {
        public studisEntities db = new studisEntities();

        // GET: VpisniList
        //GET: VPisniList/id od userja!
        public ActionResult VpisniList(int? x)
        {
            UserHelper uh = new UserHelper();

            student sid = null;
            kandidat kid = null;

            if (x == null && User.IsInRole("Študent"))
            {
                //poglej ce obstaja vnos v tabeli student
                sid = uh.FindByName(User.Identity.Name).students.FirstOrDefault();
                if (sid != null)
                {
                    ViewBag.zetoniVB = sid.zetons.Where(a => a.porabljen == false);
                    //poglej ce ima zeton
                    if (!uh.imaZeton(sid))
                    {
                        return RedirectToAction("NiZetona");
                    }
                }
                else
                {
                    kid = uh.FindByName(User.Identity.Name).kandidats.FirstOrDefault();
                    if (kid == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Kandidat null");
                        return HttpNotFound();
                    }
                }
            }
            else if (x != null && User.IsInRole("Referent"))
            {
                //pogledamo ce obstaja študent ali kandidat
                //gledamo po userId ker sta kandidat id in student vpisnaStevilka hipoteticno lahko enaka

                var usr = db.my_aspnet_users.Where(a => a.id == x).FirstOrDefault();
                if (usr != null)
                {
                    sid = usr.students.FirstOrDefault();
                    if (sid == null)
                    {
                        //pogledamo se ce obstaja kandidat
                        kid = usr.kandidats.FirstOrDefault();
                        if (kid == null)
                        {
                            System.Diagnostics.Debug.WriteLine("Kandidat null");
                            return HttpNotFound();
                        }
                    }
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Fatal error, unauthorized");
                return HttpNotFound();
            }

            if (sid == null && kid == null)
            {
                System.Diagnostics.Debug.WriteLine("Ne obstaja student ali kandidat");
                return HttpNotFound();
            }

            ViewBag.StudijskiProgramiVB = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.KlasiusVB = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            if (User.IsInRole("Študent"))
                ViewBag.VrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa.Where(a => a.id != 98), "id", "naziv");
            else if (User.IsInRole("Referent"))
                ViewBag.VrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            ViewBag.NacinStudijaVB = new SelectList(db.sifrant_nacinstudija, "id", "naziv");
            ViewBag.OblikaStudijaVB = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.SpolVB = new SelectList(db.sifrant_spol.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.ObcinaVB = new SelectList(db.sifrant_obcina.OrderBy(a => a.naziv != "Ljubljana").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.KrajIzvajanjaVB = new SelectList(db.sifrant_obcina.Where(a => a.naziv == "Ljubljana" || a.naziv == "Postojna"), "id", "naziv");
            ViewBag.DrzavaVB = new SelectList(db.sifrant_drzava.OrderBy(a => a.naziv != "Slovenija").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.PostnaStevilkaVB = new SelectList(db.sifrant_postnastevilka.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.LetnikVB = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.StudijskoLetoPrvegaVpisaVB = new SelectList(db.sifrant_studijskoletoprvegavpisa.OrderByDescending(a => a.id), "id", "naziv");
            ViewBag.IzbirnaSkupinaVB = new SelectList(db.sifrant_izbirnaskupina, "id", "naziv");
            ViewBag.SmerVB = new SelectList(db.sifrant_smer, "id", "naziv");
            
            if (sid == null)
            {
                var rdan = Enumerable.Range(1, 31).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                var rmesec = Enumerable.Range(1, 12).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                var rleto = Enumerable.Range(DateTime.Now.Year - 99, 100).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                ViewBag.dr_danVB = rdan;
                ViewBag.dr_mesecVB = rmesec;
                ViewBag.dr_letoVB = rleto;
            }
            else
            {
                var rdan = Enumerable.Range(sid.datumRojstva.Day, 1).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                var rmesec = Enumerable.Range(sid.datumRojstva.Month, 1).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                var rleto = Enumerable.Range(sid.datumRojstva.Year, 1).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                ViewBag.dr_danVB = rdan;
                ViewBag.dr_mesecVB = rmesec;
                ViewBag.dr_letoVB = rleto;
            }

            //napolnimo podatke ce se da
            if (sid != null) {
                int tid = sid.vpis.Last().id;
                Baza bz = new Baza();
                VpisniListModel model = bz.getVpisniList(tid);

                if (model.naslovZacasni != null) ViewBag.zacasnipodatkiVB = true;
                else ViewBag.zacasnipodatkiVB = false;
               
                return View(model);
            }
            else
            {
                if (kid != null)
                {
                    //prvi vpis, napolni iz tabele kandidat
                    VpisniListModel model = Baza.getVpisniListKandidat(kid.my_aspnet_users);
                    ViewBag.zacasnipodatkiVB = false;
                    return View(model);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Fatal error, ne obstaja kandidat ali student");
                    return HttpNotFound();
                }
            }
        }

        public ActionResult NiZetona()
        {
            return View();
		}        
        
        [HttpPost]
        public ActionResult VpisniList(studis.Models.VpisniListModel model, int? x)
        {
            UserHelper uh = new UserHelper();

            //poglej ce obstaja vnos v tabeli student
            student tsid = null; //zacasen iz (potencialno) uh konteksta
            student sid = null; //dejanski            

            if (x == null && User.IsInRole("Študent"))
            {
                //poglej ce obstaja vnos v tabeli student
                tsid = uh.FindByName(User.Identity.Name).students.FirstOrDefault();
            }
            else if (x != null && User.IsInRole("Referent"))
            {
                //pogledamo ce obstaja študent ali kandidat
                //gledamo po userId ker sta kandidat id in student vpisnaStevilka hipoteticno lahko enaka
                var usr = db.my_aspnet_users.Where(a => a.id == x).FirstOrDefault();
                if (usr != null)
                {
                    tsid = usr.students.FirstOrDefault();
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else return HttpNotFound();

            if (User.IsInRole("Študent"))
            {
                if (tsid != null)
                {
                    //poglej ce ima zeton
                    if (!uh.imaZeton(tsid))
                    {
                        return RedirectToAction("NiZetona");
                    }
                    else
                    {
                        //poglej ce se vsaj en zeton ujema z oddanimi podatki
                        if (!uh.preveriZeton(tsid, model))
                        {
                            return RedirectToAction("NiZetona");
                        }
                    }
                }
            }

            //sestavi datum rojstva
            DateTime temp_dr = new DateTime();
            string dt = model.dr_leto.ToString()+"-"+model.dr_mesec.ToString()+"-"+model.dr_dan.ToString();
            var tp = DateTime.TryParse(dt, out temp_dr);
            if (!tp) ModelState.AddModelError("", "Napačen datum rojstva");
            System.Diagnostics.Debug.WriteLine(dt);

            model.datumRojstva = temp_dr;

            model.studijskoLeto = "2015";

            //preveri logiko
            if (User.IsInRole("Študent"))
            {
                List<string> logic = uh.preveriLogiko(model, tsid, uh.FindByName(User.Identity.Name), false);
                if (logic.Count() != 0)
                {
                    foreach (var l in logic)
                    {
                        ModelState.AddModelError("", l);
                    }
                }
            }
            else if (User.IsInRole("Referent"))
            {
                List<string> logic = uh.preveriLogiko(model, tsid, db.my_aspnet_users.Where(a => a.id == x).FirstOrDefault(), false);
                if (logic.Count() != 0)
                {
                    foreach (var l in logic)
                    {
                        ModelState.AddModelError("", l);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                vpi v = new vpi();

                //ni prvi vpis
                if (tsid != null) {
                    
                    v.vpisnaStevilka = tsid.vpisnaStevilka;

                    //pridobi studenta iz controller konteksta!
                    sid = db.students.Where(a => a.vpisnaStevilka == tsid.vpisnaStevilka).First();

                    sid.ime = model.ime;
                    sid.priimek = model.priimek;
                    sid.datumRojstva = model.datumRojstva;
                    sid.davcnaStevilka = model.davcnaStevilka;
                    
                    sid.drzavaRojstva = model.drzavaRojstva;
                    sid.drzavljanstvo = model.drzavljanstvo;
                    sid.email = model.email;
                    sid.emso = model.emso;
                    sid.krajRojstva = model.krajRojstva;
                    sid.obcinaRojstva = model.obcinaRojstva;

                    sid.vrocanje = model.vrocanje;
                    sid.naslov = model.naslov;
                    sid.obcina = model.obcina;
                    sid.postnaStevilka = model.postnaStevilka;
                    sid.drzava = model.drzava;

                    sid.vrocanjeZacasni = model.vrocanjeZacasni;
                    sid.naslovZacasni = model.naslovZacasni;
                    sid.obcinaZacasni = model.obcinaZacasni;
                    sid.postnaStevilkaZacasni = model.postnaStevilkaZacasni;
                    sid.drzavaZacasni = model.drzavaZacasni;
                    
                    sid.prenosniTelefon = model.prenosniTelefon;
                    
                    
                    sid.spol = model.spol;

                    v.izbirnaSkupina = model.izbirnaSkupina;
                    v.krajIzvajanja = model.krajIzvajanja;      
                    v.letnikStudija = model.letnikStudija;
                    v.nacinStudija = model.nacinStudija;   
                    v.oblikaStudija = model.oblikaStudija;
                    v.smer = model.smer;
                    v.soglasje1 = model.soglasje1;
                    v.soglasje2 = model.soglasje2;
                    v.studijskiProgram = model.studijskiProgram;
                    v.studijskoLeto = Convert.ToInt32(model.studijskoLeto);
                    v.studijskoLetoPrvegaVpisa = model.studijskoLetoPrvegaVpisa;
                    v.vrstaStudija = model.vrstaStudija;
                    v.vrstaVpisa = model.vrstaVpisa;
                    if (User.IsInRole("Študent"))
                        v.potrjen = false;
                    else if (User.IsInRole("Referent"))
                        v.potrjen = true;
                    v.studijskoLeto = DateTime.Now.Year;

                    db.vpis.Add(v);
                }
                else { //prvi vpis, loceno je treba naredit vnos za studenta
                    student s = new student();

                    if (User.IsInRole("Študent"))
                        s.userId = uh.FindByName(User.Identity.Name).id;
                    else if (User.IsInRole("Referent"))
                        s.userId = x.Value;
                    
                    s.datumRojstva = model.datumRojstva;
                    s.davcnaStevilka = model.davcnaStevilka;
                    s.drzava = model.drzava;
                    s.drzavaRojstva = model.drzavaRojstva;
                    s.drzavaZacasni = model.drzavaZacasni;
                    s.drzavljanstvo = model.drzavljanstvo;
                    s.email = model.email;
                    s.emso = model.emso;
                    s.ime = model.ime;
                    s.krajRojstva = model.krajRojstva;
                    s.naslov = model.naslov;
                    s.naslovZacasni = model.naslovZacasni;
                    s.obcina = model.obcina;
                    s.obcinaRojstva = model.obcinaRojstva;
                    s.obcinaZacasni = model.obcinaZacasni;
                    s.postnaStevilka = model.postnaStevilka;
                    s.postnaStevilkaZacasni = model.postnaStevilkaZacasni;
                    s.prenosniTelefon = model.prenosniTelefon;
                    s.priimek = model.priimek;
                    s.spol = model.spol;
                    s.vrocanje = model.vrocanje;
                    s.vrocanjeZacasni = model.vrocanjeZacasni;

                    v.izbirnaSkupina = model.izbirnaSkupina;
                    v.krajIzvajanja = model.krajIzvajanja;
                    v.letnikStudija = model.letnikStudija;
                    v.nacinStudija = model.nacinStudija;
                    v.oblikaStudija = model.oblikaStudija;
                    v.smer = model.smer;
                    v.soglasje1 = model.soglasje1;
                    v.soglasje2 = model.soglasje2;
                    v.studijskiProgram = model.studijskiProgram;
                    v.studijskoLeto = Convert.ToInt32(model.studijskoLeto);
                    v.studijskoLetoPrvegaVpisa = model.studijskoLetoPrvegaVpisa;
                    v.vrstaStudija = model.vrstaStudija;
                    v.vrstaVpisa = model.vrstaVpisa;

                    if (User.IsInRole("Študent"))
                        v.potrjen = false;
                    else if (User.IsInRole("Referent"))
                        v.potrjen = true;
                    v.studijskoLeto = DateTime.Now.Year;

                    db.students.Add(s);
                    db.SaveChanges();

                    v.vpisnaStevilka = s.vpisnaStevilka;
                    db.vpis.Add(v);
                }
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                //porabi vse žetone
                if (sid != null) {
                    var zetoni = db.zetons.Where(a => a.vpisnaStevilka == sid.vpisnaStevilka);
                    foreach (var z in zetoni)
                    {
                        z.porabljen = true; 
                    }
                }
                db.SaveChanges();
                
                //ce je ponavljanje, absolvent, nadaljevanje, vpis za zaključek se ne vzpostavlja predmetnika
                if (v.vrstaVpisa != 1)
                    return RedirectToAction("VpisniListSuccess", "VpisniList");

                //ce je referent vzpostavi obvezni del predmetnika in vrzi na success
                if (User.IsInRole("Referent"))
                {
                    PredmetHelper ph = new PredmetHelper();
                    List<predmet> obv = null;
                    if (v.letnikStudija == 1) obv = ph.obvezni1().ToList();
                    if (v.letnikStudija == 2) obv = ph.obvezni2().ToList();
                    if (v.letnikStudija == 3) obv = ph.obvezni3().ToList();

                    foreach (var o in obv)
                    {
                        v.izvajanjes.Add(db.izvajanjes.Where(a => a.predmetId == o.id).First());
                    }
                    db.SaveChanges();

                    TempData["id"] = v.id;
                    return RedirectToAction("VpisniListSuccess", "VpisniList");
                }
                //sicer naprej na predmetnik
                return RedirectToAction("VpisniListPredmetnik", "VpisniList", new { id = v.id });

            }
            else
            {
                var errors = ModelState.Select(z => z.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                foreach (var e in errors)
                    foreach (var ee in e)
                        System.Diagnostics.Debug.WriteLine(ee.ErrorMessage);
            }

            //repopulate model lists
            ViewBag.StudijskiProgramiVB = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.KlasiusVB = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            if (User.IsInRole("Študent"))
                ViewBag.VrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa.Where(a => a.id != 98), "id", "naziv");
            else if (User.IsInRole("Referent"))
                ViewBag.VrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            ViewBag.NacinStudijaVB = new SelectList(db.sifrant_nacinstudija, "id", "naziv");
            ViewBag.OblikaStudijaVB = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.SpolVB = new SelectList(db.sifrant_spol.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.ObcinaVB = new SelectList(db.sifrant_obcina.OrderBy(a => a.naziv != "Ljubljana").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.KrajIzvajanjaVB = new SelectList(db.sifrant_obcina.Where(a => a.naziv == "Ljubljana" || a.naziv == "Postojna"), "id", "naziv");
            ViewBag.DrzavaVB = new SelectList(db.sifrant_drzava.OrderBy(a => a.naziv != "Slovenija").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.PostnaStevilkaVB = new SelectList(db.sifrant_postnastevilka.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.LetnikVB = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.StudijskoLetoPrvegaVpisaVB = new SelectList(db.sifrant_studijskoletoprvegavpisa.OrderByDescending(a => a.id), "id", "naziv");
            ViewBag.IzbirnaSkupinaVB = new SelectList(db.sifrant_izbirnaskupina, "id", "naziv");
            ViewBag.SmerVB = new SelectList(db.sifrant_smer, "id", "naziv");

            if (sid == null)
            {
                var rdan = Enumerable.Range(1, 31).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                var rmesec = Enumerable.Range(1, 12).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                var rleto = Enumerable.Range(DateTime.Now.Year - 99, 100).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                ViewBag.dr_danVB = rdan;
                ViewBag.dr_mesecVB = rmesec;
                ViewBag.dr_letoVB = rleto;
            }
            else
            {
                var rdan = Enumerable.Range(sid.datumRojstva.Day, 1).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                var rmesec = Enumerable.Range(sid.datumRojstva.Month, 1).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                var rleto = Enumerable.Range(sid.datumRojstva.Year, 1).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
                ViewBag.dr_danVB = rdan;
                ViewBag.dr_mesecVB = rmesec;
                ViewBag.dr_letoVB = rleto;
            }

            if (model.naslovZacasni != null) ViewBag.zacasnipodatkiVB = true;
            else ViewBag.zacasnipodatkiVB = false;

            return View(model);
        }

        [Authorize(Roles = "Referent")]
        public ActionResult VpisniListEdit(int id)
        {
            if (db.vpis.Find(id) == null) return HttpNotFound();

            ViewBag.StudijskiProgramiVB = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.KlasiusVB = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.VrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            ViewBag.NacinStudijaVB = new SelectList(db.sifrant_nacinstudija, "id", "naziv");
            ViewBag.OblikaStudijaVB = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.SpolVB = new SelectList(db.sifrant_spol.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.ObcinaVB = new SelectList(db.sifrant_obcina.OrderBy(a => a.naziv != "Ljubljana").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.KrajIzvajanjaVB = new SelectList(db.sifrant_obcina.Where(a => a.naziv == "Ljubljana" || a.naziv == "Postojna"), "id", "naziv");
            ViewBag.DrzavaVB = new SelectList(db.sifrant_drzava.OrderBy(a => a.naziv != "Slovenija").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.PostnaStevilkaVB = new SelectList(db.sifrant_postnastevilka.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.LetnikVB = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.StudijskoLetoPrvegaVpisaVB = new SelectList(db.sifrant_studijskoletoprvegavpisa.OrderByDescending(a => a.id), "id", "naziv");
            ViewBag.IzbirnaSkupinaVB = new SelectList(db.sifrant_izbirnaskupina, "id", "naziv");
            ViewBag.SmerVB = new SelectList(db.sifrant_smer, "id", "naziv");

            var sid = db.vpis.Find(id).student;

            var rdan = Enumerable.Range(sid.datumRojstva.Day, 1).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString()
            });
            var rmesec = Enumerable.Range(sid.datumRojstva.Month, 1).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString()
            });
            var rleto = Enumerable.Range(sid.datumRojstva.Year, 1).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString()
            });
            ViewBag.dr_danVB = rdan;
            ViewBag.dr_mesecVB = rmesec;
            ViewBag.dr_letoVB = rleto;

            Baza bz = new Baza();
            VpisniListModel model = bz.getVpisniListEdit(id);

            if (sid.naslovZacasni == null)
                ViewBag.zacasnipodatkiVB = false;
            else
                ViewBag.zacasnipodatkiVB = true;

            return View(model);
        }

        [Authorize(Roles = "Referent")]
        [HttpPost]
        public ActionResult VpisniListEdit(VpisniListModel model, int id)
        {
            vpi v = db.vpis.Find(id);
            if (v == null) return HttpNotFound();
            student sid = v.student;

            UserHelper uh = new UserHelper();

            //sestavi datum rojstva
            DateTime temp_dr = new DateTime();
            string dt = model.dr_leto.ToString() + "-" + model.dr_mesec.ToString() + "-" + model.dr_dan.ToString();
            var tp = DateTime.TryParse(dt, out temp_dr);
            if (!tp) ModelState.AddModelError("", "Napačen datum rojstva");
            System.Diagnostics.Debug.WriteLine(dt);

            model.datumRojstva = temp_dr;

            model.studijskoLeto = "2015";

            //preveri logiko
            List<string> logic = uh.preveriLogiko(model, sid, db.my_aspnet_users.Where(a => a.id == sid.userId).FirstOrDefault(), true);
            if (logic.Count() != 0)
            {
                foreach (var l in logic)
                {
                    ModelState.AddModelError("", l);
                }
            }

            if (ModelState.IsValid)
            {
                //ni prvi vpis
                v.vpisnaStevilka = sid.vpisnaStevilka;

                //pridobi studenta iz controller konteksta!
                sid = db.students.Where(a => a.vpisnaStevilka == sid.vpisnaStevilka).First();

                sid.ime = model.ime;
                sid.priimek = model.priimek;
                sid.datumRojstva = model.datumRojstva;
                sid.davcnaStevilka = model.davcnaStevilka;

                sid.drzavaRojstva = model.drzavaRojstva;
                sid.drzavljanstvo = model.drzavljanstvo;
                sid.email = model.email;
                sid.emso = model.emso;
                sid.krajRojstva = model.krajRojstva;
                sid.obcinaRojstva = model.obcinaRojstva;

                sid.vrocanje = model.vrocanje;
                sid.naslov = model.naslov;
                sid.obcina = model.obcina;
                sid.postnaStevilka = model.postnaStevilka;
                sid.drzava = model.drzava;

                sid.vrocanjeZacasni = model.vrocanjeZacasni;
                sid.naslovZacasni = model.naslovZacasni;
                sid.obcinaZacasni = model.obcinaZacasni;
                sid.postnaStevilkaZacasni = model.postnaStevilkaZacasni;
                sid.drzavaZacasni = model.drzavaZacasni;

                sid.prenosniTelefon = model.prenosniTelefon;


                sid.spol = model.spol;

                v.izbirnaSkupina = model.izbirnaSkupina;
                v.krajIzvajanja = model.krajIzvajanja;
                v.letnikStudija = model.letnikStudija;
                v.nacinStudija = model.nacinStudija;
                v.oblikaStudija = model.oblikaStudija;
                v.smer = model.smer;
                v.soglasje1 = model.soglasje1;
                v.soglasje2 = model.soglasje2;
                v.studijskiProgram = model.studijskiProgram;
                v.studijskoLeto = Convert.ToInt32(model.studijskoLeto);
                v.studijskoLetoPrvegaVpisa = model.studijskoLetoPrvegaVpisa;
                v.vrstaStudija = model.vrstaStudija;
                v.vrstaVpisa = model.vrstaVpisa;
                v.potrjen = false;
                v.studijskoLeto = DateTime.Now.Year;
             
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                TempData["id"] = id;
                TempData["nazaj"] = sid.vpisnaStevilka;
                return RedirectToAction("VpisniListSuccess2", "VpisniList", new { id = v.id });

            }
            else
            {
                ModelState.AddModelError("", "Prišlo je do napake.");
                var errors = ModelState.Select(z => z.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                foreach (var e in errors)
                    foreach (var ee in e)
                        System.Diagnostics.Debug.WriteLine(ee.ErrorMessage);
            }

            //repopulate model lists
            ViewBag.StudijskiProgramiVB = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.KlasiusVB = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.VrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            ViewBag.NacinStudijaVB = new SelectList(db.sifrant_nacinstudija, "id", "naziv");
            ViewBag.OblikaStudijaVB = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.SpolVB = new SelectList(db.sifrant_spol.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.ObcinaVB = new SelectList(db.sifrant_obcina.OrderBy(a => a.naziv != "Ljubljana").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.KrajIzvajanjaVB = new SelectList(db.sifrant_obcina.Where(a => a.naziv == "Ljubljana" || a.naziv == "Postojna"), "id", "naziv");
            ViewBag.DrzavaVB = new SelectList(db.sifrant_drzava.OrderBy(a => a.naziv != "Slovenija").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.PostnaStevilkaVB = new SelectList(db.sifrant_postnastevilka.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.LetnikVB = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.StudijskoLetoPrvegaVpisaVB = new SelectList(db.sifrant_studijskoletoprvegavpisa.OrderByDescending(a => a.id), "id", "naziv");
            ViewBag.IzbirnaSkupinaVB = new SelectList(db.sifrant_izbirnaskupina, "id", "naziv");
            ViewBag.SmerVB = new SelectList(db.sifrant_smer, "id", "naziv");

            var rdan = Enumerable.Range(sid.datumRojstva.Day, 1).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString()
            });
            var rmesec = Enumerable.Range(sid.datumRojstva.Month, 1).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString()
            });
            var rleto = Enumerable.Range(sid.datumRojstva.Year, 1).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString()
            });
            ViewBag.dr_danVB = rdan;
            ViewBag.dr_mesecVB = rmesec;
            ViewBag.dr_letoVB = rleto;

            if (model.naslovZacasni != null) ViewBag.zacasnipodatkiVB = true;
            else ViewBag.zacasnipodatkiVB = false;

            return View(model);
        }

        public ActionResult VpisniListSuccess()
        {
            ViewBag.id = TempData["id"];
            return View();
        }

        public ActionResult VpisniListSuccess2()
        {
            ViewBag.id = TempData["id"];
            ViewBag.nazaj = TempData["nazaj"];
            return View();
        }

        public ActionResult VpisniListPredmetnik(int id)
        {
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            if (vl.letnikStudija==1)
                return RedirectToAction("PrviPredmetnik", "VpisniList", new { id = vl.id });
            else if (vl.letnikStudija==2)
                return RedirectToAction("DrugiPredmetnik", "VpisniList", new { id = vl.id });
            else if (vl.letnikStudija==3)
                return RedirectToAction("TretjiPredmetnik", "VpisniList", new { id = vl.id });
            else
                return RedirectToAction("NeznanPredmetnik", "VpisniList");
        }

        [Authorize(Roles = "Referent")]
        public ActionResult IzbiraLetnika(int id)
        {
            var s = db.students.Find(id);
            if (s == null) return HttpNotFound();

            // dodaj samo tiste, ki imajo vpisni list za 2 ali 3 letnik
            var vpisi = s.vpis.Where(a => (a.letnikStudija == 2 || a.letnikStudija == 3) && a.vrstaVpisa != 2);
            List<vpi> vplist = new List<vpi>();
            foreach (var v in vpisi)
            {
                vplist.Add(v);
            }

            ViewBag.Stevilo = vplist.Count();
            ViewBag.vpisni = vplist;
            ViewBag.student = s;

            return View();
        }

        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik2(int id)
        {   
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (!uh.jePredmetnikVzpostavljen(vl))
            {
                return RedirectToAction("DrugiPredmetnik", "VpisniList", new { id = vl.id });
            }
            else 
            {
                PredmetHelper ph = new PredmetHelper();

                //obvezni plus 1 strokovno izbirni plus 1 prosto izbirni
                ViewBag.obvezniPredmeti = ph.obvezni2();
                ViewBag.strokovnoPredmeti = ph.strokovnoizbirni2();
                ViewBag.prostoPredmeti = ph.prostoizbirni2();

                int sumObv = ph.getKreditObv2();

                ViewBag.sumObv = sumObv;
                ViewBag.sumIzb = 60 - sumObv;

                // kateri strokovni predmet ima izbran                                               
                var sp = vl.izvajanjes.Where(v => v.predmet.strokovnoizbirni == true).First();
                ViewBag.Strokovni = sp.predmetId;

                // katere prosto izbirne predmete ima izbrane
                ViewBag.Prosto = vl.izvajanjes.Where(v => v.predmet.prostoizbirni == true).ToList();

                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik2(DrugiPredmetnikModel model, int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
            int kreditne = 60 - ph.getKreditObv2();
            List<predmet> dodaj_p = new List<predmet>();
            StudentHelper sh = new StudentHelper();

            int num_prosto = 0;
            int num_strok = 0;

            foreach (string key in Request.Form)
            {
                if (key.StartsWith("prosto_"))
                {
                    int k = Convert.ToInt32(Request.Form[key]);
                    predmet p = db.predmets.Where(a => a.id == k).First();
                    if (p != null)
                    {
                        dodaj_p.Add(p);
                        kreditne -= p.kreditne;
                        num_prosto++;
                    }
                }
                else if (key == "strokovni")
                {
                    int k = Convert.ToInt32(Request.Form[key]);
                    predmet p = db.predmets.Where(a => a.id == k).First();
                    if (p != null)
                    {
                        dodaj_p.Add(p);
                        kreditne -= p.kreditne;
                        num_strok++;
                    }
                }
            }

            if (num_strok == 0 || num_prosto == 0)
            {
                // kateri strokovni predmet ima izbran                                                
                var sp = vl.izvajanjes.Where(v => v.predmet.strokovnoizbirni == true).First();
                ViewBag.Strokovni = sp.predmetId;

                // katere prosto izbirne predmete ima izbrane
                ViewBag.Prosto = vl.izvajanjes.Where(v => v.predmet.prostoizbirni == true).ToList();

                TempData["error"] = "Izbrati morate vsaj en strokovno ali prosto izbirni predmet";
                return RedirectToAction("UrediPredmetnik2", new { id = id });
            }

            if (kreditne == 0)
            {
                // odstrani vse
                try
                {
                    vl.izvajanjes.Clear();
                    db.SaveChanges();
                }
                catch
                {
                    TempData["error"] = "Prišlo je do napake pri urejanju podatkov v bazi!";
                    return RedirectToAction("UrediPredmetnik2", new { id = id });
                }            
                    
                // dodaj obvezne
                foreach (var o in ph.obvezni2())
                {
                    //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                    var izlist = db.izvajanjes.Where(a => a.predmetId == o.id).ToList();
                    bool breakk = false;
                    foreach (var i in izlist)
                    {
                        if (breakk) break;
                        foreach (var il in i.izvajanjeletoes)
                            if (il.studijskoletoId == vl.studijskoLeto)
                            {
                                vl.izvajanjes.Add(i);
                                breakk = true;
                                break;
                            }
                    }
                }

                // dodaj izbirne
                foreach (var p in dodaj_p)
                {
                    System.Diagnostics.Debug.WriteLine("--" + p.ime + p.id.ToString());
                    //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                    var izlist = db.izvajanjes.Where(a => a.predmetId == p.id).ToList();
                    bool breakk = false;
                    foreach (var i in izlist)
                    {
                        if (breakk) break;
                        foreach (var il in i.izvajanjeletoes)
                        {
                            System.Diagnostics.Debug.WriteLine("---izvajanje leto " + il.studijskoletoId);
                            if (il.studijskoletoId == vl.studijskoLeto)
                            {
                                vl.izvajanjes.Add(i);
                                breakk = true;
                                System.Diagnostics.Debug.WriteLine("---" + i.predmet.ime);
                                break;
                            }
                        }
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("napaka " + ex.Message);
                    return HttpNotFound();
                }             

                TempData["id"] = id;
                return RedirectToAction("VpisniListSuccess");
            }
            else
            {
                // kateri strokovni predmet ima izbran                                               
                var sp = vl.izvajanjes.Where(v => v.predmet.strokovnoizbirni == true).First();
                ViewBag.Strokovni = sp.predmetId;

                // katere prosto izbirne predmete ima izbrane
                ViewBag.Prosto = vl.izvajanjes.Where(v => v.predmet.prostoizbirni == true).ToList();

                TempData["error"] = "Nepravilno število izbranih kreditnih točk";
                return RedirectToAction("UrediPredmetnik2", new { id = id });
            }
        }

        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik3(int id)
        {
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (!uh.jePredmetnikVzpostavljen(vl))
            {
                //če je povprečna ocena 8 ali več si prosto izbira, sicer izbere 2 modula plus en izbirni plus diploma obvezni
                if (uh.preveriPovprecje(vl.student))
                {
                    return RedirectToAction("TretjiPredmetnikProsti", new { id = id });
                }
                else
                {
                    return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
                }
            }
            else
            {
                //če je povprečna ocena 8 ali več si prosto izbira, sicer izbere 2 modula plus en izbirni plus diploma obvezni
                if (uh.preveriPovprecje(vl.student))
                {
                    return RedirectToAction("UrediPredmetnik3Prosti", new { id = id });
                }
                else
                {
                    return RedirectToAction("UrediPredmetnik3Moduli", new { id = id });
                }
            }
        }

        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik3Prosti(int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce ima povprecje
            UserHelper uh = new UserHelper();
            if (!uh.preveriPovprecje(vl.student)) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();

            //obvezni plus vsi predmeti iz modula na izbiro
            ViewBag.obvezniPredmeti = ph.obvezni3();
            ViewBag.izbirniPredmeti = db.moduls.ToList();

            int sumObv = ph.getKreditObv3();

            ViewBag.sumObv = sumObv;
            ViewBag.sumIzb = 60 - sumObv;

            // označi tiste ki so že izbrani
            //var sp = vl.izvajanjes.Where(v => v.predmet.modul != null).First();
            ViewBag.Modul = vl.izvajanjes.Where(v => v.predmet.modul != null); 

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik3Prosti(TretjiPredmetnikProstiModel model, int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce ima povprecje
            UserHelper uh = new UserHelper();
            if (!uh.preveriPovprecje(vl.student)) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
            StudentHelper sh = new StudentHelper();
            int kreditne = 60 - ph.getKreditObv3();
            List<predmet> dodaj_p = new List<predmet>();

            foreach (string key in Request.Form)
            {
                if (key.StartsWith("prosto_"))
                {
                    int k = Convert.ToInt32(Request.Form[key]);
                    predmet p = db.predmets.Where(a => a.id == k).First();
                    if (p != null)
                    {
                        dodaj_p.Add(p);
                        kreditne -= p.kreditne;
                    }
                }
            }

            if (kreditne == 0)
            {
                // odstrani vse
                try
                {
                    vl.izvajanjes.Clear();
                    db.SaveChanges();
                }
                catch
                {
                    TempData["error"] = "Prišlo je do napake pri urejanju podatkov v bazi!";
                    return RedirectToAction("UrediPredmetnik3Prosti", new { id = id });
                }

                //dodaj obvezne
                foreach (var o in ph.obvezni3())
                {
                    //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                    var izlist = db.izvajanjes.Where(a => a.predmetId == o.id).ToList();
                    bool breakk = false;
                    foreach (var i in izlist)
                    {
                        if (breakk) break;
                        foreach (var il in i.izvajanjeletoes)
                            if (il.studijskoletoId == vl.studijskoLeto)
                            {
                                vl.izvajanjes.Add(i);
                                breakk = true;
                            }
                    }
                }

                //dodaj izbirne
                foreach (var p in dodaj_p)
                {
                    //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                    var izlist = db.izvajanjes.Where(a => a.predmetId == p.id).ToList();
                    bool breakk = false;
                    foreach (var i in izlist)
                    {
                        if (breakk) break;
                        foreach (var il in i.izvajanjeletoes)
                            if (il.studijskoletoId == vl.studijskoLeto)
                            {
                                vl.izvajanjes.Add(i);
                                breakk = true;
                            }
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return HttpNotFound();
                }

                TempData["id"] = id;
                return RedirectToAction("VpisniListSuccess");
            }
            else
            {
                // označi tiste ki so že izbrani
                ViewBag.Modul = vl.izvajanjes.Where(v => v.predmet.modul != null); 
                //ViewBag.Modul = db.studentinpredmets.Where(v => v.vpisId == id && v.predmet.modul != null); 

                TempData["error"] = "Nepravilno število izbranih kreditnih točk";
                return RedirectToAction("UrediPredmetnik3Prosti", new { id = id });
            }
        }

        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik3Moduli(int id)
        {

            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce ima povprecje
            UserHelper uh = new UserHelper();
            if (uh.preveriPovprecje(vl.student)) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();

            //dva modula plus en izbirni
            ViewBag.obvezniPredmeti = ph.obvezni3();
            ViewBag.moduli = db.moduls.ToList();
            ViewBag.vlid = id;

            int sumObv = ph.getKreditObv3();

            ViewBag.sumObv = sumObv;
            ViewBag.sumIzb = 60 - sumObv;

            // oznaci tiste ki so že izbrani
            //var izbrani = db.studentinpredmets.Where(v => v.vpisId == id && v.predmet.modul != null).ToList();
            var izbrani = vl.izvajanjes.Where(v => v.predmet.modul != null).ToList();
            ViewBag.Oznaci = izbrani;

            // ne oznaci tistega modula, ki ima izbirni predmet
            var prost = izbrani.Select(v => v.predmet.modulId).Distinct().ToList();
            var stevilo = Convert.ToInt32(prost.First());
            ViewBag.NeOznaci = stevilo;

            var i = izbrani.Where(v => v.predmet.modulId == stevilo).Select(v => v.predmet.id).ToList().First();
            var items = ph.izbirni3();
            ViewBag.izbirniPredmeti = new SelectList(items, "id", "ime", items.Where(x => x.id == i).ToList().First().id);
            
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik3Moduli(TretjiPredmetnikModuliModel model, int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
            StudentHelper sh = new StudentHelper();
            int kreditne = 60 - ph.getKreditObv3();
            List<predmet> dodaj_p = new List<predmet>();
            List<modul> moduli = new List<modul>();

            //moduli
            foreach (string key in Request.Form)
            {
                if (key.StartsWith("modul_"))
                {
                    int k = Convert.ToInt32(Request.Form[key]);
                    modul m = db.moduls.Where(a => a.id == k).First();
                    moduli.Add(m);
                    if (m != null)
                    {
                        dodaj_p.AddRange(m.predmets.ToList());
                        foreach (var p in m.predmets)
                            kreditne -= p.kreditne;
                    }
                }
            }

            //dodatni predmet
            predmet d = db.predmets.Find(model.izbirni);
            if (ModelState.IsValid)
            {
                if (d != null)
                {
                    dodaj_p.Add(d);
                    kreditne -= d.kreditne;
                }
                else
                {
                    // oznaci tiste ki so že izbrani
                    //var izbrani = db.studentinpredmets.Where(v => v.vpisId == id && v.predmet.modul != null).ToList();
                    var izbrani = vl.izvajanjes.Where(v => v.predmet.modul != null).ToList();
                    ViewBag.Oznaci = izbrani;

                    // ne oznaci tistega modula, ki ima izbirni predmet
                    var prost = izbrani.Select(v => v.predmet.modulId).Distinct().ToList();
                    ViewBag.NeOznaci = Convert.ToInt32(prost.First());
                    

                    TempData["error"] = "Dodatni predmet ne obstaja";
                    return RedirectToAction("UrediPredmetnik3Moduli", new { id = id });
                }

            }
            else
            {
                // oznaci tiste ki so že izbrani
                //var izbrani = db.studentinpredmets.Where(v => v.vpisId == id && v.predmet.modul != null).ToList();
                var izbrani = vl.izvajanjes.Where(v => v.predmet.modul != null).ToList();
                ViewBag.Oznaci = izbrani;

                // ne oznaci tistega modula, ki ima izbirni predmet
                var prost = izbrani.Select(v => v.predmet.modulId).Distinct().ToList();
                ViewBag.NeOznaci = Convert.ToInt32(prost.First());
                

                TempData["error"] = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList().First().First().ErrorMessage;
                return RedirectToAction("UrediPredmetnik3Moduli", new { id = id });
            }

            if (kreditne == 0)
            {
                //preveri da dodaten predmet ni v modulih
                if (ph.preveriIzbirne3(moduli, d))
                {
                    // odstrani vse
                    try
                    {
                        vl.izvajanjes.Clear();
                        db.SaveChanges();
                    }
                    catch
                    {
                        TempData["error"] = "Prišlo je do napake pri urejanju podatkov v bazi!";
                        return RedirectToAction("UrediPredmetnik3Moduli", new { id = id });
                    }

                    //dodaj obvezne
                    foreach (var o in ph.obvezni3())
                    {
                        //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                        var izlist = db.izvajanjes.Where(a => a.predmetId == o.id).ToList();
                        bool breakk = false;
                        foreach (var i in izlist)
                        {
                            if (breakk) break;
                            foreach (var il in i.izvajanjeletoes)
                                if (il.studijskoletoId == vl.studijskoLeto)
                                {
                                    vl.izvajanjes.Add(i);
                                    breakk = true;
                                }
                        }
                    }

                    foreach (var p in dodaj_p)
                    {
                        //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                        var izlist = db.izvajanjes.Where(a => a.predmetId == p.id).ToList();
                        bool breakk = false;
                        foreach (var i in izlist)
                        {
                            if (breakk) break;
                            foreach (var il in i.izvajanjeletoes)
                                if (il.studijskoletoId == vl.studijskoLeto)
                                {
                                    vl.izvajanjes.Add(i);
                                    breakk = true;
                                }
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        return HttpNotFound();
                    }

                    TempData["id"] = id;
                    return RedirectToAction("VpisniListSuccess");
                }
                else
                {
                    // oznaci tiste ki so že izbrani
                    //var izbrani = db.studentinpredmets.Where(v => v.vpisId == id && v.predmet.modul != null).ToList();
                    var izbrani = vl.izvajanjes.Where(v => v.predmet.modul != null).ToList();
                    ViewBag.Oznaci = izbrani;

                    // ne oznaci tistega modula, ki ima izbirni predmet
                    var prost = izbrani.Select(v => v.predmet.modulId).Distinct().ToList();
                    ViewBag.NeOznaci = Convert.ToInt32(prost.First());
                    
                    
                    TempData["error"] = "Dodatni predmet je že del enega izmed modulov";
                    return RedirectToAction("UrediPredmetnik3Moduli", new { id = id });
                }
            }
            else
            {
                // oznaci tiste ki so že izbrani
                // var izbrani = db.studentinpredmets.Where(v => v.vpisId == id && v.predmet.modul != null).ToList();
                var izbrani = vl.izvajanjes.Where(v => v.predmet.modul != null).ToList();
                ViewBag.Oznaci = izbrani;

                // ne oznaci tistega modula, ki ima izbirni predmet
                var prost = izbrani.Select(v => v.predmet.modulId).Distinct().ToList();
                ViewBag.NeOznaci = Convert.ToInt32(prost.First());
                

                TempData["error"] = "Nepravilno število izbranih kreditnih točk";
                return RedirectToAction("UrediPredmetnik3Moduli", new { id = id });
            }
        }

        public ActionResult PrviPredmetnik(int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen, vshe 60kt
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
            PrviPredmetnikModel m = new PrviPredmetnikModel();
            m.vlid = id;
            ViewBag.predmeti = ph.obvezni1();
            return View(m);
        }

        [HttpPost]
        public ActionResult PrviPredmetnik(PrviPredmetnikModel model)
        {
            var vl = db.vpis.Find(model.vlid);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen, ne le delni ampak vseh 60kt
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
            StudentHelper sh = new StudentHelper();

            //shrani predmetnik
            foreach (var p in ph.obvezni1())
            {
                //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu, dodamo le eno!
                var izlist = db.izvajanjes.Where(a => a.predmetId == p.id).ToList();
                bool breakk = false;
                foreach (var i in izlist)
                {
                    if (breakk) break;
                    foreach (var il in i.izvajanjeletoes)
                        if (il.studijskoletoId == vl.studijskoLeto)
                        {
                            vl.izvajanjes.Add(i);
                            breakk = true;
                        }
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return HttpNotFound();
            }

            TempData["id"] = model.vlid;
            return RedirectToAction("VpisniListSuccess");
        }

        public ActionResult DrugiPredmetnik(int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null)
            {
                System.Diagnostics.Debug.WriteLine("vpisni list ne obstaja");
                return HttpNotFound();
            }

            //preveri ce je predmetnik ze bil izpolnjen, vseh 60kt
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl))
            {
                System.Diagnostics.Debug.WriteLine("predmetnik je ze vzpostavljen");
                return HttpNotFound();
            }

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            if (!User.IsInRole("Referent"))
            {
                my_aspnet_users usr = uh.FindByName(User.Identity.Name);
                if (vl.student.userId != usr.id || vl.letnikStudija != 2)
                {
                    System.Diagnostics.Debug.WriteLine("nimate dostopa");
                    return HttpNotFound();
                }
            }
            
            PredmetHelper ph = new PredmetHelper();

            //obvezni plus 1 strokovno izbirni plus 1 prosto izbirni
            ViewBag.obvezniPredmeti = ph.obvezni2();
            ViewBag.strokovnoPredmeti = ph.strokovnoizbirni2();
            ViewBag.prostoPredmeti = ph.prostoizbirni2();

            int sumObv = ph.getKreditObv2();

            ViewBag.sumObv = sumObv;
            ViewBag.sumIzb = 60 - sumObv;

            return View();
        }

        [HttpPost]
        public ActionResult DrugiPredmetnik(DrugiPredmetnikModel model, int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null)
            {
                System.Diagnostics.Debug.WriteLine("vpisni list ne obstaja");
                return HttpNotFound();
            }

            //preveri ce je predmetnik ze bil izpolnjen, vseh 60kt
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl))
            {
                System.Diagnostics.Debug.WriteLine("predmetnik je ze vzpostavljen");
                return HttpNotFound();
            }

            // preveri ce je trenutni user referent
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (usr.name != "referent")
            {
                //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
                if (vl.student.userId != usr.id || vl.letnikStudija != 2) 
                {
                    System.Diagnostics.Debug.WriteLine("nimate dostopa");
                    return HttpNotFound();
                }
            }
            
            PredmetHelper ph = new PredmetHelper();
            StudentHelper sh = new StudentHelper();
            int kreditne = 60 - ph.getKreditObv2();
            List<predmet> dodaj_p = new List<predmet>();

            int num_prosto = 0;
            int num_strok = 0;

            foreach(string key in Request.Form) {
                if (key.StartsWith("prosto_")) {
                    int k = Convert.ToInt32(Request.Form[key]);
                    predmet p = db.predmets.Where(a => a.id == k).First();
                    if (p != null) {
                        dodaj_p.Add(p);
                        kreditne -= p.kreditne;
                        num_prosto++;
                    }
                }
                else if (key == "strokovni")
                {
                    int k = Convert.ToInt32(Request.Form[key]);
                    predmet p = db.predmets.Where(a => a.id == k).First();
                    if (p != null) {
                        dodaj_p.Add(p);
                        kreditne -= p.kreditne;
                        num_strok++;
                    }
                }
            }

            if (num_strok == 0 || num_prosto == 0)
            {
                TempData["error"] = "Izbrati morate vsaj en strokovno ali prosto izbirni predmet";
                return RedirectToAction("DrugiPredmetnik", new { id = id });
            }

            if (kreditne == 0) {
                //dodaj obvezne
                //ce je delni potem so ze noter
                if (!uh.jeDelniPredmetnikVzpostavljen(vl))
                {
                    foreach (var o in ph.obvezni2())
                    {
                        //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                        var izlist = db.izvajanjes.Where(a => a.predmetId == o.id).ToList();
                        bool breakk = false;
                        foreach (var i in izlist)
                        {
                            if (breakk) break;
                            foreach (var il in i.izvajanjeletoes)
                                if (il.studijskoletoId == vl.studijskoLeto)
                                {
                                    vl.izvajanjes.Add(i);
                                    breakk = true;
                                    break;
                                }
                        }
                    }
                }

                //dodaj izbirne
                foreach (var p in dodaj_p) {
                    System.Diagnostics.Debug.WriteLine("--"+p.ime+p.id.ToString());
                    //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                    var izlist = db.izvajanjes.Where(a => a.predmetId == p.id).ToList();
                    bool breakk = false;
                    foreach (var i in izlist)
                    {
                        if (breakk) break;
                        foreach (var il in i.izvajanjeletoes)
                        {
                            System.Diagnostics.Debug.WriteLine("---izvajanje leto " + il.studijskoletoId);
                            if (il.studijskoletoId == vl.studijskoLeto)
                            {
                                vl.izvajanjes.Add(i);
                                breakk = true;
                                System.Diagnostics.Debug.WriteLine("---" + i.predmet.ime);
                                break;
                            }
                        }
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("napaka "+ex.Message);
                    return HttpNotFound();
                }

                TempData["id"] = id;
                return RedirectToAction("VpisniListSuccess");
            }
            else {
                TempData["error"] = "Nepravilno število izbranih kreditnih točk";
                return RedirectToAction("DrugiPredmetnik", new { id = id } );
            }

        }

        public ActionResult TretjiPredmetnik(int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();


            //če je povprečna ocena 8 ali več si prosto izbira, sicer izbere 2 modula plus en izbirni plus diploma obvezni
            if (uh.preveriPovprecje(vl.student)) return RedirectToAction("TretjiPredmetnikProsti", new { id = id });
            else return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
        }

        public ActionResult TretjiPredmetnikProsti(int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            if (!User.IsInRole("Referent"))
            {
                my_aspnet_users usr = uh.FindByName(User.Identity.Name);
                if (vl.student.userId != usr.id || vl.letnikStudija != 3) return HttpNotFound();
            }
            
            //preveri ce ima povprecje
            if (!uh.preveriPovprecje(vl.student)) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();

            //obvezni plus vsi predmeti iz modula na izbiro
            ViewBag.obvezniPredmeti = ph.obvezni3();
            ViewBag.izbirniPredmeti = db.moduls.ToList();

            int sumObv = ph.getKreditObv3();

            ViewBag.sumObv = sumObv;
            ViewBag.sumIzb = 60 - sumObv;

            return View();
        }

        [HttpPost]
        public ActionResult TretjiPredmetnikProsti(TretjiPredmetnikProstiModel model, int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            //preveri ce je trenutni user referent
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (usr.name != "referent")
            {
                //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
                if (vl.student.userId != usr.id || vl.letnikStudija != 3) return HttpNotFound();
            }

            //preveri ce ima povprecje
            if (!uh.preveriPovprecje(vl.student)) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
            StudentHelper sh = new StudentHelper();
            int kreditne = 60 - ph.getKreditObv3();
            List<predmet> dodaj_p = new List<predmet>();

            foreach (string key in Request.Form)
            {
                if (key.StartsWith("prosto_"))
                {
                    int k = Convert.ToInt32(Request.Form[key]);
                    predmet p = db.predmets.Where(a => a.id == k).First();
                    if (p != null)
                    {
                        dodaj_p.Add(p);
                        kreditne -= p.kreditne;
                    }
                }
            }

            if (kreditne == 0)
            {
                //dodaj obvezne
                //ce je delni potem so ze noter
                if (!uh.jeDelniPredmetnikVzpostavljen(vl))
                {
                    foreach (var o in ph.obvezni3())
                    {
                        //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                        var izlist = db.izvajanjes.Where(a => a.predmetId == o.id).ToList();
                        bool breakk = false;
                        foreach (var i in izlist)
                        {
                            if (breakk) break;
                            foreach (var il in i.izvajanjeletoes)
                                if (il.studijskoletoId == vl.studijskoLeto)
                                {
                                    vl.izvajanjes.Add(i);
                                    breakk = true;
                                }
                        }
                    }
                }
                //dodaj izbirne
                foreach (var p in dodaj_p)
                {
                    //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                    var izlist = db.izvajanjes.Where(a => a.predmetId == p.id).ToList();
                    bool breakk = false;
                    foreach (var i in izlist)
                    {
                        if (breakk) break;
                        foreach (var il in i.izvajanjeletoes)
                            if (il.studijskoletoId == vl.studijskoLeto)
                            {
                                vl.izvajanjes.Add(i);
                                breakk = true;
                            }
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return HttpNotFound();
                }

                TempData["id"] = id;
                return RedirectToAction("VpisniListSuccess");
            }
            else
            {
                TempData["error"] = "Nepravilno število izbranih kreditnih točk";
                return RedirectToAction("TretjiPredmetnikProsti", new { id = id });
            }

        }

        public ActionResult TretjiPredmetnikModuli(int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            if (!User.IsInRole("Referent"))
            {
                my_aspnet_users usr = uh.FindByName(User.Identity.Name);
                if (vl.student.userId != usr.id) return HttpNotFound();
            }
            
            //preveri ce ima povprecje
            if (uh.preveriPovprecje(vl.student)) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();

            //dva modula plus en izbirni
            ViewBag.obvezniPredmeti = ph.obvezni3();
            ViewBag.izbirniPredmeti = new SelectList(ph.izbirni3(), "id", "ime");
            ViewBag.moduli = db.moduls.ToList();
            ViewBag.vlid = id;

            int sumObv = ph.getKreditObv3();

            ViewBag.sumObv = sumObv;
            ViewBag.sumIzb = 60 - sumObv;

            return View();
        }

        [HttpPost]
        public ActionResult TretjiPredmetnikModuli(TretjiPredmetnikModuliModel model, int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            
           //preveri ce je trenutni user referent
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (usr.name != "referent")
            {
                //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
                if (vl.student.userId != usr.id || vl.letnikStudija != 3) return HttpNotFound();
            }

            PredmetHelper ph = new PredmetHelper();
            StudentHelper sh = new StudentHelper();
            int kreditne = 60 - ph.getKreditObv3();
            List<predmet> dodaj_p = new List<predmet>();
            List<modul> moduli = new List<modul>();

            //moduli
            foreach (string key in Request.Form)
            {
                if (key.StartsWith("modul_"))
                {
                    int k = Convert.ToInt32(Request.Form[key]);
                    modul m = db.moduls.Where(a => a.id == k).First();
                    moduli.Add(m);
                    if (m != null)
                    {
                        dodaj_p.AddRange(m.predmets.ToList());
                        foreach (var p in m.predmets)
                            kreditne -= p.kreditne;
                    }
                }
            }

            //dodatni predmet
            predmet d = db.predmets.Find(model.izbirni);
            if (ModelState.IsValid)
            {
                if (d != null)
                {
                    dodaj_p.Add(d);
                    kreditne -= d.kreditne;
                }
                else
                {
                    TempData["error"] = "Dodatni predmet ne obstaja";
                    return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
                }

            }
            else
            {
                TempData["error"] = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList().First().First().ErrorMessage;
                return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
            }

            if (kreditne == 0)
            {
                //preveri da dodaten rpedmet ni v modulih
                if (ph.preveriIzbirne3(moduli, d)) {
                    //dodaj obvezne
                    //ce je delni potem so ze noter
                    if (!uh.jeDelniPredmetnikVzpostavljen(vl))
                    {
                        foreach (var o in ph.obvezni3())
                        {
                            //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                            var izlist = db.izvajanjes.Where(a => a.predmetId == o.id).ToList();
                            bool breakk = false;
                            foreach (var i in izlist)
                            {
                                if (breakk) break;
                                foreach (var il in i.izvajanjeletoes)
                                    if (il.studijskoletoId == vl.studijskoLeto)
                                    {
                                        vl.izvajanjes.Add(i);
                                        breakk = true;
                                    }
                            }
                        }
                    }
                    //dodaj izbirne
                    foreach (var p in dodaj_p)
                    {
                        //poiščemo izvajanje pri tem predmetu ki se izvaja v prihodnjem šolskem letu
                        var izlist = db.izvajanjes.Where(a => a.predmetId == p.id).ToList();
                        bool breakk = false;
                        foreach (var i in izlist)
                        {
                            if (breakk) break;
                            foreach (var il in i.izvajanjeletoes)
                                if (il.studijskoletoId == vl.studijskoLeto)
                                {
                                    vl.izvajanjes.Add(i);
                                    breakk = true;
                                }
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return HttpNotFound();
                    }

                    TempData["id"] = id;
                    return RedirectToAction("VpisniListSuccess");
                }
                else {
                    TempData["error"] = "Dodatni predmet je že del enega izmed modulov";
                return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
                }
            }
            else
            {
                TempData["error"] = "Nepravilno število izbranih kreditnih točk";
                return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
            }

        }

        public JsonResult PreveriEmso(string emso)
        {
            var result = Validate.isEmso(emso);
            return Json(result);
        }

        public JsonResult PreveriIme(string ime)
        {
            var result = Validate.veljavnoIme(ime);
            return Json(result);
        }

        public JsonResult PreveriPriimek(string priimek)
        {
            var result = Validate.veljavnoIme(priimek);
            return Json(result);
        }

        public JsonResult PreveriDatum(DateTime datumRojstva)
        {
            System.Diagnostics.Debug.WriteLine("Preveri datum metoda");
            var result= Validate.veljavenDatum(datumRojstva);
            return Json(result);
        }

        public JsonResult PreveriVrstaVpisa(int vrstaVpisa)
        {
            bool result = true;
            if (User.IsInRole("Študent"))
            {
                if (vrstaVpisa < 1 || vrstaVpisa > 7)
                    result = false;
            }
            return Json(result);
        }

        public JsonResult PreveriDrzavoInObcino(int obcinaRojstva, int drzavaRojstva)
        {
            bool result = false;
            if (drzavaRojstva == 705 && obcinaRojstva != -1)
            {
                result = true;
            }
            else if (drzavaRojstva != 705 && obcinaRojstva == -1)
            {
                result = true;
            }
            return Json(result);
        }

        public JsonResult PreveriDrzavoInObcinoInPostno(int drzava, int obcina, int postnaStevilka)
        {
            bool result = false;
            if (drzava == 705 && obcina != -1 && postnaStevilka != -1)
            {
                result = true;
            }
            else if (drzava != 705 && obcina == -1 && postnaStevilka == -1)
            {
                result = true;
            }
            return Json(result);
        }

        public JsonResult PreveriDrzavoInObcinoInPostno2(int drzavaZacasni, int obcinaZacasni, int postnaStevilkaZacasni)
        {
            int drzava = drzavaZacasni;
            int obcina = obcinaZacasni;
            int postnaStevilka = postnaStevilkaZacasni;
            bool result = false;
            if (drzava == 705 && obcina != -1 && postnaStevilka != -1)
            {
                result = true;
            }
            else if (drzava != 705 && obcina == -1 && postnaStevilka == -1)
            {
                result = true;
            }
            return Json(result);
        }
    }
}