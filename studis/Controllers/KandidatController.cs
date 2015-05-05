using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;

namespace studis.Controllers
{
    [Authorize(Roles = "Referent")]
    public class KandidatController : Controller
    {
        public studisEntities db = new studisEntities();

        // GET: Kandidat
        public ActionResult Index()
        {
            ViewBag.kandidati = db.kandidats.Where(k => k.my_aspnet_users.students.Count() == 0).ToList();
            return View();
        }
    }
}