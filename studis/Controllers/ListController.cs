using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using MvcRazorToPdf;

namespace studis.Controllers
{
    public class ListController : Controller
    {
        // GET: Pdf
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Pdf()
        {
            return new PdfActionResult(null);
        }
    }
}