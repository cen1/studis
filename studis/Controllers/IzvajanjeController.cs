using studis.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Controllers
{
    [Authorize(Roles = "Referent")]
    public class IzvajanjeController : Controller
    {
        
        public studisEntities db = new studisEntities();

        // GET: Izvajanje
        public ActionResult Dodaj()
        {
            List<predmet> listPredmetov = db.predmets.OrderBy(a => a.ime).ToList();

            List<SelectListItem> predmeti = new List<SelectListItem>();
            foreach (predmet i in listPredmetov)
            {
                SelectListItem p = new SelectListItem();
                int stIzvajanj = i.izvajanjes.Count;
                p.Value = i.id.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.ime + " (" + i.koda + ") - " + stIzvajanj;
                predmeti.Add(p);
            }
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");

            List<profesor> listProfesorjev = db.profesors.OrderBy(a => a.priimek).ToList();

            List<SelectListItem> profesorji = new List<SelectListItem>();
            foreach (profesor i in listProfesorjev)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.id.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.priimek + " " + i.ime;
                profesorji.Add(p);
            }
            ViewBag.Profesors = new SelectList(profesorji, "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Dodaj(IzvajanjeModel model)
        {
            List<predmet> listPredmetov = db.predmets.OrderBy(a => a.ime).ToList();

            List<SelectListItem> predmeti = new List<SelectListItem>();
            foreach (predmet i in listPredmetov)
            {
                SelectListItem p = new SelectListItem();
                int stIzvajanj = i.izvajanjes.Count;
                p.Value = i.id.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.ime + " (" + i.koda + ") - " + stIzvajanj;
                predmeti.Add(p);
            }
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");

            List<profesor> listProfesorjev = db.profesors.OrderBy(a => a.priimek).ToList();

            List<SelectListItem> profesorji = new List<SelectListItem>();
            foreach (profesor i in listProfesorjev)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.id.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.priimek + " " + i.ime;
                profesorji.Add(p);
            }
            ViewBag.Profesors = new SelectList(profesorji, "Value", "Text");

            izvajanje izvajanje = new izvajanje();
            izvajanje.predmetId = model.predmet;
                
            izvajanje.izvajalec1Id = model.profesor1;
            if (model.profesor2 != 0)
                izvajanje.izvajalec2Id = model.profesor2;
            if (model.profesor3 != 0)
                izvajanje.izvajalec3Id = model.profesor3;

            System.Diagnostics.Debug.WriteLine(model.predmet.ToString() + "-" + model.profesor1.ToString() + "-" + model.profesor2.ToString());
            db.izvajanjes.Add(izvajanje);

            try
            {
                db.SaveChanges();
            }
            catch
            {
                return View("Error");
            }

            TempData["ok"] = "Shranjeno";
            return RedirectToAction("Dodaj");
        }
    }
}