using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using MvcRazorToPdf;
using studis.Models;
using System.Web.Security;

namespace studis.Controllers
{
    public class ListController : Controller
    {
        public studisEntities db = new studisEntities();

        //[Authorize(Roles = "Študent")]
        //[Authorize(Roles = "Referent")]
        //[Authorize(Roles = "Skrbnik")]
        public ActionResult Pdf(int id)
        {
            var list = db.vpisnilists.SingleOrDefault(v => v.id == id);

            // preveri če vpisni list obstaja
            if (list.vrstaVpisa != null) {
                var model = new studis.Models.VpisniListModel
                {
                    studijskoLeto = list.studijskoLeto,
                    vpisnaStevilka = list.vpisnaStevilka,
                    ime = list.ime,
                    priimek = list.priimek,
                    datumRojstva = list.datumRojstva,
                    krajRojstva = list.krajRojstva,
                    obcinaRojstva = list.obcinaRojstva,
                    drzavaRojstva = list.drzavaRojstva,
                    drzavljanstvo = list.drzavljanstvo,
                    spol = list.spol,
                    emso = list.emso,
                    davcnaStevilka = list.davcnaStevilka,
                    email = list.email,
                    prenosniTelefon = list.prenosniTelefon,
                    naslov = list.naslov,
                    vrocanje = Convert.ToBoolean(list.vrocanje),
                    postnaStevilka = list.postnaStevilka,
                    obcina = list.obcina,
                    drzava = list.drzava,
                    naslovZacasni = list.naslovZacasni,
                    vrocanjeZacasni = Convert.ToBoolean(list.vrocanjeZacasni),
                    postnaStevilkaZacasni = list.postnaStevilkaZacasni,
                    obcinaZacasni = list.obcinaZacasni,
                    drzavaZacasni = list.drzavaZacasni,
                    studijskiProgram = list.studijskiProgram,
                    smer = list.smer,
                    krajIzvajanja = list.krajIzvajanja,
                    izbirnaSkupina = list.izbirnaSkupina,
                    smer2 = list.smer2,
                    krajIzvajanja2 = list.krajIzvajanja2,
                    izbirnaSkupina2 = list.izbirnaSkupina2,
                    vrstaStudija = list.vrstaStudija,
                    vrstaVpisa = list.vrstaVpisa,
                    letnikStudija = list.letnikStudija,
                    nacinStudija = list.nacinStudija,
                    oblikaStudija = list.oblikaStudija,
                    studijskoLetoPrvegaVpisa = list.studijskoLetoPrvegaVpisa,
                    soglasje1 = Convert.ToBoolean(list.soglasje1),
                    soglasje2 = Convert.ToBoolean(list.soglasje2)
                };

                // refactoring needed
                ViewBag.PostnaStevilka = Sifranti.POSTNESTEVILKE[list.postnaStevilka];
                ViewBag.Obcina = Sifranti.OBCINE[list.obcina];
                ViewBag.Drzava = Sifranti.DRZAVE[list.drzava];
                ViewBag.PostnaStevilkaZacasni = Sifranti.POSTNESTEVILKE[Convert.ToInt32(list.postnaStevilkaZacasni)];
                ViewBag.ObcinaZacasni = Sifranti.OBCINE[Convert.ToInt32(list.obcinaZacasni)];
                ViewBag.DrzavaZacasni = Sifranti.DRZAVE[Convert.ToInt32(list.drzavaZacasni)];
                ViewBag.StudijskiProgram = Sifranti.STUDIJSKIPROGRAM[list.studijskiProgram];
                ViewBag.VrstaStudija = Sifranti.KLASIUS[list.vrstaVpisa];
                ViewBag.NacinStudija = Sifranti.NACINSTUDIJA[list.nacinStudija];
                ViewBag.OblikaStudija = Sifranti.OBLIKASTUDIJA[list.oblikaStudija];
                ViewBag.KrajIzvajanja = Sifranti.OBCINE[list.krajIzvajanja];
                ViewBag.VrstaVpisa = Sifranti.VRSTAVPISA[list.vrstaVpisa];
                ViewBag.Spol = Sifranti.SPOL[list.spol];
                ViewBag.ObcinaRojstva = Sifranti.OBCINE[list.obcinaRojstva];
                ViewBag.DrzavaRojstva = Sifranti.DRZAVE[list.drzavaRojstva];
                ViewBag.Drzavljanstvo = Sifranti.DRZAVE[list.drzavljanstvo];

            
                return View(model);
            }
            else
            {
                return RedirectToAction("ListEmpty");
            }
        }

        //[Authorize(Roles = "Referent")]
        //[Authorize(Roles = "Skrbnik")]
        public ActionResult PdfSkrbnik()
        {
            return View();
        }

        //[Authorize(Roles = "Študent")]
        //[Authorize(Roles = "Referent")]
        //[Authorize(Roles = "Skrbnik")]
        public ActionResult ListEmpty()
        {
            return View();
        }
    }
}