using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using MvcRazorToPdf;
using studis.Models;
using System.Web.Security;

namespace studis.Controllers
{
    public class ListController : Controller
    {
        public studisEntities db = new studisEntities();

        //[Authorize(Roles = "Študent")]
        //[Authorize(Roles = "Referent")]
        //[Authorize(Roles = "Skrbnik")]
        public ActionResult List()
        {
            /*if (Roles.GetRolesForUser().Contains("Skrbnik") || Roles.GetRolesForUser().Contains("Referent"))
            {
                return RedirectToAction("PdfSkrbnik");
            }
            else if (Roles.GetRolesForUser().Contains("Študent"))
            {
                return RedirectToAction("Pdf");
            }

            return RedirectToAction("Index", "Home");*/
            return RedirectToAction("PdfSkrbnik");
        }

        //[Authorize(Roles = "Študent")]
        //[Authorize(Roles = "Referent")]
        //[Authorize(Roles = "Skrbnik")]
        public ActionResult Pdf()
        {
/*
            // preveri če vpisni list obstaja
            if (vpisnilist) {
                // da
                var model = new studis.Models.nekinovga
                {
                    student = db.students.SingleOrDefault(m => m.userId == 10),
                    vpisnilist = db.vpisnilist.SingleOrDefault(m => m.userId == ?)
                };
            
                return View(model);
            }
  
            // ne
            else
            {
                return RedirectToAction("ListEmpty");
            }
*/
            return new PdfActionResult(db.students.SingleOrDefault(m => m.userId == 10));
        }

        //[Authorize(Roles = "Referent")]
        //[Authorize(Roles = "Skrbnik")]
        public ActionResult PdfSkrbnik()
        {
            return View();
        }

        //[Authorize(Roles = "Študent")]
        //[Authorize(Roles = "Referent")]
        //[Authorize(Roles = "Skrbnik")]
        public ActionResult ListEmpty()
        {
            return View();
        }
    }
}