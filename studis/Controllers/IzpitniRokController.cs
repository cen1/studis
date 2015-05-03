using studis.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace studis.Controllers
{
    [Authorize(Roles = "Profesor, Referent")]
    public class IzpitniRokController : Controller
    {
        public studisEntities db = new studisEntities();


        //GET: IzpitniRok/Dodaj
        public ActionResult Dodaj()
        {
            SelectList temp = new SelectList(db.predmets.OrderBy(a => a.ime), "id", "ime");
            
            List<SelectListItem> predmeti = new List<SelectListItem>();
            foreach(SelectListItem i in temp) {
                //Debug.WriteLine(i.Value + " " + i.Text);
                //i.Text = i.Value  + i.Text;
                SelectListItem p = new SelectListItem();
                p.Value = i.Value;
                p.Text = Convert.ToInt32(i.Value).ToString("000") + " - " + i.Text;
                predmeti.Add(p);
            }
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value="", Text="Izbira profesorja" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");
            //new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            //IzpitniRokModel model= new IzpitniRokModel();
            return View();
        }

        // POST: IzpitniRok/Dodaj
        [HttpPost]
        public ActionResult Dodaj(IzpitniRokModel model)
        {
            IzpitniRok izpitniRok = new IzpitniRok();
            izpitniRok.datum = UserHelper.StringToDate(model.datum);
            izpitniRok.predmet = db.predmets.SingleOrDefault(v => v.id == model.predmet);
            izpitniRok.profesor = db.profesors.SingleOrDefault(p => p.id == model.profesor);
            try
            {
                // TODO: Add insert logic here
                db.IzpitniRoks.Add(izpitniRok);

                return RedirectToAction("UspesnoDodan");
            }
            catch
            {
                return View();
            }
        }

        // GET: IzpitniRok/Edit
        public ActionResult Edit()
        {
            SelectList temp = new SelectList(db.predmets.OrderBy(a => a.ime), "id", "ime");

            List<SelectListItem> predmeti = new List<SelectListItem>();
            foreach (SelectListItem i in temp)
            {
                //Debug.WriteLine(i.Value + " " + i.Text);
                //i.Text = i.Value  + i.Text;
                SelectListItem p = new SelectListItem();
                p.Value = i.Value;
                p.Text = Convert.ToInt32(i.Value).ToString("000") + " - " + i.Text;
                predmeti.Add(p);
            }
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izbira" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");
            return View();
        }

        // POST: IzpitniRok/Edit
        [HttpPost]
        public ActionResult Edit(IzpitniRokModel model)
        {
            IzpitniRok izpitniRok = new IzpitniRok();
            izpitniRok.datum = UserHelper.StringToDate(model.datum);
            izpitniRok.predmet = db.predmets.SingleOrDefault(v => v.id == model.predmet);
            izpitniRok.profesor = db.profesors.SingleOrDefault(p => p.id == model.profesor);

            if (model.id == 0) //NI IZBRAN NOBEN ROK, USTVARI NOVEGA -> POGOJ ID "0" NE OBSTAJA
            {

            }
            else { //ROK JE IZBRAN

            }
            
            try
            {
                // TODO: Add update logic here
                //db.IzpitniRoks.Update
                return RedirectToAction("UspesnoSpremenjen");
            }
            catch
            {
                return View();
            }
        }

        // GET: IzpitniRok/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IzpitniRok/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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

        public string GetProfesorsForPredmet(string predmet)
        {
            
            int iid = Convert.ToInt32(predmet);
            List<profesor> profesors;
            try
            {
                profesors = db.predmets.SingleOrDefault(v => v.id == iid).profesors.ToList();
            } catch {
                profesors = new List<profesor>();
            }
            var seznamProfesorjev = new List<SelectListItem>();
            int c = 0;
            foreach (profesor p in profesors)
            {
                c++;
                seznamProfesorjev.Add(new SelectListItem() { Value = p.id.ToString(), Text = (p.ime + " " + p.priimek) });
            }
            if (c < 1)
            {
                seznamProfesorjev.Add(new SelectListItem() { Value = "0", Text = "Ta predmet nima nosilca." });
            }
            /*
            var seznam = new List<SelectListItem>();
            Random rand= new Random();
            for(int i = 0; i< 5; i++) {
                seznam.Add(new SelectListItem() { Value = i.ToString(), Text = rand.Next(i, 100).ToString() });
            }
            */
            return new JavaScriptSerializer().Serialize(seznamProfesorjev);
        }

        public string GetIzpitniRoksForPredmet(string predmet)
        {
            /*
             * TO DO TO DO TO DO TO DO TO DO TO DO
             */ 
            int iid = Convert.ToInt32(predmet);
            var izpitniRoki= new List<IzpitniRok>();// = db.predmets.SingleOrDefault(v => v.id == iid).izpitniRoks.ToList();
            var seznamIzpitniRoki = new List<SelectListItem>();
            int c = 0;
            foreach (IzpitniRok i in izpitniRoki)
            {
                c++;
                seznamIzpitniRoki.Add(new SelectListItem() { Value = i.ID.ToString(), Text = (UserHelper.DateToString(i.datum) + " - " + i.profesor.ime + " " + i.profesor.priimek) });
            }
            if (c < 1)
            {
                seznamIzpitniRoki.Add(new SelectListItem() { Value = "0", Text = "Ta predmet nima razpisanih rokov." });
            }
            return new JavaScriptSerializer().Serialize(seznamIzpitniRoki);
        }

        public string GetDatumForIzpitniRok(string id)
        {
            int iid = Convert.ToInt32(id);
            var datum = db.IzpitniRoks.SingleOrDefault(r => r.ID == iid).datum;
            return UserHelper.DateToString(datum);
        }

        public JsonResult PreveriDatum(string datum)
        {
            Debug.WriteLine("datum: " + datum);
            DateTime d = UserHelper.StringToDate(datum);
            Debug.WriteLine("Datum: " + d);
            var result = Validate.veljavenDatum(d);
            if (d < DateTime.Today)
            {
                result = false;
            }
            return Json(result);
        }
    }
}
