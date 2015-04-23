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
            var list = db.vpis.SingleOrDefault(v => v.id == id);
            // preveri če vpisni list obstaja
            try {
                var model = new studis.Models.VpisniListModel
                {
                    studijskoLeto = StudijskoLeto.toString(list.studijskoLeto),//studijskoLeto,
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

                ViewBag.PostnaStevilka = Sifranti.POSTNESTEVILKE.SingleOrDefault(item => item.id == list.student.postnaStevilka);
                ViewBag.Obcina = Sifranti.OBCINE.SingleOrDefault(item => item.id == list.student.obcina);
                ViewBag.Drzava = Sifranti.DRZAVE.SingleOrDefault(item => item.id == list.student.drzava);
                ViewBag.PostnaStevilkaZacasni = Sifranti.POSTNESTEVILKE.SingleOrDefault(item => item.id == Convert.ToInt32(list.student.postnaStevilkaZacasni));
                ViewBag.ObcinaZacasni = Sifranti.OBCINE.SingleOrDefault(item => item.id == Convert.ToInt32(list.student.obcinaZacasni));
                ViewBag.DrzavaZacasni = Sifranti.DRZAVE.SingleOrDefault(item => item.id == Convert.ToInt32(list.student.drzavaZacasni));
                ViewBag.StudijskiProgram = Sifranti.STUDIJSKIPROGRAM.SingleOrDefault(item => item.id == list.studijskiProgram);
                ViewBag.StudijskiProgram2 = Sifranti.STUDIJSKIPROGRAM.SingleOrDefault(item => item.id == list.studijskiProgram2);
                ViewBag.VrstaStudija = Sifranti.KLASIUS.SingleOrDefault(item => item.id == list.vrstaStudija);
                ViewBag.NacinStudija = Sifranti.NACINSTUDIJA.SingleOrDefault(item => item.id == list.nacinStudija);
                ViewBag.OblikaStudija = Sifranti.OBLIKASTUDIJA.SingleOrDefault(item => item.id == list.oblikaStudija);
                ViewBag.KrajIzvajanja = Sifranti.OBCINE.SingleOrDefault(item => item.id == list.krajIzvajanja);
                ViewBag.KrajIzvajanja2 = Sifranti.OBCINE.SingleOrDefault(item => item.id == list.krajIzvajanja2);
                ViewBag.VrstaVpisa = Sifranti.VRSTAVPISA.SingleOrDefault(item => item.id == list.vrstaVpisa);
                ViewBag.Spol = Sifranti.SPOL.SingleOrDefault(item => item.id == list.student.spol);
                ViewBag.ObcinaRojstva = Sifranti.OBCINE.SingleOrDefault(item => item.id == list.student.obcinaRojstva);
                ViewBag.DrzavaRojstva = Sifranti.DRZAVE.SingleOrDefault(item => item.id == list.student.drzavaRojstva);
                ViewBag.Drzavljanstvo = Sifranti.DRZAVE.SingleOrDefault(item => item.id == list.student.drzavljanstvo);
                ViewBag.IzbirnaSkupina = Sifranti.IZBIRNASKUPINA.SingleOrDefault(item => item.id == list.izbirnaSkupina);
                ViewBag.IzbirnaSkupina2 = Sifranti.IZBIRNASKUPINA.SingleOrDefault(item => item.id == list.izbirnaSkupina2);
                ViewBag.LetnikStudija = Sifranti.LETNIK.SingleOrDefault(item => item.id == list.letnikStudija);
                ViewBag.Vpisna = list.vpisnaStevilka;
                ViewBag.datumRojstva = list.student.datumRojstva.ToString("dd.MM.yyyy");

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

                var stud = db.students.Single(s => s.vpisnaStevilka == list.vpisnaStevilka);
                var predmeti = stud.studentinpredmets.Where(a => a.predmetId == list.id);

                ViewBag.Predmeti = predmeti;
            
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