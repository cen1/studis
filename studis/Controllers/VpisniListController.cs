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
            ViewBag.StudijskiProgrami = new SelectList(Sifranti.STUDIJSKIPROGRAM);
            ViewBag.Klasius = Sifranti.KLASIUS;
            ViewBag.VrstaVpisa = Sifranti.VRSTAVPISA;
            ViewBag.NacinStudija = Sifranti.NACINSTUDIJA;
            ViewBag.OblikaStudija = Sifranti.OBLIKASTUDIJA;

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