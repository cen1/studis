﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace studis.Controllers
{
    [Authorize(Roles = "Student, Referent")]
    public class IzpitniRokPrijavaController : Controller
    {
        public studisEntities db = new studisEntities();
        // GET: IzpitniRokPrijava
        public ActionResult Index()
        {
            return View();
        }


        // GET: IzpitniRokPrijava/Prijavi
       public ActionResult Prijavi()//SPREJMI VPISNO ind id, za boljso izbiro studenta
        {
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izberi" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            if (User.IsInRole("Student"))
            {
                ViewBag.Izvajanja = GetIzvajanaForStudent();
            }
            else
            {
                ViewBag.Izvajanja = new SelectList(ltemp, "Value", "Text");
            }
            List<student> temp = db.students.OrderBy(a => a.priimek).ToList();

            List<SelectListItem> studenti = new List<SelectListItem>();
            foreach (student i in temp)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.vpisnaStevilka.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString() + " - " + i.ime + " " + i.priimek;
                studenti.Add(p);
            }
            ViewBag.Studenti = new SelectList(studenti, "Value", "Text");

            return View();
        }

        // POST: IzpitniRokPrijava/Prijavi
        [HttpPost]
        public ActionResult Prijavi(PrijavaNaIzpitModel model)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // GET: IzpitniRokPrijava/Delete/5
        public ActionResult Odjavi()
        {
            return View();
        }

        // POST: IzpitniRokPrijava/Delete/5
        [HttpPost]
        public ActionResult Odjavi(PrijavaNaIzpitModel model)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        [Authorize(Roles = "Student")]
        private List<SelectListItem> GetIzvajanaForStudent()
        {
            var student = UserHelper.GetStudentByUserName(User.Identity.Name);
            var izvajanja = student.vpis.LastOrDefault().izvajanjes.ToList();
            return IzvajanaToSeznam(izvajanja);
        }

        private List<SelectListItem> IzvajanaToSeznam(List<izvajanje> izvajanja)
        {
            var seznamIzvajanja = new List<SelectListItem>();
            int c = 0;
            foreach (izvajanje i in izvajanja)
            {
                c++;
                string profesorji = "";
                var profesor1 = i.profesor;
                profesorji += profesor1.ime + " " + profesor1.priimek;
                if (i.profesor1 != null)
                {
                    var profesor2 = i.profesor1;
                    profesorji += ", " + profesor2.ime + " " + profesor2.priimek;
                }
                if (i.profesor2 != null)
                {
                    var profesor3 = i.profesor2;
                    profesorji += ", " + profesor3.ime + " " + profesor3.priimek;
                }

                profesorji = profesorji + " (" + i.sifrant_studijskoleto.naziv + ")";
                seznamIzvajanja.Add(new SelectListItem() { Value = i.id.ToString(), Text = profesorji });
            }
            if (c < 1)
            {
                seznamIzvajanja.Add(new SelectListItem() { Value = "", Text = "Ni predemtov." });
            }
            else
            {
                seznamIzvajanja.Insert(0, new SelectListItem() { Value = "", Text = "Izberi" });
            }
            return seznamIzvajanja;
        }

        public string GetIzvajanjaForStudent(int id)
        {
            student student;
            if (User.IsInRole("Student"))
            {
                student = UserHelper.GetStudentByUserName(User.Identity.Name);
            }
            else
            {
                student = UserHelper.GetStudentByVpisna(id);
            }
            var izvajanja = student.vpis.LastOrDefault().izvajanjes.ToList();
            
            return new JavaScriptSerializer().Serialize(IzvajanaToSeznam(izvajanja));
        }

        public string GetIzpitniRoksForIzvajanja(int id)
        {

            Debug.WriteLine("ID " + id);
            int iid = Convert.ToInt32(id);
            Debug.WriteLine("ID " + iid);
            var izvajanje = db.izvajanjes.SingleOrDefault(i => i.id == iid);//db.predmets.SingleOrDefault(p => p.id == iid);
            var izpitniRoki = izvajanje.izpitniroks;//pPredmet.izpitniroks.ToList(); //Exception 
            var seznamIzpitniRoki = new List<SelectListItem>();
            int c = 0;
            foreach (izpitnirok i in izpitniRoki)
            {
                c++;
                string prostor = "";
                if (i.sifrant_prostor != null)
                {
                    prostor = i.sifrant_prostor.naziv;
                }
                string ura = "";
                if (i.ura != null)
                {
                    ura = UserHelper.TimeToString((DateTime)i.ura);
                }
                seznamIzpitniRoki.Add(new SelectListItem() { Value = i.id.ToString(), Text = UserHelper.DateToString(i.datum) + " " + ura + " " + prostor });
            }
            if (c < 1)
            {
                seznamIzpitniRoki.Add(new SelectListItem() { Value = "", Text = "Ta predmet nima razpisanih rokov." });
            }
            else
            {
                seznamIzpitniRoki.Insert(0, new SelectListItem() { Value = "", Text = "Izberi" });
            }
            return new JavaScriptSerializer().Serialize(seznamIzpitniRoki);
        }

        //PREVERJANJA
        /*
        -preveri cas min 23 ur pred dnem izpita
        -max 3 polaganja letos
        -max 6 polaganj vse skupaj (ponavljanje resetira)
        -preveri ce prijva pri tem predmetu(izvajanju) ze obstaja
        -preveri prijavo na rok na katerga je ze prijavljen
        -preveri ce je izpit ze opravljen
        -preveri da je od prijave minilo x dni
        -preveri ce za prejsnjo prijavo ze obstaja ocena
        -preveri ce mora student placati izpit (4+ redni, 1+ izredni)
        */

        public JsonResult preveri(int vpisna, int izpitniRok)
        {
            return null;
        }
    }
}
