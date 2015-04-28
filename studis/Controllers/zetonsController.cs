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
            var zetons = db.zetons.Include(z => z.sifrant_klasius).Include(z => z.sifrant_letnik).Include(z => z.sifrant_oblikastudija).Include(z => z.sifrant_studijskiprogram).Include(z => z.sifrant_vrstavpisa).Include(z => z.student);
            return View(zetons.ToList());
        }

        // GET: zetons/Create
        public ActionResult Create(int id)
        {
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius, "id", "naziv");
            ViewBag.letnik = new SelectList(db.sifrant_letnik, "id", "naziv");
            ViewBag.oblikaStudija = new SelectList(db.sifrant_oblikastudija, "id", "naziv");
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram, "id", "naziv");
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
            if (ModelState.IsValid)
            {
                zeton.porabljen = false;
                zeton.vpisnaStevilka = id;
                db.zetons.Add(zeton);
                db.SaveChanges();
                if (zeton.studijskiProgram != 1000468 || zeton.letnik>=4 || zeton.vrstaStudija != 16204)
                    TempData["warning"] = "Dodali ste žeton s parametri za program, ki trenutno ni podprt. Še enkrat poglejte če je to res bil vaš namen.";
                return RedirectToAction("Index");
            }

            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius, "id", "naziv", zeton.vrstaStudija);
            ViewBag.letnik = new SelectList(db.sifrant_letnik, "id", "naziv", zeton.letnik);
            ViewBag.oblikaStudija = new SelectList(db.sifrant_oblikastudija, "id", "naziv", zeton.oblikaStudija);
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram, "id", "naziv", zeton.studijskiProgram);
            ViewBag.vrstaVpisa = new SelectList(db.sifrant_vrstavpisa, "id", "naziv", zeton.vrstaVpisa);
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
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius, "id", "naziv", zeton.vrstaStudija);
            ViewBag.letnik = new SelectList(db.sifrant_letnik, "id", "naziv", zeton.letnik);
            ViewBag.oblikaStudija = new SelectList(db.sifrant_oblikastudija, "id", "naziv", zeton.oblikaStudija);
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram, "id", "naziv", zeton.studijskiProgram);
            ViewBag.vrstaVpisa = new SelectList(db.sifrant_vrstavpisa, "id", "naziv", zeton.vrstaVpisa);
            ViewBag.vpisnaStevilka = new SelectList(db.students, "vpisnaStevilka", "ime", zeton.vpisnaStevilka);
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
            ViewBag.vrstaStudija = new SelectList(db.sifrant_klasius, "id", "naziv", zeton.vrstaStudija);
            ViewBag.letnik = new SelectList(db.sifrant_letnik, "id", "naziv", zeton.letnik);
            ViewBag.oblikaStudija = new SelectList(db.sifrant_oblikastudija, "id", "naziv", zeton.oblikaStudija);
            ViewBag.studijskiProgram = new SelectList(db.sifrant_studijskiprogram, "id", "naziv", zeton.studijskiProgram);
            ViewBag.vrstaVpisa = new SelectList(db.sifrant_vrstavpisa, "id", "naziv", zeton.vrstaVpisa);
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
