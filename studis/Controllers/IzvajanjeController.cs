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
            try
            {
                izvajanje izvajanje = new izvajanje();
                izvajanje.predmet = db.predmets.SingleOrDefault(p => p.id == model.predmet);
                izvajanje.profesor = db.profesors.SingleOrDefault(p => p.id == model.profesor1);
                if (model.profesor2 != 0 && model.profesor2 != null)
                    izvajanje.profesor1 = db.profesors.SingleOrDefault(p => p.id == model.profesor2);
                if (model.profesor3 != 0 && model.profesor3 != null)
                    izvajanje.profesor2 = db.profesors.SingleOrDefault(p => p.id == model.profesor3);
                Debug.WriteLine("Dodano izvajanje");
                db.SaveChanges();
            }
            catch
            {
                return View("Error");
            }
            return View("Dodaj");
        }
    }
}