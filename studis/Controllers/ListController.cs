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

        public ActionResult Pdf(int id)
        {
            var list = db.vpisnilists.SingleOrDefault(v => v.id == id);
            // preveri če vpisni list obstaja
            try {
                var model = new studis.Models.VpisniListModel
                {
                    studijskoLeto = list.studijskoLeto,
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
                ViewBag.PostnaStevilka = Sifranti.POSTNESTEVILKE.SingleOrDefault(item => item.id == list.postnaStevilka);
                ViewBag.Obcina = Sifranti.OBCINE.SingleOrDefault(item => item.id == list.obcina);
                ViewBag.Drzava = Sifranti.DRZAVE.SingleOrDefault(item => item.id == list.drzava);
                ViewBag.PostnaStevilkaZacasni = Sifranti.POSTNESTEVILKE.SingleOrDefault(item => item.id == Convert.ToInt32(list.postnaStevilkaZacasni));
                ViewBag.ObcinaZacasni = Sifranti.OBCINE.SingleOrDefault(item => item.id == Convert.ToInt32(list.obcinaZacasni));
                ViewBag.DrzavaZacasni = Sifranti.DRZAVE.SingleOrDefault(item => item.id == Convert.ToInt32(list.drzavaZacasni));
                ViewBag.StudijskiProgram = Sifranti.STUDIJSKIPROGRAM.SingleOrDefault(item => item.id == list.studijskiProgram);
                ViewBag.VrstaStudija = Sifranti.KLASIUS.SingleOrDefault(item => item.id == list.vrstaVpisa);
                ViewBag.NacinStudija = Sifranti.NACINSTUDIJA.SingleOrDefault(item => item.id == list.nacinStudija);
                ViewBag.OblikaStudija = Sifranti.OBLIKASTUDIJA.SingleOrDefault(item => item.id == list.oblikaStudija);
                ViewBag.KrajIzvajanja = Sifranti.OBCINE.SingleOrDefault(item => item.id == list.krajIzvajanja);
                ViewBag.VrstaVpisa = Sifranti.VRSTAVPISA.SingleOrDefault(item => item.id == list.vrstaVpisa);
                ViewBag.Spol = Sifranti.SPOL.SingleOrDefault(item => item.id == list.spol);
                ViewBag.ObcinaRojstva = Sifranti.OBCINE.SingleOrDefault(item => item.id == list.obcinaRojstva);
                ViewBag.DrzavaRojstva = Sifranti.DRZAVE.SingleOrDefault(item => item.id == list.drzavaRojstva);
                ViewBag.Drzavljanstvo = Sifranti.DRZAVE.SingleOrDefault(item => item.id == list.drzavljanstvo);

                ViewBag.vpisna = list.vpisnaStevilka;
            
                return new PdfActionResult(model);
            }
            catch
            {
                return RedirectToAction("ListEmpty");
            }
        }

        [Authorize(Roles = "Referent")]
        public ActionResult PdfSkrbnik()
        {
            return View();
        }

        public ActionResult ListEmpty()
        {
            return View();
        }
    }
}