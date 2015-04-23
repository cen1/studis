using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;


namespace studis.Controllers
{
    [Authorize(Roles = "Referent")]
    public class ZetonController : Controller
    {
        public studisEntities db = new studisEntities();

        // GET: Zeton
        [Authorize(Roles = "Referent")]
        public ActionResult Dodaj(int id)
        {
            //id je vpisna
            student s = db.students.Find(id);
            if (s == null) return HttpNotFound();

            ZetonModel model = new ZetonModel();
            ViewBag.letniki = model.pridobiLetnike(id);
            model.vpisna = id;
            var vpisi = db.vpis.Where(v => v.vpisnaStevilka == id);
            ViewBag.vpisi = vpisi;
            ViewBag.sovpisi = false;

            if (vpisi.Count() == 0) ViewBag.sovpisi = false;

            ViewBag.ime = s.ime;
            ViewBag.priimek = s.priimek;
            ViewBag.vpisna = id;

            return View(model);
        }

        /*[HttpPost]
        public ActionResult Dodaj()
        {
            
        }*/
    }
}