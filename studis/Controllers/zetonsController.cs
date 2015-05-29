using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using studis.Models;

namespace studis
{
    [Authorize(Roles = "Referent")]
    public class zetonsController : Controller
    {
        private studisEntities db = new studisEntities();

        // GET: zetons
        public ActionResult Index()
        {
            var zetons = db.zetons.Include(z => z.sifrant_klasius).Include(z => z.sifrant_letnik).Include(z => z.sifrant_oblikastudija).Include(z => z.sifrant_studijskiprogram).Include(z => z.sifrant_vrstavpisa).Include(z => z.student).Where(z => z.porabljen == false);
            return View(zetons.ToList());
        }

        // GET: zetons/Create
        public ActionResult Create(int id)
        {
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.letnik = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.oblikaStudija = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.id != 1000468).ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.vrstaVpisa = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            return View();
        }

        // POST: zetons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "letnik,studijskiProgram,vrstaVpisa,vrstaStudija,oblikaStudija")] zeton zeton, int id)
        {
            student s = db.students.Find(id);
            vpi zadnjivpisUNI = s.vpis.Where(a => a.studijskiProgram == 1000468).Last();

            if (ModelState.IsValid)
            {
                if (zeton.studijskiProgram == 1000468) { //UNI
                    //preveri če se vpisuje v višji če ni ponavljanje
                    if (zeton.letnik <= zadnjivpisUNI.letnikStudija && zeton.vrstaVpisa != 2)
                    {
                        ModelState.AddModelError("", "Premajhen letnik študija");
                    } //preveri če se vpisuje v isti če je ponavljanje
                    else if (zeton.vrstaVpisa == 2 && zeton.letnik != zadnjivpisUNI.letnikStudija)
                    {
                        ModelState.AddModelError("", "Letnik mora biti enak zadnjemu vpisu, ker gre za ponavljanje");
                    } //preveri ce je absolvent da ima vpis v 3 letnik
                    else if (zeton.letnik == 0 && s.vpis.Where(a => a.letnikStudija == 3).Where(a => a.studijskiProgram == 1000468).FirstOrDefault() == null)
                    {
                        ModelState.AddModelError("", "Študent se ne more vpisati v dodatno leto ker nima opravljenega 3. letnika UNI");
                    }
                } //mag prvi letnik
                else if (zeton.studijskiProgram == 1000471 && zeton.letnik == 1) {
                    if (s.vpis.Where(a => a.letnikStudija == 3).Where(a => a.studijskiProgram == 1000468).FirstOrDefault() == null) {
                        ModelState.AddModelError("", "Študent še ni bil vpisan v 3. letnik UNI");
                    }
                } //mag neprvi
                else if (zeton.studijskiProgram == 1000471 && zeton.letnik == 1) {
                    ModelState.AddModelError("", "Podprt je samo prvi letnik magisterskega študija");
                } //preveri pavziranje
                else if (zeton.vrstaVpisa == 3 && (zeton.letnik != zadnjivpisUNI.letnikStudija))
                {
                    ModelState.AddModelError("", "Študent lahko pavzira le zadnji vpisan letnik");
                } //vpis za zaključek le če ima tretji in uni
                else if (s.vpis.Where(a => a.letnikStudija == 3).Where(a => a.studijskiProgram == 1000468).FirstOrDefault() == null && zeton.studijskiProgram == 1000468)
                {
                    ModelState.AddModelError("", "Študent nima vpisa za 3. letnik UNI");
                }

                if (ModelState.IsValid)
                {

                    zeton.porabljen = false;
                    zeton.vpisnaStevilka = id;
                    db.zetons.Add(zeton);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                
            }

            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.letnik = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.oblikaStudija = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.id != 1000468).ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.vrstaVpisa = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            return View(zeton);
        }

        // GET: zetons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            zeton zeton = db.zetons.Find(id);
            if (zeton == null || zeton.porabljen)
            {
                return HttpNotFound();
            }
            ViewBag.vrstaStudijaVB = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.letnikVB = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.oblikaStudijaVB = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.studijskiProgramVB = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.id != 1000468).ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.vrstaVpisaVB = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            ViewBag.vpisnaStevilkaVB = new SelectList(db.students, "vpisnaStevilka", "ime", zeton.vpisnaStevilka);

            return View(zeton);
        }

        // POST: zetons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,vpisnaStevilka,letnik,studijskiProgram,porabljen,vrstaVpisa,vrstaStudija,oblikaStudija")] zeton zeton)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zeton).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("err", "Napaka pri enem izmed polj");
            }
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius.OrderBy(a => a.id != 16204).ThenBy(b => b.id), "id", "naziv");
            ViewBag.letnik = new SelectList(db.sifrant_letnik.OrderBy(a => a.naziv != "Prvi").ThenBy(b => b.id), "id", "naziv");
            ViewBag.oblikaStudija = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.id != 1000468).ThenBy(a => a.naziv), "id", "naziv");
            ViewBag.vrstaVpisa = new SelectList(db.sifrant_vrstavpisa, "id", "naziv");
            ViewBag.vpisnaStevilka = new SelectList(db.students, "vpisnaStevilka", "ime", zeton.vpisnaStevilka);
            return View(zeton);
        }

        // GET: zetons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            zeton zeton = db.zetons.Find(id);
            if (zeton == null || zeton.porabljen)
            {
                return HttpNotFound();
            }
            return View(zeton);
        }

        // POST: zetons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            zeton zeton = db.zetons.Find(id);
            if (zeton.porabljen)
            {
                return HttpNotFound();
            }
            db.zetons.Remove(zeton);
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
