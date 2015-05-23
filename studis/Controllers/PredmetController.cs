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
    public class PredmetController : Controller
    {
        private studisEntities db = new studisEntities();

        // GET: /Predmet/
        public ActionResult Predmeti()
        {
            var predmets = db.predmets;
            ViewBag.Seznam = new SelectList((from s in predmets.ToList() select new { id = s.id, nameCode = s.koda + " - " + s.ime }), "id", "nameCode", null);

            var sList = new SelectList(new[] 
            {
                new { Value = "2013", Text = "2013/2014" },
                new { Value = "2014", Text = "2014/2015" },
                new { Value = "2015", Text = "2015/2016" },
                
            },
            "Value", "Text", 1);
            ViewData["sList"] = sList;

            return View();
        }

        [HttpPost]
        public ActionResult Predmeti(long id, string Value)
        {
            int leto = Convert.ToInt32(Value);

            //vsi študenti
            var students = db.students.Include(p => p.studentinpredmets).Include(p => p.vpis).ToList();

            //studenti iz povezovalne tabele, ki imajo ta predmet v iskanem letu
            var povezovalna = from p in db.studentinpredmets select p; 
            povezovalna = povezovalna.Where(p => p.predmetId == id && (p.vpi.studijskoLeto == leto || leto==0)).Distinct();

            List<student> list = new List<student>();
            foreach(var vpisna in povezovalna)
            {
                student st = students.Where(s => s.vpisnaStevilka == vpisna.studentId).SingleOrDefault();
                if (st != null)
                {
                    list.Add(st);
                }
            }

            if (list.Any())
                list = list.OrderBy(o => o.priimek).ToList();
            else
                list = null;

            return View("PredmetStudenti", list);
        }

        // GET: /Predmet/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            predmet predmet = db.predmets.Find(id);
            if (predmet == null)
            {
                return HttpNotFound();
            }
            return View(predmet);
        }

        // GET: /Predmet/Create
        public ActionResult Create()
        {
            ViewBag.modulId = new SelectList(db.moduls, "id", "ime");
            ViewBag.letnik = new SelectList(db.sifrant_letnik, "id", "naziv");
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram, "id", "naziv");
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius, "id", "naziv");
            return View();
        }

        // POST: /Predmet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,ime,opis,kreditne,semester,koda,letnik,obvezen,modulId,prostoizbirni,strokovnoizbirni,studijskiProgram,vrstaStudija")] predmet predmet)
        {
            if (ModelState.IsValid)
            {
                db.predmets.Add(predmet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.modulId = new SelectList(db.moduls, "id", "ime", predmet.modulId);
            ViewBag.letnik = new SelectList(db.sifrant_letnik, "id", "naziv", predmet.letnik);
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram, "id", "naziv", predmet.studijskiProgram);
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius, "id", "naziv", predmet.vrstaStudija);
            return View(predmet);
        }

        // GET: /Predmet/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            predmet predmet = db.predmets.Find(id);
            if (predmet == null)
            {
                return HttpNotFound();
            }
            ViewBag.modulId = new SelectList(db.moduls, "id", "ime", predmet.modulId);
            ViewBag.letnik = new SelectList(db.sifrant_letnik, "id", "naziv", predmet.letnik);
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram, "id", "naziv", predmet.studijskiProgram);
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius, "id", "naziv", predmet.vrstaStudija);
            return View(predmet);
        }

        // POST: /Predmet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,ime,opis,kreditne,semester,koda,letnik,obvezen,modulId,prostoizbirni,strokovnoizbirni,studijskiProgram,vrstaStudija")] predmet predmet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(predmet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.modulId = new SelectList(db.moduls, "id", "ime", predmet.modulId);
            ViewBag.letnik = new SelectList(db.sifrant_letnik, "id", "naziv", predmet.letnik);
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram, "id", "naziv", predmet.studijskiProgram);
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius, "id", "naziv", predmet.vrstaStudija);
            return View(predmet);
        }

        // GET: /Predmet/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            predmet predmet = db.predmets.Find(id);
            if (predmet == null)
            {
                return HttpNotFound();
            }
            return View(predmet);
        }

        // POST: /Predmet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            predmet predmet = db.predmets.Find(id);
            db.predmets.Remove(predmet);
            db.SaveChanges();
            return RedirectToAction("Index");
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
