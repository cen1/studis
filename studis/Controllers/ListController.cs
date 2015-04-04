using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace studis.Controllers
{
    public class ListController : Controller
    {
        // GET: Pdf
        public ActionResult List()
        {
            return View();
        }
    }
}