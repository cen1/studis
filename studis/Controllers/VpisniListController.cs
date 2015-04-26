using studis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace studis.Controllers
{
    [Authorize(Roles = "Študent")]
    public class VpisniListController : Controller
    {
        public studisEntities db = new studisEntities();

        // GET: VpisniList
        public ActionResult VpisniList()
        {

            //poglej ce obstaja vnos v tabeli student in ce ja, poglej zeton
            //ce vnosa ni je prvi vpis in ni treba preverjat
            //var sid = studis.Models.UserHelper.FindByName(User.Identity.Name).students.FirstOrDefault();
            //if (sid != null) 

            if (false) //ni zetona
            {
                return View("");
            }

            ViewBag.Title = "VpisniList";
            ViewBag.StudijskiProgrami = new SelectList(Sifranti.STUDIJSKIPROGRAM, "id", "IdNaziv");
            ViewBag.Klasius = new SelectList(Sifranti.KLASIUS, "id", "naziv");
            ViewBag.VrstaVpisa = new SelectList(Sifranti.VRSTAVPISA, "id", "naziv");
            ViewBag.NacinStudija = new SelectList(Sifranti.NACINSTUDIJA, "id", "naziv");
            ViewBag.OblikaStudija = new SelectList(Sifranti.OBLIKASTUDIJA, "id", "naziv");
            ViewBag.Spol = new SelectList(Sifranti.SPOL, "id", "naziv");
            ViewBag.Obcina = new SelectList(Sifranti.OBCINE, "id", "naziv");
            ViewBag.Drzava = new SelectList(Sifranti.DRZAVE, "id", "naziv");
            ViewBag.PostnaStevilka = new SelectList(Sifranti.POSTNESTEVILKE, "id", "IdNaziv");
            ViewBag.Letnik = new SelectList(Sifranti.LETNIK, "id", "naziv");
            ViewBag.StudijskoLetoPrvegaVpisa = new SelectList(Sifranti.StudijskoLetoGenerator(DateTime.Now.Year, DateTime.Now.Year - 20), "id", "naziv");
            ViewBag.IzbirnaSkupina = new SelectList(Sifranti.IZBIRNASKUPINA, "id", "naziv");
            ViewBag.Smer = new SelectList(Sifranti.SMER, "id", "naziv");
            return View();
        }
        
        [HttpPost]
        public ActionResult VpisniList(studis.Models.VpisniListModel model)
        {
            model.studijskoLeto = "2014/2015";
            if (ModelState.IsValid)
            {
                    vpi v = new vpi();
                    v.student.datumRojstva = model.datumRojstva;
                    v.student.davcnaStevilka = model.davcnaStevilka;
                    v.student.drzava = model.davcnaStevilka;
                    v.student.drzavaRojstva = model.drzavaRojstva;
                    v.student.drzavaZacasni = model.drzavaZacasni;
                    v.student.drzavljanstvo = model.drzavljanstvo;
                    v.student.email = model.email;
                    v.student.emso = model.emso;
                    v.student.ime = model.ime;
                    v.izbirnaSkupina = model.izbirnaSkupina;
                    v.izbirnaSkupina2 = model.izbirnaSkupina2;
                    v.krajIzvajanja = model.krajIzvajanja;
                    v.krajIzvajanja2 = model.krajIzvajanja2;
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
                    v.smer2 = model.smer2;
                    v.soglasje1 = model.soglasje1;
                    v.soglasje2 = model.soglasje2;
                    v.student.spol = model.spol;
                    v.studijskiProgram = model.studijskiProgram;
                    v.studijskiProgram2 = model.studijskiProgram2;
                    v.studijskoLeto = Convert.ToInt32(model.studijskoLeto);
                    v.studijskoLetoPrvegaVpisa = model.studijskoLetoPrvegaVpisa;
                    v.student.vrocanje = model.vrocanje;
                    v.vrstaStudija = model.vrstaStudija;
                    v.vrstaVpisa = model.vrstaVpisa;

                    if (model.vrocanje) v.student.vrocanjeZacasni = false;
                    else v.student.vrocanjeZacasni = model.vrocanjeZacasni;
                    if (!model.vrocanje && !model.vrocanjeZacasni)
                    {
                        ModelState.AddModelError("", "Izberite naslov za vročanje.");
                        return View(model);
                    }

                    v.vpisnaStevilka = -1;
                    v.potrjen = false;
                    v.student = null;
                    v.studijskoLeto = DateTime.Now.Year;

                    db.vpis.Add(v);
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
            ViewBag.Title = "VpisniList";
            ViewBag.StudijskiProgrami = new SelectList(Sifranti.STUDIJSKIPROGRAM, "id", "naziv");
            ViewBag.Klasius = new SelectList(Sifranti.KLASIUS, "id", "naziv");
            ViewBag.VrstaVpisa = new SelectList(Sifranti.VRSTAVPISA, "id", "naziv");
            ViewBag.NacinStudija = new SelectList(Sifranti.NACINSTUDIJA, "id", "naziv");
            ViewBag.OblikaStudija = new SelectList(Sifranti.OBLIKASTUDIJA, "id", "naziv");
            ViewBag.Spol = new SelectList(Sifranti.SPOL, "id", "naziv");
            ViewBag.Obcina = new SelectList(Sifranti.OBCINE, "id", "naziv");
            ViewBag.Drzava = new SelectList(Sifranti.DRZAVE, "id", "naziv");
            ViewBag.PostnaStevilka = new SelectList(Sifranti.POSTNESTEVILKE, "id", "naziv");
            ViewBag.Letnik = new SelectList(Sifranti.LETNIK, "id", "naziv");
            ViewBag.StudijskoLetoPrvegaVpisa = new SelectList(Sifranti.STUDIJSKOLETO, "id", "naziv");
            ViewBag.IzbirnaSkupina = new SelectList(Sifranti.IZBIRNASKUPINA, "id", "naziv");

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

            PrviPredmetnikModel m = new PrviPredmetnikModel();
            m.vlid = id;
            ViewBag.predmeti = PredmetHelper.obvezni1();
            return View(m);
        }

        [HttpPost]
        public ActionResult PrviPredmetnik(PrviPredmetnikModel model)
        {
            var vl = db.vpis.Find(model.vlid);
            if (vl == null) return HttpNotFound();

            //preveri ce student ze obstaja
            if (vl.student != null) {
                TempData["id"] = model.vlid;
                return RedirectToAction("VpisniListSuccess");
            }

            //kreiraj studenta
            student s = new student();
            s.ime = vl.student.ime;
            s.priimek = vl.student.priimek;
            s.datumRojstva = vl.student.datumRojstva;//.ToShortDateString();
            s.naslov = vl.student.naslov;
            s.spol = Sifranti.SPOL.SingleOrDefault(item => item.id == vl.student.spol).id;
            s.userId = studis.Models.UserHelper.FindByName(User.Identity.Name).id;

            var predmeti = PredmetHelper.obvezni1();

            //shrani predmetnik
            foreach (var p in predmeti)
            {
                studentinpredmet sip = new studentinpredmet();
                sip.predmetId = p.id;
                sip.studentId = s.vpisnaStevilka;
                sip.vpisId = vl.id;
                s.studentinpredmets.Add(sip);
            }

            //fk v vl
            vl.student = s;

            db.students.Add(s);
            db.SaveChanges();

            TempData["id"] = model.vlid;
            return RedirectToAction("VpisniListSuccess");
        }

        public ActionResult DrugiPredmetnik(int id)
        {
            //var vl = db.vpis.Find(id);
            //if (vl == null) return HttpNotFound();

            //obvezni plus 1 strokovno izbirni plus 1 prosto izbirni
            ViewBag.obvezniPredmeti = PredmetHelper.obvezni2();
            ViewBag.strokovnoPredmeti = PredmetHelper.strokovnoizbirni2();
            ViewBag.prostoPredmeti = PredmetHelper.prostoizbirni2();

            int sumObv = 0;
            foreach (var pr in PredmetHelper.obvezni2())
                sumObv += pr.kreditne;

            ViewBag.sumObv = sumObv;
            ViewBag.sumIzb = 60 - sumObv;

            var t = PredmetHelper.obvezni2();
            System.Diagnostics.Debug.WriteLine(t.First().ime);
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