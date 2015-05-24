using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;

namespace studis.Controllers
{
    public class IzpitniRokPrijavaController : Controller
    {
        // GET: IzpitniRokPrijava
        public ActionResult Index()
        {
            return View();
        }


        // GET: IzpitniRokPrijava/Prijavi
       public ActionResult Prijavi()
        {
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
    }
}
