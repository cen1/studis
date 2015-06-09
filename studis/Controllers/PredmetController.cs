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
            var predmets = db.predmets.OrderBy(a => a.ime);
            ViewBag.Seznam = new SelectList((from s in predmets.ToList() select new { id = s.id, nameCode = s.koda + " - " + s.ime }), "id", "nameCode", null);

            var sList = new SelectList(new[] 
            {
                new { Value = "2010", Text = "2010/2011" },
                new { Value = "2011", Text = "2011/2012" },
                new { Value = "2012", Text = "2012/2013" },
                new { Value = "2013", Text = "2013/2014" },
                new { Value = "2014", Text = "2014/2015" },
                new { Value = "2015", Text = "2015/2016" }
            },
            "Value", "Text", 1);
            ViewData["sList"] = sList;

            return View();
        }

        [HttpPost]
        public ActionResult Predmeti(long id, string Value)
        {
            //izbrano štud.leto
            int leto = Convert.ToInt32(Value);
            
            //pridobi izvajanje za določen predmet
            var izvajanja = db.izvajanjes.Where(izv => izv.predmetId == id).ToList();

            //najdi vse študente ki ustrezajo kriterijem
            List<student> list = new List<student>();
            List<string> vrstaVpisa = new List<string>();

            foreach(izvajanje izvajanje in izvajanja)
            {
                //foreach (izvajanjeleto letoizvajanja in izvajanje.izvajanjeletoes)
                //{
                    //predavanja v iskanem letu
                    //if (letoizvajanja.sifrant_studijskoleto.id == leto)
                   // {
                        //System.Diagnostics.Debug.WriteLine(letoizvajanja.sifrant_studijskoleto.id+"="+leto);
                        foreach (vpi vpis in izvajanje.vpis)
                        {
                            if (vpis.studijskoLeto == leto && vpis.potrjen==true)
                            {
                                student st = db.students.Where(s => s.vpisnaStevilka == vpis.vpisnaStevilka).SingleOrDefault();
                                if (st != null)
                                {
                                    list.Add(st);
                                }
                            }
                        }
                    //}
                //}
            }

            //uredi seznam študentov
            if (list.Any())
            {
                list = list.OrderBy(o => o.priimek).ToList();

                //naredi seznam "vrst vpisov", v enakem vrstnem redu kot so študenti
                foreach (var student in list)
                {
                    vpi vpis = student.vpis.Where(v => v.studijskoLeto == leto).SingleOrDefault();
                    string temp = vpis.sifrant_vrstavpisa.naziv;
                    vrstaVpisa.Add(temp);
                }
                ViewBag.vrstaVpisa = vrstaVpisa;
            }
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
