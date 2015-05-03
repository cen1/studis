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
        public ActionResult VpisniList()
        {
            UserHelper uh = new UserHelper();
            //poglej ce obstaja vnos v tabeli student
            var sid = uh.FindByName(User.Identity.Name).students.FirstOrDefault();
            if (sid != null) {
                ViewBag.zetoniVB = sid.zetons.Where(a => a.porabljen == false);
                //poglej ce ima zeton
                if (!uh.imaZeton(sid))
                {
                    return RedirectToAction("NiZetona");
                }
            }

            ViewBag.StudijskiProgramiVB = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.KlasiusVB = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.VrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa.Where(a => a.id != 98), "id", "naziv");
            ViewBag.NacinStudijaVB = new SelectList(db.sifrant_nacinstudija, "id", "naziv");
            ViewBag.OblikaStudijaVB = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.SpolVB = new SelectList(db.sifrant_spol.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.ObcinaVB = new SelectList(db.sifrant_obcina.OrderBy(a => a.naziv != "Ljubljana").ThenBy(a => a.naziv), "id", "naziv");
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
                //prvi vpis, napolni iz tabele kandidat
                VpisniListModel model = Baza.getVpisniListKandidat(uh.FindByName(User.Identity.Name));
                ViewBag.zacasnipodatkiVB = false;
                return View(model);
            }
        }

        public ActionResult NiZetona()
        {
            return View();
		}
		
        [Authorize(Roles = "Referent")]
        public ActionResult VpisniListEdit(int id)
        {
            ViewBag.StudijskiProgrami = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.id != 1000468).ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.Klasius = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.VrstaVpisa = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            ViewBag.NacinStudija = new SelectList(db.sifrant_nacinstudija, "id", "naziv");
            ViewBag.OblikaStudija = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.Spol = new SelectList(db.sifrant_spol.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.Obcina = new SelectList(db.sifrant_obcina.OrderBy(a => a.naziv != "Ljubljana").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.Drzava = new SelectList(db.sifrant_drzava.OrderBy(a => a.naziv != "Slovenija").ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.PostnaStevilka = new SelectList(db.sifrant_postnastevilka.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.Letnik = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.StudijskoLetoPrvegaVpisa = new SelectList(db.sifrant_studijskoletoprvegavpisa.OrderByDescending(a => a.id), "id", "naziv");
            ViewBag.IzbirnaSkupina = new SelectList(db.sifrant_izbirnaskupina, "id", "naziv");
            ViewBag.Smer = new SelectList(db.sifrant_smer, "id", "naziv");

            Baza bz = new Baza();
            VpisniListModel model = bz.getVpisniList(id);

            return View(model);
        }
        
        
        [HttpPost]
        public ActionResult VpisniList(studis.Models.VpisniListModel model)
        {
            UserHelper uh = new UserHelper();

            //poglej ce obstaja vnos v tabeli student
            var sid = uh.FindByName(User.Identity.Name).students.FirstOrDefault();
            if (sid != null)
            {
                //poglej ce ima zeton
                if (!uh.imaZeton(sid))
                {
                    return RedirectToAction("NiZetona");
                }
                else
                {
                    //poglej ce se vsaj en zeton ujema z oddanimi podatki
                    if (!uh.preveriZeton(sid, model))
                    {
                        return RedirectToAction("NiZetona");
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
            List<string> logic = uh.preveriLogiko(model, sid, uh.FindByName(User.Identity.Name));
            if (logic.Count() != 0)
            {
                foreach (var l in logic)
                {
                    ModelState.AddModelError("", l);
                }
            }

            if (ModelState.IsValid)
            {
                vpi v = new vpi();

                //ni prvi vpis
                if (sid != null) {
                    
                    v.vpisnaStevilka = sid.vpisnaStevilka;

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

                    db.vpis.Add(v);
                }
                else { //prvi vpis, loceno je treba naredit vnos za studenta
                    student s = new student();
                    
                    s.userId = uh.FindByName(User.Identity.Name).id;
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

                    v.potrjen = false;
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
                

                return RedirectToAction("VpisniListPredmetnik", "VpisniList", new { id = v.id });

            }
            else
            {
                ModelState.AddModelError("", "Prišlo je do napake.");
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                foreach (var e in errors)
                    foreach (var ee in e)
                        System.Diagnostics.Debug.WriteLine(ee.ErrorMessage);
            }

            //repopulate model lists
            ViewBag.StudijskiProgramiVB = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.KlasiusVB = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.VrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa.Where(a => a.id != 98), "id", "naziv");
            ViewBag.NacinStudijaVB = new SelectList(db.sifrant_nacinstudija, "id", "naziv");
            ViewBag.OblikaStudijaVB = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.SpolVB = new SelectList(db.sifrant_spol.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.ObcinaVB = new SelectList(db.sifrant_obcina.OrderBy(a => a.naziv != "Ljubljana").ThenBy(a => a.naziv), "id", "naziv");
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

        public ActionResult VpisniListSuccess()
        {
            ViewBag.id = TempData["id"];
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
            ViewBag.Id = id;
            return View();
        }

        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik2(int id)
        {
            // klic z vpisno stevilko
            if (id > 60000000)
            {
                try
                {
                    var tmp = db.vpis.Where(v => v.vpisnaStevilka == id && v.letnikStudija == 2).ToList();
                    id = tmp.Last().id;
                }
                catch
                {
                    TempData["Napaka"] = "Študent nima izpolnjenega vpisnega lista za 3. letnik!";
                    return RedirectToAction("Napaka");
                }
            }

            var list = db.vpis.SingleOrDefault(v => v.id == id);

            // logika za prikaz predmetov za urejanje

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik2(int id)
        {

            // validacija

            return View();
        }

        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik3(int id)
        {
            // klic z vpisno stevilko
            if (id > 60000000)
            {
                try
                {
                    var tmp = db.vpis.Where(v => v.vpisnaStevilka == id && v.letnikStudija == 3).ToList();
                    id = tmp.Last().id;
                }
                catch
                {
                    TempData["Napaka"] = "Študent nima izpolnjenega vpisnega lista za 3. letnik!";
                    return RedirectToAction("Napaka");
                }
            }

            var list = db.vpis.SingleOrDefault(v => v.id == id);

            // logika za prikaz predmetov za urejanje

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Referent")]
        public ActionResult UrediPredmetnik3(int id)
        {

            // validacija

            return View();
        }

        public ActionResult PrviPredmetnik(int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
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

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();

            //shrani predmetnik
            foreach (var p in ph.obvezni1())
            {
                studentinpredmet sip = new studentinpredmet();
                sip.predmetId = p.id;
                sip.studentId = vl.vpisnaStevilka;
                sip.vpisId = vl.id;
                db.studentinpredmets.Add(sip);
            }

            db.SaveChanges();

            TempData["id"] = model.vlid;
            return RedirectToAction("VpisniListSuccess");
        }

        public ActionResult DrugiPredmetnik(int id)
        {
            //preveri ce vpisni sploh obstaja
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id || vl.letnikStudija != 2) return HttpNotFound();

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
            if (vl == null) return HttpNotFound();

            //preveri ce je predmetnik ze bil izpolnjen
            UserHelper uh = new UserHelper();
            if (uh.jePredmetnikVzpostavljen(vl)) return HttpNotFound();

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id || vl.letnikStudija != 2) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
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
                Session["error"] = "Izbrati morate vsaj en strokovno ali prosto izbirni predmet";
                return RedirectToAction("DrugiPredmetnik", new { id = id });
            }

            if (kreditne == 0) {
                //dodaj izbirne
                foreach (var p in dodaj_p) {
                    studentinpredmet sip = new studentinpredmet();
                    sip.predmetId = p.id;
                    sip.studentId = vl.student.vpisnaStevilka;
                    sip.vpisId = vl.id;
                    db.studentinpredmets.Add(sip);
                }
                //dodaj obvezne
                foreach (var o in ph.obvezni2())
                {
                    studentinpredmet sip = new studentinpredmet();
                    sip.predmetId = o.id;
                    sip.studentId = vl.student.vpisnaStevilka;
                    sip.vpisId = vl.id;
                    db.studentinpredmets.Add(sip);
                }

                db.SaveChanges();

                TempData["id"] = id;
                return RedirectToAction("VpisniListSuccess");
            }
            else {
                Session["error"] = "Nepravilno število izbranih kreditnih točk";
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
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id || vl.letnikStudija != 3) return HttpNotFound();

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

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id || vl.letnikStudija != 3) return HttpNotFound();

            //preveri ce ima povprecje
            if (!uh.preveriPovprecje(vl.student)) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
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
                //dodaj izbirne
                foreach (var p in dodaj_p)
                {
                    studentinpredmet sip = new studentinpredmet();
                    sip.predmetId = p.id;
                    sip.studentId = vl.student.vpisnaStevilka;
                    sip.vpisId = vl.id;
                    db.studentinpredmets.Add(sip);
                }
                //dodaj obvezne
                foreach (var o in ph.obvezni3())
                {
                    studentinpredmet sip = new studentinpredmet();
                    sip.predmetId = o.id;
                    sip.studentId = vl.student.vpisnaStevilka;
                    sip.vpisId = vl.id;
                    db.studentinpredmets.Add(sip);
                }

                db.SaveChanges();

                TempData["id"] = id;
                return RedirectToAction("VpisniListSuccess");
            }
            else
            {
                Session["error"] = "Nepravilno število izbranih kreditnih točk";
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
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id) return HttpNotFound();

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

            //preveri ce trenutni user sploh lahko dostopa do tega predmetnika
            my_aspnet_users usr = uh.FindByName(User.Identity.Name);
            if (vl.student.userId != usr.id || vl.letnikStudija != 3) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();
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
                    Session["error"] = "Dodatni predmet ne obstaja";
                    return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
                }

            }
            else
            {
                Session["error"] = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList().First().First().ErrorMessage;
                return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
            }

            if (kreditne == 0)
            {
                //preveri da dodaten rpedmet ni v modulih
                if (ph.preveriIzbirne3(moduli, d)) {

                    //dodaj izbirne
                    foreach (var p in dodaj_p)
                    {
                        studentinpredmet sip = new studentinpredmet();
                        sip.predmetId = p.id;
                        sip.studentId = vl.student.vpisnaStevilka;
                        sip.vpisId = vl.id;
                        db.studentinpredmets.Add(sip);
                    }
                    //dodaj obvezne
                    foreach (var o in ph.obvezni3())
                    {
                        studentinpredmet sip = new studentinpredmet();
                        sip.predmetId = o.id;
                        sip.studentId = vl.student.vpisnaStevilka;
                        sip.vpisId = vl.id;
                        db.studentinpredmets.Add(sip);
                    }

                    db.SaveChanges();

                    TempData["id"] = id;
                    return RedirectToAction("VpisniListSuccess");
                }
                else {
                    Session["error"] = "Dodatni predmet je že del enega izmed modulov";
                return RedirectToAction("TretjiPredmetnikModuli", new { id = id });
                }
            }
            else
            {
                Session["error"] = "Nepravilno število izbranih kreditnih točk";
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