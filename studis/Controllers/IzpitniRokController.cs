using studis.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace studis.Controllers
{
    [Authorize(Roles = "Profesor, Referent")]
    public class IzpitniRokController : Controller
    {
        public studisEntities db = new studisEntities();

        public ActionResult Index()
        {
            return View();
        }

        //GET: IzpitniRok/Dodaj
        public ActionResult Dodaj()
        {
            List<predmet> temp = db.predmets.OrderBy(a => a.ime).ToList();

            List<SelectListItem> predmeti = new List<SelectListItem>();
            foreach (predmet i in temp)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.id.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.ime + " (" + i.koda + ")";
                predmeti.Add(p);
            }
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izbira izvajalcev" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            //new SelectList(db.sifrant_studijskiprogram.OrderBy(a => a.naziv), "id", "naziv");
            //IzpitniRokModel model= new IzpitniRokModel();

            var uraSeznam = Enumerable.Range(0, 24).Select(i => new SelectListItem
               {
                   Value = i.ToString(),
                   Text = i.ToString("00")
               });
            var minuntaSeznam = Enumerable.Range(0, 60).Select(i => new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString("00")
                });
            ViewBag.ura = uraSeznam;
            ViewBag.minuta = minuntaSeznam;

            var prostorSeznam = Enumerable.Range(1, 22).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = "P" + i.ToString("00")
            });

            ViewBag.prostor = prostorSeznam;

            return View();
        }

        // POST: IzpitniRok/Dodaj
        [HttpPost]
        public ActionResult Dodaj(IzpitniRokModel model)
        {
            izpitnirok izpitniRok = new izpitnirok();
            izpitniRok.datum = UserHelper.StringToDate(model.datum);
            izpitniRok.izvajanje = db.izvajanjes.SingleOrDefault(i => i.id == model.izvajanje);//db.predmets.SingleOrDefault(v => v.id == model.predmet);
            //izpitniRok.profesor = db.profesors.SingleOrDefault(p => p.id == model.profesor);
            if(model.ura != 0)
                izpitniRok.ura = UserHelper.IntsToTime(model.ura, model.minuta);
            if(model.prostor != 0)
                izpitniRok.prostorId = model.prostor;
            try
            {
                // TODO: Add insert logic here
                db.izpitniroks.Add(izpitniRok);
                db.SaveChanges();
                return View("UspesnoDodan");
            }
            catch
            {
                return Dodaj();
            }
        }

        // GET: IzpitniRok/Edit
        public ActionResult Edit()
        {
            List<predmet> temp = db.predmets.OrderBy(a => a.ime).ToList();

            List<SelectListItem> predmeti = new List<SelectListItem>();
            foreach (predmet i in temp)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.id.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.ime + " (" + i.koda + ")";
                predmeti.Add(p);
            }
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izbira" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");

            var uraSeznam = Enumerable.Range(0, 24).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString("00")
            });
            var minuntaSeznam = Enumerable.Range(0, 60).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = i.ToString("00")
            });
            ViewBag.ura = uraSeznam;
            ViewBag.minuta = minuntaSeznam;

            var prostorSeznam = Enumerable.Range(1, 22).Select(i => new SelectListItem
            {
                Value = i.ToString(),
                Text = "P" + i.ToString("00")
            });

            ViewBag.prostor = prostorSeznam;

            return View();
        }

        // POST: IzpitniRok/Edit
        [HttpPost]
        public ActionResult Edit(IzpitniRokModel model)
        {

            try
            {
                // TODO: Add update logic here
                var rok = db.izpitniroks.SingleOrDefault(r => r.id == model.id);

                rok.datum = UserHelper.StringToDate(model.datum);
                //rok.predmet = db.predmets.SingleOrDefault(v => v.id == model.predmet);
                if (model.ura != 0)
                {
                    rok.ura = UserHelper.IntsToTime(model.ura, model.minuta);
                }
                else
                {
                    rok.ura = null;
                }
                if (model.prostor != 0)
                {
                    rok.prostorId = model.prostor;
                }
                else
                {
                    rok.prostorId = null;
                }
                Debug.WriteLine("Prostor " + model.prostor);
                db.SaveChanges();
                return View("UspesnoSpremenjen");
            }
            catch
            {
                return Edit();
            }
        }

        // GET: IzpitniRok/Delete/5
        public ActionResult Izbrisi()
        {
            List<predmet> temp = db.predmets.OrderBy(a => a.ime).ToList();

            List<SelectListItem> predmeti = new List<SelectListItem>();
            foreach (predmet i in temp)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.id.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.ime + " (" + i.koda + ")";
                predmeti.Add(p);
            }
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izbira" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");
            return View();
        }

        // POST: IzpitniRok/Delete/5
        [HttpPost]
        public ActionResult Izbrisi(int id)
        {
            try
            {
                // TODO: Add delete logic here
                db.izpitniroks.Remove(db.izpitniroks.SingleOrDefault(r => r.id == id));
                db.SaveChanges();
                return View("UspesnoIzbrisan");
            }
            catch
            {
                return Izbrisi();
            }
        }

        // GET: IzpitniRok/Seznam/5
        public ActionResult Seznam()
        {
            List<predmet> temp = db.predmets.OrderBy(a => a.ime).ToList();

            List<SelectListItem> predmeti = new List<SelectListItem>();
            foreach (predmet i in temp)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.id.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.ime + " (" + i.koda + ")";
                predmeti.Add(p);
            }
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izbira" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");
            return View();
        }

        // POST: IzpitniRok/Seznam/5
        [HttpPost]
        public ActionResult Seznam(string id)
        {
            try
            {
                int idRoka = Convert.ToInt32(id);
                izpitnirok rok = db.izpitniroks.Where(i => i.id == idRoka).SingleOrDefault();

                return RedirectToAction("SeznamPrijavljenihKandidatov", rok);
            }
            catch
            {
                return Seznam();
            }
        }

        public ActionResult SeznamPrijavljenihKandidatov(izpitnirok rok)
        {
            //izpitnirok tmp = rok;
            sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
            izvajanje izv = db.izvajanjes.Where(s => s.id == rok.izvajanjeId).SingleOrDefault();

            ViewBag.prostor = predavalnica.naziv;
            ViewBag.datum = GetDatumForIzpitniRok(rok.id);//rok.datum;
            ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);//GetUraForIzpitniRok(rok.id)+":"+GetMinutaForIzpitniRok(rok.id);//rok.ura;
            ViewBag.sifraPredmeta = izv.predmetId;
            ViewBag.imePredmeta = izv.predmet.ime;

            return View();
        }


        /*
        public string GetProfesorsForPredmet(int id)
        {
            
            int iid = Convert.ToInt32(id);
            List<profesor> profesors;
            try
            {
                profesors = db.predmets.SingleOrDefault(v => v.id == iid).profesors.ToList();
            } catch {
                //Debug.WriteLine("GetProfesorsForPredmet/" + id + "prazen seznam!");
                profesors = new List<profesor>();
            }
            var seznamProfesorjev = new List<SelectListItem>();
            int c = 0;
            foreach (profesor p in profesors)
            {
                c++;
                seznamProfesorjev.Add(new SelectListItem() { Value = p.id.ToString(), Text = (p.ime + " " + p.priimek) });
            }
            if (c < 1)
            {
                seznamProfesorjev.Add(new SelectListItem() { Value = "0", Text = "Ta predmet nima nosilca." });
            }
            /*
            var seznam = new List<SelectListItem>();
            Random rand= new Random();
            for(int i = 0; i< 5; i++) {
                seznam.Add(new SelectListItem() { Value = i.ToString(), Text = rand.Next(i, 100).ToString() });
            }
            
            return new JavaScriptSerializer().Serialize(seznamProfesorjev);
        } 
        */

        public string GetIzvajanjaForPredmet(int id)
        {
            /*
             * TO DO TO DO TO DO TO DO TO DO TO DO
             */
            Debug.WriteLine("ID " + id);
            int iid = Convert.ToInt32(id);
            Debug.WriteLine("ID " + iid);
            var pPredmet = db.predmets.SingleOrDefault(p => p.id == iid);
            var izvajanja = pPredmet.izvajanjes.ToList();//izpitniroks.ToList(); //Exception 
            var seznamIzvajanja = new List<SelectListItem>();
            int c = 0;
            foreach (izvajanje i in izvajanja)
            {
                c++;
                string profesorji = "";
                var profesor1 = i.profesor;//db.profesors.SingleOrDefault( p => p.id == i.izvajalec1Id);
                profesorji += profesor1.ime + " " + profesor1.priimek;
                if (i.profesor1 != null)
                {
                    var profesor2 = i.profesor1;// db.profesors.SingleOrDefault(p => p.id == i.izvajalec2Id);
                    profesorji += ", " + profesor2.ime + " " + profesor2.priimek;
                }
                if (i.profesor2 != null)
                {
                    var profesor3 = i.profesor2;//db.profesors.SingleOrDefault(p => p.id == i.izvajalec3Id);
                    profesorji += ", " + profesor3.ime + " " + profesor3.priimek;
                }

                seznamIzvajanja.Add(new SelectListItem() { Value = i.id.ToString(), Text = profesorji });
            }
            if (c < 1)
            {
                seznamIzvajanja.Add(new SelectListItem() { Value = "", Text = "Ni izvajalcev." });
            }
            else
            {
                seznamIzvajanja.Insert(0, new SelectListItem() { Value = "", Text = "Izberi" });
            }
            return new JavaScriptSerializer().Serialize(seznamIzvajanja);
        }

        public string GetIzpitniRoksForIzvajanja(int id)
        {

            Debug.WriteLine("ID " + id);
            int iid = Convert.ToInt32(id);
            Debug.WriteLine("ID " + iid);
            var izvajanje = db.izvajanjes.SingleOrDefault(i => i.id == iid);//db.predmets.SingleOrDefault(p => p.id == iid);
            var izpitniRoki = izvajanje.izpitniroks;//pPredmet.izpitniroks.ToList(); //Exception 
            var seznamIzpitniRoki = new List<SelectListItem>();
            int c = 0;
            foreach (izpitnirok i in izpitniRoki)
            {
                c++;
                string prostor = "";
                if (i.sifrant_prostor != null)
                {
                    prostor= i.sifrant_prostor.naziv;
                }
                string ura = "";
                if (i.ura != null)
                {
                    ura = UserHelper.TimeToString((DateTime)i.ura);
                }
                seznamIzpitniRoki.Add(new SelectListItem() { Value = i.id.ToString(), Text = UserHelper.DateToString(i.datum) + " " + ura + " " + prostor});
            }
            if (c < 1)
            {
                seznamIzpitniRoki.Add(new SelectListItem() { Value = "", Text = "Ta predmet nima razpisanih rokov." });
            }
            else
            {
                seznamIzpitniRoki.Insert(0, new SelectListItem() { Value = "", Text = "Izberi" });
            }
            return new JavaScriptSerializer().Serialize(seznamIzpitniRoki);
        }

        public string GetDatumForIzpitniRok(int id)
        {

            int iid = Convert.ToInt32(id);
            var datum = db.izpitniroks.SingleOrDefault(r => r.id == iid).datum;
            return UserHelper.DateToString(datum);
        }

        public string GetUraForIzpitniRok(int id)
        {
            int iid = Convert.ToInt32(id);
            var cas = db.izpitniroks.SingleOrDefault(r => r.id == iid).ura;
            if (cas != null)
            {
                int[] t = UserHelper.TimeToInts((DateTime)cas);
                return t[0].ToString();
            }
            else
            {
                return "";
            }
        }

        public string GetMinutaForIzpitniRok(int id)
        {
            int iid = Convert.ToInt32(id);
            var cas = db.izpitniroks.SingleOrDefault(r => r.id == iid).ura;
            if (cas != null)
            {
                int[] t = UserHelper.TimeToInts((DateTime)cas);
                return t[1].ToString();
            }
            else
            {
                return "";
            }
        }

        public string GetProstorForIzpitniRok(int id)
        {
            int iid = Convert.ToInt32(id);
            var prostor = db.izpitniroks.SingleOrDefault(r => r.id == iid).prostorId;
            if (prostor != 0)
            {
                return prostor.ToString();
            }
            else
            {
                return "";
            }
        }

        public JsonResult PreveriDatum(string datum)
        {
            Debug.WriteLine("datum: " + datum);
            DateTime d = UserHelper.StringToDate(datum);
            Debug.WriteLine("Datum: " + d);
            var result = Validate.veljavenDatum(d);
            if (d < DateTime.Today)
            {
                result = false;
            }
            return Json(result);
        }

        public JsonResult PreveriIzpitniRok(int id)
        {
            Debug.WriteLine("PreveriIzpitniRok(" + id + ")");
            int steviloOcen = 0;
            try
            {
                steviloOcen = db.izpitniroks.SingleOrDefault(r => r.id == id).ocenas.Count;
            }
            catch (Exception e)
            {
                steviloOcen = 0;
            }
            var result = true;
            if (steviloOcen > 0)
            {
                result = false;
            }
            return Json(result);
        }


        public string PreveriPrijave(int id)
        {
            int st = 0;
            try
            {
                st = db.izpitniroks.SingleOrDefault(r => r.id == id).students.Count;
            }
            catch (Exception e) { st = 0; }
            return st.ToString(); ;
        }

    }
}
