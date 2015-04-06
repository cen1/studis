using studis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Controllers
{
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

                db.vpisnilists.Add(v);
                db.SaveChanges();

            }
            else
            {
                ModelState.AddModelError("", "Prišlo je do napake.");
            }

            return View(model);
            //tule returnaj PDF
        }


    }
}