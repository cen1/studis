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
            izpitniRok.datum = model.datum;
            izpitniRok.predmet = db.predmets.SingleOrDefault(v => v.id == model.predmet);
            izpitniRok.profesors.Add(db.profesors.SingleOrDefault(p => p.id == model.profesor));
            try
            {
                // TODO: Add insert logic here
                db.IzpitniRoks.Add(izpitniRok);

                return RedirectToAction("");
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
            izpitniRok.datum = model.datum;
            izpitniRok.predmet = db.predmets.SingleOrDefault(v => v.id == model.predmet);
            izpitniRok.profesors.Add(db.profesors.SingleOrDefault(p => p.id == model.profesor));
            try
            {
                // TODO: Add update logic here
                //db.IzpitniRoks.//SingleOrDefault(r => r.ID == model.id).;
                return RedirectToAction("Index");
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

        public string GetProfesorsForPredmet(string id)
        {
            
            int iid = Convert.ToInt32(id);
            var profesors = db.predmets.SingleOrDefault(v => v.id == iid).profesors.ToList();
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

        public string GetIzpitniRoksForPredmet(string idPredmet)
        {
            int iid = Convert.ToInt32(idPredmet);
            //var izpitniRoki = db.predmets.SingleOrDefault(v => v.id == iid).izpitniRoks.ToList();

            return "";
        }

        public string GetDatumForIzpitniRok(string idIzpitniRok)
        {
            return "";
        }

        public JsonResult PreveriDatum(DateTime datum)
        {
            Debug.WriteLine("Datum: " + datum);
            var result = Validate.veljavenDatum(datum);
            if (datum < DateTime.Today)
            {
                result = false;
            }
            return Json(result);
        }
    }
}
