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
                ViewBag.zetoni = sid.zetons.Where(a => a.porabljen == false);
                //poglej ce ima zeton
                if (!uh.imaZeton(sid))
                {
                    return RedirectToAction("NiZetona");
                }
            }

            ViewBag.Title = "Vpisni List";
            ViewBag.StudijskiProgrami = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.Klasius = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.VrstaVpisa = new SelectList(db.sifrant_vrstavpisa.Where(a => a.id != 98), "id", "naziv");
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

            //napolnimo podatke ce se da
            if (sid != null) {
                int tid = sid.vpis.Last().id;
                Baza bz = new Baza();
                VpisniListModel model = bz.getVpisniList(tid);
                return View(model);
            }
            else
            {
                //prvi vpis, napolni iz tabele kandidat
                VpisniListModel model = Baza.getVpisniListKandidat(uh.FindByName(User.Identity.Name));
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
            ViewBag.Title = "Vpisni List";
            ViewBag.StudijskiProgrami = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
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
            DateTime temp_dr = new DateTime(); ;
            DateTime.TryParse(model.dr_mesec.ToString()+"/"+model.dr_dan.ToString() + "/" + model.dr_leto.ToString(), out temp_dr);
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
                    v.student = sid;

                    v.student.datumRojstva = model.datumRojstva;
                    v.student.davcnaStevilka = model.davcnaStevilka;
                    v.student.drzava = model.drzava;
                    v.student.drzavaRojstva = model.drzavaRojstva;
                    v.student.drzavaZacasni = model.drzavaZacasni;
                    v.student.drzavljanstvo = model.drzavljanstvo;
                    v.student.email = model.email;
                    v.student.emso = model.emso;
                    v.student.ime = model.ime;
                    v.izbirnaSkupina = model.izbirnaSkupina;
                    v.krajIzvajanja = model.krajIzvajanja;
                    v.student.krajRojstva = model.krajRojstva;
                    v.letnikStudija = model.letnikStudija;
                    v.nacinStudija = model.nacinStudija;
                    v.student.naslov = model.naslov;
                    v.student.naslovZacasni = model.naslovZacasni;
                    v.student.obcina = model.obcina;
                    v.student.obcinaRojstva = model.obcinaRojstva;
                    v.student.obcinaZacasni = model.obcinaZacasni;
                    v.oblikaStudija = model.oblikaStudija;
                    v.student.postnaStevilka = model.postnaStevilka;
                    v.student.postnaStevilkaZacasni = model.postnaStevilkaZacasni;
                    v.student.prenosniTelefon = model.prenosniTelefon;
                    v.student.priimek = model.priimek;
                    v.smer = model.smer;
                    v.soglasje1 = model.soglasje1;
                    v.soglasje2 = model.soglasje2;
                    v.student.spol = model.spol;
                    v.studijskiProgram = model.studijskiProgram;
                    v.studijskoLeto = Convert.ToInt32(model.studijskoLeto);
                    v.studijskoLetoPrvegaVpisa = model.studijskoLetoPrvegaVpisa;
                    v.student.vrocanje = model.vrocanje;
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
                    foreach (var z in sid.zetons)
                        z.porabljen = true;
                }

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
            ViewBag.Title = "Vpisni List";
            ViewBag.StudijskiProgrami = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            ViewBag.Klasius = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.VrstaVpisa = new SelectList(db.sifrant_vrstavpisa.Where(a => a.id != 98), "id", "naziv");
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

        public ActionResult PrviPredmetnik(int id)
        {
            var vl = db.vpis.Find(id);
            if (vl == null) return HttpNotFound();

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
            //var vl = db.vpis.Find(id);
            //if (vl == null) return HttpNotFound();

            PredmetHelper ph = new PredmetHelper();

            //obvezni plus 1 strokovno izbirni plus 1 prosto izbirni
            ViewBag.obvezniPredmeti = ph.obvezni2();
            ViewBag.strokovnoPredmeti = ph.strokovnoizbirni2();
            ViewBag.prostoPredmeti = ph.prostoizbirni2();

            int sumObv = 0;
            foreach (var pr in ph.obvezni2())
                sumObv += pr.kreditne;

            ViewBag.sumObv = sumObv;
            ViewBag.sumIzb = 60 - sumObv;

            var t = ph.obvezni2();
     
            return View();
        }

        public ActionResult TretjiPredmetnik(int id)
        {
            //var vl = db.vpis.Find(id);
            //if (vl == null) return HttpNotFound();

            //če je povprečna ocena 8 ali več si prosto izbira, sicer izbere 2 modula plus en izbirni plus diploma obvezni
            //if (UserHelper.preveriPovprecje()) return RedirectToAction("TretjiPredmetnikProsti");
            //else return RedirectToAction("TretjiPredmetnikModuli");
            return View();
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