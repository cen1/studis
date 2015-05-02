﻿using System;
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

        // izpis vpisnega lista
        public ActionResult Potrditev(int id)
        {
            if (id > 60000000)
            {
                try
                {
                    var tmp = db.vpis.SingleOrDefault(v => v.vpisnaStevilka == id && v.potrjen == false);
                    id = tmp.id;
                }
                catch
                {
                    TempData["Napaka"] = "Študent nima na voljo vpisega lista za potrditev";
                    return RedirectToAction("Napaka");
                }
            }

            var list = db.vpis.SingleOrDefault(v => v.id == id && v.potrjen == false);

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

                ViewBag.PostnaStevilka = list.student.sifrant_postnastevilka.id;
                ViewBag.Posta = list.student.sifrant_postnastevilka.naziv;
                ViewBag.Obcina = list.student.sifrant_obcina.naziv;
                ViewBag.Drzava = list.student.sifrant_drzava.naziv;

                if (list.student.sifrant_postnastevilka1 != null)
                {
                    ViewBag.PostnaStevilkaZacasni = list.student.sifrant_postnastevilka1.id;
                    ViewBag.PostaZacasni = list.student.sifrant_postnastevilka1.naziv;
                }

                if (list.student.sifrant_obcina1 != null)
                {
                    ViewBag.ObcinaZacasni = list.student.sifrant_obcina1.naziv;
                }

                if (list.student.sifrant_drzava1 != null)
                {
                    ViewBag.DrzavaZacasni = list.student.sifrant_drzava1.naziv;
                }

                ViewBag.StudijskiProgram = list.sifrant_studijskiprogram.naziv;

                if (list.sifrant_studijskiprogram1 != null)
                {
                    ViewBag.StudijskiProgram2 = list.sifrant_studijskiprogram1.naziv;
                }

                ViewBag.VrstaStudija = list.sifrant_klasius.naziv;
                ViewBag.NacinStudija = list.sifrant_nacinstudija.naziv;
                ViewBag.OblikaStudija = list.sifrant_oblikastudija.naziv;
                ViewBag.KrajIzvajanja = list.sifrant_obcina.naziv;

                if (list.sifrant_obcina1 != null)
                {
                    ViewBag.KrajIzvajanja2 = list.sifrant_obcina1.naziv;
                }

                ViewBag.VrstaVpisa = list.sifrant_vrstavpisa.naziv;
                ViewBag.Spol = list.student.sifrant_spol.naziv;
                ViewBag.ObcinaRojstva = list.student.sifrant_obcina2.naziv;
                ViewBag.DrzavaRojstva = list.student.sifrant_drzava2.naziv;
                ViewBag.Drzavljanstvo = list.student.sifrant_drzava3.naziv;
                ViewBag.IzbirnaSkupina = list.sifrant_izbirnaskupina.naziv;

                if (list.sifrant_izbirnaskupina1 != null)
                {
                    ViewBag.IzbirnaSkupina2 = list.sifrant_izbirnaskupina1.naziv;
                }

                ViewBag.LetnikStudija = list.sifrant_letnik.naziv;
                ViewBag.Vpisna = list.vpisnaStevilka;
                ViewBag.DatumRojstva = list.student.datumRojstva.ToString("dd.MM.yyyy");

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

                var predmeti = list.studentinpredmets.Where(v => v.vpisId == id);

                ViewBag.Predmeti = predmeti;
                ViewBag.id = id;

                return View(model);
            }
            catch (System.NullReferenceException) {
                TempData["Napaka"] = "Vpisni list je že potrjen ali pa ne obstaja!";
                return RedirectToAction("Napaka");
            }
            catch
            {
                TempData["Napaka"] = "Nekateri obvezni podatki niso izpolnjeni!";
                return RedirectToAction("Napaka");
            }
        }

        // tiskanje potrdil in potrditev
        [HttpPost]
        public ActionResult Izpis(int id, string Selektor)
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
                    TempData["Napaka"] = "Študent nima na voljo vpisega lista za potrditev";
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

                UserHelper uh = new UserHelper();

                if (uh.jePredmetnikVzpostavljen(db.vpis.Find(id)))
                {
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
                    ViewBag.Stevilo = Convert.ToInt32(Selektor);
 
                    try
                    {
                        // potrditev !!
                        var store = db.vpis.SingleOrDefault(v => v.id == id);
                        store.potrjen = true;
                        db.SaveChanges();
                    }
                    catch
                    {
                        TempData["Napaka"] = "Potrditev ni uspela. Napaka baze!";
                        return RedirectToAction("Napaka");
                    }
                    

                    return new PdfActionResult(model);
                }
                else
                {
                    TempData["Napaka"] = "Študent nima vzpostavljenega predmetnika!";
                    return RedirectToAction("Napaka");
                }
            }
            catch (System.NullReferenceException)
            {
                TempData["Napaka"] = "Vpisni list je že potrjen ali pa ne obstaja!";
                return RedirectToAction("Napaka");
            }
            catch
            {
                TempData["Napaka"] = "Nekateri obvezni podatki niso izpolnjeni!";
                return RedirectToAction("Napaka");
            }
        }

        public ActionResult Napaka()
        {
            ViewBag.Message = TempData["Napaka"];
            return View();
        }
    }
}