using studis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Controllers
{
    public class VpisniListController : Controller
    {
        // GET: VpisniList
        public ActionResult VpisniList()
        {
            ViewBag.Title = "VpisniList";
            ViewBag.StudijskiProgrami = new SelectList(Sifranti.STUDIJSKIPROGRAM, "id", "naziv");
            ViewBag.Klasius = new SelectList(Sifranti.KLASIUS, "id", "naziv");
            ViewBag.VrstaVpisa = new SelectList(Sifranti.VRSTAVPISA, "id", "naziv");
            ViewBag.NacinStudija = new SelectList(Sifranti.NACINSTUDIJA, "id", "naziv");
            ViewBag.OblikaStudija = new SelectList(Sifranti.OBLIKASTUDIJA, "id", "naziv");

            return View();
        }
        
        [HttpPost]
        public ActionResult Zajemi(VpisniList vpisniList)
        {
            //preveri

            //shrani

            return View(vpisniList);
        }


    }
}