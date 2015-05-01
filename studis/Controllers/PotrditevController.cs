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
    [Authorize(Roles = "Referent")]
    public class PotrditevController : Controller
    {
        public studisEntities db = new studisEntities();

        public ActionResult Potriditev()
        {
            return View();
        }

        public ActionResult Izpis(int id)
        {
            if (id > 60000000)
            {
                try
                {
                    var tmp = db.vpis.Single(v => v.vpisnaStevilka == id);
                    id = tmp.id;
                }
                catch
                {
                    return RedirectToAction("Napaka");
                }

            }
            var list = db.vpis.SingleOrDefault(v => v.id == id);
            
            // preveri če vpisni list obstaja
            try
            {
                var model = new studis.Models.VpisniListModel
                {
                    studijskoLeto = StudijskoLeto.toString(list.studijskoLeto),
                    ime = list.student.ime,
                    priimek = list.student.priimek,
                    datumRojstva = list.student.datumRojstva,
                    krajRojstva = list.student.krajRojstva,
                    obcinaRojstva = list.student.obcinaRojstva,
                    drzavaRojstva = list.student.drzavaRojstva,
                    drzavljanstvo = list.student.drzavljanstvo,
                    spol = list.student.spol,
                    emso = list.student.emso,
                    davcnaStevilka = list.student.davcnaStevilka ?? default(int),
                    email = list.student.email,
                    prenosniTelefon = list.student.prenosniTelefon,
                    naslov = list.student.naslov,
                    vrocanje = Convert.ToBoolean(list.student.vrocanje),
                    postnaStevilka = list.student.postnaStevilka,
                    obcina = list.student.obcina,
                    drzava = list.student.drzava,
                    naslovZacasni = list.student.naslovZacasni,
                    vrocanjeZacasni = Convert.ToBoolean(list.student.vrocanjeZacasni),
                    postnaStevilkaZacasni = list.student.postnaStevilkaZacasni,
                    obcinaZacasni = list.student.obcinaZacasni,
                    drzavaZacasni = list.student.drzavaZacasni,
                    studijskiProgram = list.studijskiProgram,
                    smer = list.smer ?? default(int),
                    krajIzvajanja = list.krajIzvajanja,
                    izbirnaSkupina = list.izbirnaSkupina,
                    studijskiProgram2 = list.studijskiProgram2,
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

                if (Convert.ToBoolean(list.student.vrocanje))
                {
                    ViewBag.Vrocanje = "DA";
                }
                else
                {
                    ViewBag.Vrocanje = "NE";
                }

                if (Convert.ToBoolean(list.student.vrocanjeZacasni))
                {
                    ViewBag.VrocanjeZacasni = "DA";
                }
                else
                {
                    ViewBag.VrocanjeZacasni = "NE";
                }

                ViewBag.LetnikStudija = list.sifrant_letnik.naziv;
                ViewBag.Vpisna = list.vpisnaStevilka;
                ViewBag.DatumRojstva = list.student.datumRojstva.ToString("dd.MM.yyyy");
                ViewBag.StudijskiProgram = list.sifrant_studijskiprogram.naziv;
                ViewBag.NacinStudija = list.sifrant_nacinstudija.naziv;
                ViewBag.Spol = list.student.sifrant_spol.naziv;
                ViewBag.Stevilo = 6;

                return new PdfActionResult(model);

            }
            catch
            {
                return RedirectToAction("Napaka");
            }
        }
    }
}