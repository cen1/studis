using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using studis.Models;

namespace studis.Controllers
{
    //[Authorize(Roles = "Študent")]
    public class StudentController : Controller
    {
        private studisEntities db = new studisEntities();

        // GET: /Student/
        public ActionResult Students()
        {
            return View();
        }

        [Authorize(Roles = "Referent, Profesor")]
        public ActionResult StudentSearchPartial(string searchString)
        {
            var students = from s in db.students select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                int vpisna;
                bool isNumerical = int.TryParse(searchString, out vpisna);
                if (isNumerical)
                {
                    students = students.Where(s => s.vpisnaStevilka == vpisna);
                }
                else
                {
                    string[] searchData = searchString.Split(null);

                    if (searchData.Length > 1 && searchData[1] != "")
                    {
                        string firstChar = searchData[0];
                        string secondChar = searchData[1];

                        var tempStudents = students.Where(s => s.ime.StartsWith(firstChar) && s.priimek.StartsWith(secondChar));

                        if (tempStudents.Any())
                            students = tempStudents;
                        else
                            students = students.Where(s => s.ime.StartsWith(secondChar) && s.priimek.StartsWith(firstChar));
                    }
                    else
                    {
                        string firstChar = searchData[0];
                        students = students.Where(s => s.ime.StartsWith(firstChar) || s.priimek.StartsWith(firstChar));
                    }
                }
            }

            List<student> st = null;
            if (students.Any())
                st = students.ToList();
            return PartialView("_StudentSearchPartial",st);
        }

        [Authorize(Roles = "Referent")]
        public ActionResult StudentSearchPDFPartial(string searchString1)
        {
            var students = from s in db.students select s;

            if (!String.IsNullOrEmpty(searchString1))
            {
                int vpisna;
                bool isNumerical = int.TryParse(searchString1, out vpisna);
                if (isNumerical)
                {
                    students = students.Where(s => s.vpisnaStevilka == vpisna);
                }
                else
                {
                    string[] searchData = searchString1.Split(null);

                    if (searchData.Length > 1 && searchData[1] != "")
                    {
                        string firstChar = searchData[0];
                        string secondChar = searchData[1];

                        var tempStudents = students.Where(s => s.ime.StartsWith(firstChar) && s.priimek.StartsWith(secondChar));

                        if (tempStudents.Any())
                            students = tempStudents;
                        else
                            students = students.Where(s => s.ime.StartsWith(secondChar) && s.priimek.StartsWith(firstChar));
                    }
                    else
                    {
                        string firstChar = searchData[0];
                        students = students.Where(s => s.ime.StartsWith(firstChar) || s.priimek.StartsWith(firstChar));
                    }
                }
            }
            if (!students.Any())
                students = null;
            return PartialView("_StudentSearchPDFPartial", students);
        }

        // GET: /Student/Details/63060363
        [Authorize(Roles = "Referent, Profesor")]
        public ActionResult Details(int? vpisnaSt)
        {
            if (vpisnaSt == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            student student = db.students.Find(vpisnaSt);
            if (student == null)
            {
                return HttpNotFound();
            }

            ViewBag.sklep = student.skleps.ToList();

            if (User.IsInRole("Profesor"))
            {
                /*
                UserHelper uh = new UserHelper();
                var profesor = uh.FindByName(User.Identity.Name);
                var izvajanja = db.profesors.Where(p => p.id == profesor.id).First().izvajanjes.ToList(); // namesto p.id mora bit nek drug ID

                */
                var izvajanja = db.profesors.Where(p => p.id == 93).First().izvajanjes.ToList(); // TEMP!!

                List<izpitnirok> roki = new List<izpitnirok>();
                foreach (var i in izvajanja)
                {
                    foreach (var r in i.izpitniroks.ToList())
                    {
                        roki.Add(r);
                    }
                }
                ViewBag.Roki = roki;

                var vpis = db.vpis.Where(v => v.vpisnaStevilka == vpisnaSt).ToList();

                List<prijavanaizpit> prijave = new List<prijavanaizpit>();
                foreach (var v in vpis)
                {
                    foreach (var p in v.prijavanaizpits.Where(p => p.stanje == 2 || p.stanje == 3).ToList())
                    {
                        prijave.Add(p);
                    }
                }
                ViewBag.Prijave = prijave;
            }
            
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="vpisna_stevilka,ime,priimek,naslov,datum_rojstva,spol")] student student)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Students");
            }

            return View(student);
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="vpisna_stevilka,ime,priimek,naslov,datum_rojstva,spol")] student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Students");
            }
            return View(student);
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            student student = db.students.Find(id);
            db.students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Students");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
