using studis.Models;
using System;
using System.Collections.Generic;
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


        //GET: IzpitniRok/New
        public ActionResult Dodaj()
        {
            ViewBag.Predmets = db.predmets;

            
            return View();
        }

        // POST: IzpitniRok/Create
        [HttpPost]
        public ActionResult Create(IzpitniRok izpitniRok)
        {
            try
            {
                // TODO: Add insert logic here
                db.IzpitniRoks.Add(izpitniRok);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: IzpitniRok/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IzpitniRok/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

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
            foreach (profesor p in profesors)
            {
                seznamProfesorjev.Add(new SelectListItem() {Text = (p.ime + " " + p.priimek), Value = p.id.ToString() });
            }

            return new JavaScriptSerializer().Serialize(seznamProfesorjev);
        }
    }
}
