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
            return View();
        }
        
        [HttpPost]
        public ActionResult VpisniList(studis.Models.VpisniListModel model)
        {
            model.studijskoLeto = "2014/2015";
            if (ModelState.IsValid)
            {
                    vpisnilist v = new vpisnilist();
                    v.datumRojstva = model.datumRojstva;
                    v.davcnaStevilka = model.davcnaStevilka;
                    v.drzava = model.davcnaStevilka;
                    v.drzavaRojstva = model.drzavaRojstva;
                    v.drzavaZacasni = model.drzavaZacasni;
                    v.drzavljanstvo = model.drzavljanstvo;
                    v.email = model.email;
                    v.emso = model.emso;
                    v.ime = model.ime;
                    v.izbirnaSkupina = model.izbirnaSkupina;
                    v.izbirnaSkupina2 = model.izbirnaSkupina2;
                    v.krajIzvajanja = model.krajIzvajanja;
                    v.krajIzvajanja2 = model.krajIzvajanja2;
                    v.krajRojstva = model.krajRojstva;
                    v.letnikStudija = model.letnikStudija;
                    v.nacinStudija = model.nacinStudija;
                    v.naslov = model.naslov;
                    v.naslovZacasni = model.naslovZacasni;
                    v.obcina = model.obcina;
                    v.obcinaRojstva = model.obcinaRojstva;
                    v.obcinaZacasni = model.obcinaZacasni;
                    v.oblikaStudija = model.oblikaStudija;
                    v.postnaStevilka = model.postnaStevilka;
                    v.postnaStevilkaZacasni = model.postnaStevilkaZacasni;
                    v.prenosniTelefon = model.prenosniTelefon;
                    v.priimek = model.priimek;
                    v.smer = 0;
                    v.smer2 = 0;
                    v.soglasje1 = model.soglasje1;
                    v.soglasje2 = model.soglasje2;
                    v.spol = model.spol;
                    v.studijskiProgram = model.studijskiProgram;
                    v.studijskiProgram2 = model.studijskiProgram2;
                    v.studijskoLeto = model.studijskoLeto;
                    v.studijskoLetoPrvegaVpisa = model.studijskoLetoPrvegaVpisa;
                    v.vrocanje = model.vrocanje;
                    v.vrstaStudija = model.vrstaStudija;
                    v.vrstaVpisa = model.vrstaVpisa;

                    if (model.vrocanje) v.vrocanjeZacasni = false;
                    else v.vrocanjeZacasni = model.vrocanjeZacasni;
                    if (!model.vrocanje && !model.vrocanjeZacasni)
                    {
                        ModelState.AddModelError("", "Izberite naslov za vročanje.");
                        return View(model);
                    }

                    v.vpisnaStevilka = null;
                    v.potrjen = false;
                    v.student = null;
                    v.studijskoLeto = "2014/2015";

                    db.vpisnilists.Add(v);
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
            return View();
        }

        public ActionResult VpisniListPredmetnik(int id)
        {
            var vl = db.vpisnilists.Find(id);

            System.Diagnostics.Debug.WriteLine(id);
            System.Diagnostics.Debug.WriteLine(vl.id);
            System.Diagnostics.Debug.WriteLine(vl.letnikStudija);

            if (vl.letnikStudija==1)
                return RedirectToAction("PrviPredmetnik", "VpisniList", new { id = vl.id });
            else if (vl.letnikStudija==2)
                return RedirectToAction("DrugiPredmetnik", "VpisniList", new { id = vl.id });
            else if (vl.letnikStudija==3)
                return RedirectToAction("TretjiPredmetnik", "VpisniList", new { id = vl.id });
            else
                return RedirectToAction("NeznanPredmetnik", "VpisniList");

            //ViewBag.strokovnoizbirni = db.predmets.Where(l => l.letnik == id).Where(m => m.strokovnoizbirni == 1);
            //ViewBag.prostoizbirni = db.predmets.Where(l => l.letnik == id).Where(m => m.obvezen == 1);
            //
        }

        public ActionResult PrviPredmetnik(int id)
        {
            var vl = db.vpisnilists.Find(id);
            PrviPredmetnikModel m = new PrviPredmetnikModel();
            m.vlid = id;
            m.predmeti = db.predmets.Where(l => l.letnik == vl.letnikStudija).Where(n => n.obvezen == true);
            return View(m);
        }

        [HttpPost]
        public ActionResult PrviPredmetnik(studis.Models.PrviPredmetnikModel model)
        {
            var vl = db.vpisnilists.Find(model.vlid);
            
            //kreiraj studenta
            student s = new student();
            s.ime = vl.ime;
            s.priimek = vl.priimek;
            s.datum_rojstva = vl.datumRojstva.ToShortDateString();
            s.naslov = vl.naslov;
            s.spol = Sifranti.SPOL.SingleOrDefault(item => item.id == vl.spol).naziv;
            s.userId = studis.Models.User.FindByName(db, User.Identity.Name).id;

            var predmeti = db.predmets.Where(l => l.letnik == vl.letnikStudija).Where(n => n.obvezen == true);

            //shrani predmetnik
            foreach (var p in predmeti)
            {
                s.predmets.Add(p);
            }

            db.students.Add(s);
            db.SaveChanges();

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

    }
}