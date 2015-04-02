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
    public class StudentController : Controller
    {
        private studisEntities db = new studisEntities();

        // GET: /Student/
        public ActionResult Students()
        {
            return View();
        }

        public ActionResult StudentSearchPartial(string searchString)
        {
            var students = from s in db.students select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                int vpisna;
                bool isNumerical = int.TryParse(searchString, out vpisna);
                if (isNumerical)
                    students = students.Where(s => s.vpisna_stevilka == vpisna);
                else
                {
                    string[] searchData = searchString.Split(null);

                    if (searchData.Length > 1)
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
                        var tempStudents = students.Where(s => s.ime.Contains(searchString));
                        if (tempStudents.Any())
                            students = tempStudents;
                        else
                            students = students.Where(s => s.priimek.Contains(searchString));
                    }
                }
            }
            return PartialView("_StudentSearchPartial",students);
        }

        // GET: /Student/Details/5
        public ActionResult Details(int? id)
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
