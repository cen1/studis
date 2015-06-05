using studis.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.Entity;

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
                izpitniRok.fiktiven = false;
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
            List<SelectListItem> predmeti = new List<SelectListItem>();

            if (User.IsInRole("Referent"))
            {
                List<predmet> temp = db.predmets.OrderBy(a => a.ime).ToList();
                foreach (predmet i in temp)
                {
                    SelectListItem p = new SelectListItem();
                    p.Value = i.id.ToString();
                    p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.ime + " (" + i.koda + ")";
                    predmeti.Add(p);
                }
            }
            else if (User.IsInRole("Profesor"))
            {
                int profid = Convert.ToInt32(UserHelper.getProfByName(User.Identity.Name).id);
                var izvajanja = db.izvajanjes.Where(a => a.izvajalec1Id == profid || a.izvajalec2Id == profid || a.izvajalec3Id == profid);
                if (izvajanja != null)
                {
                    foreach (izvajanje i in izvajanja.ToList())
                    {
                        SelectListItem p = new SelectListItem();
                        p.Value = i.predmetId.ToString();
                        p.Text = Convert.ToInt32(p.Value).ToString("000") + " - " + i.predmet.ime + " (" + i.predmet.koda + ")";
                        predmeti.Add(p);
                    }
                }
            }


            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Iščem.." });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            ViewBag.Predmets = new SelectList(predmeti, "Value", "Text");
            return View();
        }

        // POST: IzpitniRok/Seznam/5
        [HttpPost]
        public ActionResult Seznam(int id = -1)
        {
            try
            {
                izpitnirok rok = db.izpitniroks.Where(i => i.id == id).SingleOrDefault();

                return RedirectToAction("SeznamPrijavljenihKandidatov", rok);
            }
            catch
            {
                return Seznam();
            }
        }

        public ActionResult SeznamPrijavljenihKandidatov(izpitnirok rok)
        {
            if(rok == null)
                return RedirectToAction("Seznam");

            //podatki o izpitnem roku
            sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
            izvajanje izv = db.izvajanjes.Where(s => s.id == rok.izvajanjeId).SingleOrDefault();

            string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
            if (izv.izvajalec2Id != null)
                izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
            if (izv.izvajalec3Id != null)
                izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

            ViewBag.idRoka = rok.id;
            ViewBag.izvajalci = izvajalci;
            ViewBag.prostor = predavalnica.naziv;
            ViewBag.datum = GetDatumForIzpitniRok(rok.id);
            ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
            ViewBag.sifraPredmeta = izv.predmetId;
            ViewBag.imePredmeta = izv.predmet.ime;


            //pridobi prijavljene študente
            var prijave = db.prijavanaizpits.Where( p => p.izpitnirokId == rok.id).ToList();

            List<VnosTockModel> listVnosov = new List<VnosTockModel>();
            StudentHelper sh = new StudentHelper();
            foreach(prijavanaizpit prijava in prijave)
            {
                vpi vpiss = db.vpis.Where(v => v.id == prijava.vpisId).SingleOrDefault();
                student st = db.students.Where(s => s.vpisnaStevilka == vpiss.vpisnaStevilka).SingleOrDefault();
                VnosTockModel vnos = new VnosTockModel();

                if (st != null)
                {
                    vnos.idRoka = rok.id;
                    vnos.vpisnaStevilka = st.vpisnaStevilka;
                    vnos.ime = st.ime;
                    vnos.priimek = st.priimek;
                    vnos.studijskoLeto = vpiss.sifrant_studijskoleto.naziv;
                    vnos.zaporednoSteviloPonavljanja = sh.zaporednoPolaganje(st.vpisnaStevilka, (int)izv.id, vpiss.studijskiProgram, prijava.izpitnirok.datum);
                    listVnosov.Add(vnos);
                }
            }

            if (listVnosov.Any())
            {
                //uredi seznam študentov
                listVnosov = listVnosov.OrderBy(o => o.priimek).ToList();

                int zaporednaSt = 0;
                foreach (var item in listVnosov)
                {
                    zaporednaSt = zaporednaSt + 1;
                    item.zaporednaStevilka = zaporednaSt;
                }
            }
            else
                listVnosov = null;

            //dodaj linke za vpis točk in ocen? ..prikaži za trenutno šolsko leto in če rok ni v prihodnosti
            StudentHelper uh = new StudentHelper();
            if (rok.datum.Year == uh.trenutnoSolskoLeto()+1 && rok.datum < DateTime.Now)
                ViewBag.ocenetocke = true;
            else
                ViewBag.ocenetocke = false;

            return View(listVnosov);
        }


        public ActionResult VpisTock(int rokID = -1)
        {
            if (rokID == -1)
                return RedirectToAction("Seznam");

            //podatki o izpitnem roku
            izpitnirok rok = db.izpitniroks.Where(r => r.id == rokID).SingleOrDefault();

            sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
            izvajanje izv = db.izvajanjes.Where(i => i.id == rok.izvajanjeId).SingleOrDefault();

            string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
            if (izv.izvajalec2Id != null)
                izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
            if (izv.izvajalec3Id != null)
                izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

            ViewBag.idRoka = rok.id;
            ViewBag.izvajalci = izvajalci;
            ViewBag.prostor = predavalnica.naziv;
            ViewBag.datum = GetDatumForIzpitniRok(rok.id);
            ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
            ViewBag.sifraPredmeta = izv.predmetId;
            ViewBag.imePredmeta = izv.predmet.ime;


            //pridobi prijavljene študente
            List<VnosTockModel> listVnosov = new List<VnosTockModel>();
            StudentHelper sh = new StudentHelper();

            var prijave = db.prijavanaizpits.Where(p => p.izpitnirokId == rok.id).ToList();
            
            foreach (prijavanaizpit prijava in prijave)
            {
                vpi vpiss = db.vpis.Where(v => v.id == prijava.vpisId).SingleOrDefault();
                student st = db.students.Where(s => s.vpisnaStevilka == vpiss.vpisnaStevilka).SingleOrDefault();
                VnosTockModel vnos = new VnosTockModel();

                if (st != null)
                {
                    vnos.idRoka = rokID;
                    vnos.vpisnaStevilka = st.vpisnaStevilka;
                    vnos.ime = st.ime;
                    vnos.priimek = st.priimek;
                    vnos.studijskoLeto = vpiss.sifrant_studijskoleto.naziv;
                    vnos.zaporednoSteviloPonavljanja = sh.zaporednoPolaganje(st.vpisnaStevilka, (int)izv.id, vpiss.studijskiProgram, prijava.izpitnirok.datum);

                    try
                    {
                        //preveri če že obstaja vnos?
                        tocke tocke = db.tockes.Where( t => t.prijavaId == prijava.id).FirstOrDefault();

                        if (prijava.stanje != 4)
                        {
                            vnos.zeVpisaneTocke = tocke.tocke1.ToString();
                        }
                        else
                        { 
                            vnos.zeVpisaneTocke = "VP";
                        }
                    }
                    catch (Exception e)
                    {
                        vnos.zeVpisaneTocke = "/";
                        //Debug.WriteLine("Tocke za tega studenta in prijavo niso še vnesene..");
                    }

                    listVnosov.Add(vnos);
                }
            }

            if (listVnosov.Any())
            {
                //uredi seznam študentov
                listVnosov = listVnosov.OrderBy(o => o.priimek).ToList();

                int zaporednaSt = 0;
                foreach (var item in listVnosov)
                {
                    zaporednaSt = zaporednaSt + 1;
                    item.zaporednaStevilka = zaporednaSt;
                }
            }
            else
                listVnosov = null;

            return View(listVnosov);
        }

        [HttpPost]
        public ActionResult VpisTock(IList<studis.Models.VnosTockModel> list)
        {
            UserHelper uHelper = new UserHelper();
            if (list.Any())
            {
                foreach (VnosTockModel m in list)
                {
                    //podatki o izpitnem roku
                    if(m.zaporednaStevilka==1)
                    {
                        ViewBag.idRoka = m.idRoka; //parameter za klic view z izpisom
                        izpitnirok rok = db.izpitniroks.Where(r => r.id == m.idRoka).SingleOrDefault();

                        sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
                        izvajanje izv = db.izvajanjes.Where(i => i.id == rok.izvajanjeId).SingleOrDefault();

                        string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
                        if (izv.izvajalec2Id != null)
                            izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
                        if (izv.izvajalec3Id != null)
                            izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

                        ViewBag.idRoka = rok.id;
                        ViewBag.izvajalci = izvajalci;
                        ViewBag.prostor = predavalnica.naziv;
                        ViewBag.datum = GetDatumForIzpitniRok(rok.id);
                        ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
                        ViewBag.sifraPredmeta = izv.predmetId;
                        ViewBag.imePredmeta = izv.predmet.ime;
                    }

                    //vnos točk
                    if (ModelState.IsValid)
                    {                        
                        //vpisi tocke, če so bile vnese v view-u
                        if (m.tocke != null)
                        {
                            //VP=-1 tock(0 v bazi), drugače convert u int
                            int stTock=-1;
                            string stringTocke = m.tocke.ToLower();
                            if(!stringTocke.Equals("vp"))
                            {
                                stTock = Convert.ToInt32(m.tocke);
                            }
                            
                            vpi vpis = db.vpis.Where(v => v.vpisnaStevilka == m.vpisnaStevilka && v.sifrant_studijskoleto.naziv == m.studijskoLeto).FirstOrDefault();

                            prijavanaizpit prijava = db.prijavanaizpits.Where(p => p.izpitnirokId == m.idRoka && p.vpisId == vpis.id).FirstOrDefault();
                            tocke tocke = null;
                            try
                            {
                                //preveri če že obstaja vnos?
                                tocke = db.tockes.Where(t => t.prijavaId == prijava.id).FirstOrDefault();
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("Ni vnosa v bazi..");
                            }

                            //posodobi vnos
                            if (tocke != null)
                            {
                                try
                                {
                                    if (stTock == -1)
                                    {
                                        tocke.tocke1 = 0;
                                        prijava.stanje = 4; //VP??
                                        prijava.datumOdjave = DateTime.Now;
                                        prijava.odjavilId = uHelper.FindByName(User.Identity.Name).id;

                                        //sam za izpiz v viewu
                                        m.zeVpisaneTocke = "VP";
                                    }
                                    else
                                    {
                                        tocke.tocke1 = stTock;
                                        prijava.stanje = 2;

                                        //sam za izpiz v viewu
                                        m.zeVpisaneTocke = stTock.ToString();
                                    }
                                    tocke.prijavaId = prijava.id;
                                    tocke.datum = DateTime.Now;
                                    db.Entry(tocke).State = EntityState.Modified; //popravi točke v bazi
                                    db.Entry(prijava).State = EntityState.Modified; //nastavi stanje prijave v bazi
                                    db.SaveChanges();                                    
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine("Couldn't save changes to DB tocke!");
                                }
                            }
                            //nov vnos
                            else
                            {                                
                                try
                                {
                                    tocke = new tocke();
                                    if (stTock == -1)
                                    {
                                        tocke.tocke1 = 0;
                                        prijava.stanje = 4; //VP??
                                        prijava.datumOdjave = DateTime.Now;
                                        prijava.odjavilId = uHelper.FindByName(User.Identity.Name).id;

                                        //sam za izpiz v viewu
                                        m.zeVpisaneTocke = "VP";
                                    }
                                    else
                                    {
                                        tocke.tocke1 = stTock;
                                        prijava.stanje = 2;

                                        //sam za izpiz v viewu
                                        m.zeVpisaneTocke = stTock.ToString();
                                    }
                                    tocke.prijavaId = prijava.id;
                                    tocke.datum = DateTime.Now;
                                    db.tockes.Add(tocke); //vpiši točke v bazo
                                    db.Entry(prijava).State = EntityState.Modified; //nastavi stanje prijave v bazi
                                    db.SaveChanges();                                    
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine("Couldn't make new entry&save changes to DB tocke!");
                                }
                            }
                        }
                    }
                }
            }
            return View(list);
        }


        public ActionResult VpisOcen(int rokID = -1)
        {
            if (rokID == -1)
                return RedirectToAction("Seznam");


            //podatki o izpitnem roku
            izpitnirok rok = db.izpitniroks.Where(r => r.id == rokID).SingleOrDefault();

            sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
            izvajanje izv = db.izvajanjes.Where(i => i.id == rok.izvajanjeId).SingleOrDefault();

            string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
            if (izv.izvajalec2Id != null)
                izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
            if (izv.izvajalec3Id != null)
                izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

            ViewBag.idRoka = rok.id;
            ViewBag.izvajalci = izvajalci;
            ViewBag.prostor = predavalnica.naziv;
            ViewBag.datum = GetDatumForIzpitniRok(rok.id);
            ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
            ViewBag.sifraPredmeta = izv.predmetId;
            ViewBag.imePredmeta = izv.predmet.ime;


            //pridobi prijavljene študente
            List<VnosTockModel> listVnosov = new List<VnosTockModel>();
            StudentHelper sh = new StudentHelper();

            var prijave = db.prijavanaizpits.Where(p => p.izpitnirokId == rok.id).ToList();

            foreach (prijavanaizpit prijava in prijave)
            {
                vpi vpiss = db.vpis.Where(v => v.id == prijava.vpisId).SingleOrDefault();
                student st = db.students.Where(s => s.vpisnaStevilka == vpiss.vpisnaStevilka).SingleOrDefault();
                VnosTockModel vnos = new VnosTockModel();

                if (st != null)
                {
                    vnos.idRoka = rokID;
                    vnos.vpisnaStevilka = st.vpisnaStevilka;
                    vnos.ime = st.ime;
                    vnos.priimek = st.priimek;
                    vnos.studijskoLeto = vpiss.sifrant_studijskoleto.naziv;
                    vnos.zaporednoSteviloPonavljanja = sh.zaporednoPolaganje(st.vpisnaStevilka, (int)izv.id, vpiss.studijskiProgram, prijava.izpitnirok.datum);

                    try
                    {
                        //preveri če že obstaja vnos?
                        ocena ocena = db.ocenas.Where(t => t.prijavaId == prijava.id).FirstOrDefault();

                        if (prijava.stanje != 4)
                        {
                            vnos.zeVpisanaOcena = ocena.ocena1.ToString();
                        }
                        else
                        {
                            vnos.zeVpisanaOcena = "VP";
                        }
                    }
                    catch (Exception e)
                    {
                        vnos.zeVpisanaOcena = "/";
                        //Debug.WriteLine("Ocena za tega studenta in prijavo ni še vnesena..");
                    }

                    listVnosov.Add(vnos);
                }
            }

            if (listVnosov.Any())
            {
                //uredi seznam študentov
                listVnosov = listVnosov.OrderBy(o => o.priimek).ToList();

                int zaporednaSt = 0;
                foreach (var item in listVnosov)
                {
                    zaporednaSt = zaporednaSt + 1;
                    item.zaporednaStevilka = zaporednaSt;
                }
            }
            else
                listVnosov = null;

            return View(listVnosov);
        }


        [HttpPost]
        public ActionResult VpisOcen(IList<studis.Models.VnosTockModel> list)
        {
            UserHelper uHelper = new UserHelper();
            if (list.Any())
            {
                foreach (VnosTockModel m in list)
                {
                    //podatki o izpitnem roku
                    if (m.zaporednaStevilka == 1)
                    {
                        izpitnirok rok = db.izpitniroks.Where(r => r.id == m.idRoka).SingleOrDefault();

                        sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
                        izvajanje izv = db.izvajanjes.Where(i => i.id == rok.izvajanjeId).SingleOrDefault();

                        string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
                        if (izv.izvajalec2Id != null)
                            izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
                        if (izv.izvajalec3Id != null)
                            izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

                        ViewBag.idRoka = rok.id;
                        ViewBag.izvajalci = izvajalci;
                        ViewBag.prostor = predavalnica.naziv;
                        ViewBag.datum = GetDatumForIzpitniRok(rok.id);
                        ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
                        ViewBag.sifraPredmeta = izv.predmetId;
                        ViewBag.imePredmeta = izv.predmet.ime;
                    }

                    //vnos točk
                    if (ModelState.IsValid)
                    {
                        //vpisi tocke, če so bile vnese v view-u
                        if (m.ocena != null)
                        {
                            //VP=-1 tock(0 v bazi), drugače convert u int
                            int tempOcena = -1;
                            string stringOcena = m.ocena.ToLower();
                            if (!stringOcena.Equals("vp"))
                            {
                                tempOcena = Convert.ToInt32(m.ocena);
                            }

                            vpi vpis = db.vpis.Where(v => v.vpisnaStevilka == m.vpisnaStevilka && v.sifrant_studijskoleto.naziv == m.studijskoLeto).FirstOrDefault();

                            prijavanaizpit prijava = db.prijavanaizpits.Where(p => p.izpitnirokId == m.idRoka && p.vpisId == vpis.id).FirstOrDefault();
                            ocena ocena = null;
                            tocke tocke = null;
                            try
                            {
                                //preveri če že obstaja vnos?
                                ocena = db.ocenas.Where(t => t.prijavaId == prijava.id).FirstOrDefault();
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("Ni vnosa v bazi..");
                            }

                            //posodobi vnos
                            if (ocena != null)
                            {
                                try
                                {
                                    if (tempOcena == -1)
                                    {
                                        ocena.ocena1 = 0;
                                        prijava.stanje = 4; //VP??
                                        prijava.datumOdjave = DateTime.Now;
                                        prijava.odjavilId = uHelper.FindByName(User.Identity.Name).id;
                                        
                                        //v primeru VP točke na 0
                                        tocke = db.tockes.Where(t => t.prijavaId == prijava.id).FirstOrDefault();
                                        if (tocke != null)
                                        {
                                            tocke.tocke1 = 0;
                                            db.Entry(tocke).State = EntityState.Modified;
                                        }

                                        //sam za izpiz v viewu
                                        m.zeVpisanaOcena = "VP";
                                    }
                                    else
                                    {
                                        ocena.ocena1 = tempOcena;
                                        prijava.stanje = 2;

                                        //sam za izpiz v viewu
                                        m.zeVpisanaOcena = tempOcena.ToString();
                                    }
                                    ocena.prijavaId = prijava.id;
                                    ocena.datum = DateTime.Now;
                                    db.Entry(ocena).State = EntityState.Modified; //popravi oceno v bazi
                                    db.Entry(prijava).State = EntityState.Modified; //nastavi stanje prijave v bazi                                    
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine("Couldn't save changes to DB ocena!");
                                }
                            }
                            //nov vnos
                            else
                            {
                                try
                                {
                                    ocena = new ocena();
                                    if (tempOcena == -1)
                                    {
                                        ocena.ocena1 = 0;
                                        prijava.stanje = 4; //VP??
                                        prijava.datumOdjave = DateTime.Now;
                                        prijava.odjavilId = uHelper.FindByName(User.Identity.Name).id;

                                        //v primeru VP točke na 0
                                        tocke = db.tockes.Where(t => t.prijavaId == prijava.id).FirstOrDefault();
                                        if (tocke != null)
                                        {
                                            tocke.tocke1 = 0;
                                            db.Entry(tocke).State = EntityState.Modified;
                                        }

                                        //sam za izpiz v viewu
                                        m.zeVpisanaOcena = "VP";
                                    }
                                    else
                                    {
                                        ocena.ocena1 = tempOcena;
                                        prijava.stanje = 2;

                                        //sam za izpiz v viewu
                                        m.zeVpisanaOcena = tempOcena.ToString();
                                    }
                                    ocena.prijavaId = prijava.id;
                                    ocena.datum = DateTime.Now;
                                    db.ocenas.Add(ocena); //vpiši oceno v bazo
                                    db.Entry(prijava).State = EntityState.Modified; //nastavi stanje prijave v bazi
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine("Couldn't make new entry&save changes to DB tocke!");
                                }
                            }
                        }
                    }
                }
            }
            return View(list);
        }

        public ActionResult IndiVpisOcen(int rokID, int prijavaID)
        {
            //podatki o izpitnem roku
            izpitnirok rok = db.izpitniroks.Where(r => r.id == rokID).SingleOrDefault();

            sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
            izvajanje izv = db.izvajanjes.Where(i => i.id == rok.izvajanjeId).SingleOrDefault();

            string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
            if (izv.izvajalec2Id != null)
                izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
            if (izv.izvajalec3Id != null)
                izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

            ViewBag.idRoka = rok.id;
            ViewBag.izvajalci = izvajalci;
            ViewBag.prostor = predavalnica.naziv;
            ViewBag.datum = GetDatumForIzpitniRok(rok.id);
            ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
            ViewBag.sifraPredmeta = izv.predmetId;
            ViewBag.imePredmeta = izv.predmet.ime;


            //pridobi prijavljene študente
            List<VnosTockModel> listVnosov = new List<VnosTockModel>();
            StudentHelper sh = new StudentHelper();

            var prijave = db.prijavanaizpits.Where(p => p.id == prijavaID).ToList();

            foreach (prijavanaizpit prijava in prijave)
            {
                vpi vpiss = db.vpis.Where(v => v.id == prijava.vpisId).SingleOrDefault();
                student st = db.students.Where(s => s.vpisnaStevilka == vpiss.vpisnaStevilka).SingleOrDefault();
                VnosTockModel vnos = new VnosTockModel();

                if (st != null)
                {
                    vnos.idRoka = rokID;
                    vnos.vpisnaStevilka = st.vpisnaStevilka;
                    vnos.ime = st.ime;
                    vnos.priimek = st.priimek;
                    vnos.studijskoLeto = vpiss.sifrant_studijskoleto.naziv;
                    vnos.zaporednoSteviloPonavljanja = sh.zaporednoPolaganje(st.vpisnaStevilka, (int)izv.id, vpiss.studijskiProgram, prijava.izpitnirok.datum);

                    try
                    {
                        //preveri če že obstaja vnos?
                        ocena ocena = db.ocenas.Where(t => t.prijavaId == prijava.id).FirstOrDefault();

                        if (prijava.stanje != 4)
                        {
                            vnos.zeVpisanaOcena = ocena.ocena1.ToString();
                        }
                        else
                        {
                            vnos.zeVpisanaOcena = "VP";
                        }
                    }
                    catch (Exception e)
                    {
                        vnos.zeVpisanaOcena = "/";
                        //Debug.WriteLine("Ocena za tega studenta in prijavo ni še vnesena..");
                    }

                    listVnosov.Add(vnos);
                }
            }

            if (listVnosov.Any())
            {
                //uredi seznam študentov
                listVnosov = listVnosov.OrderBy(o => o.priimek).ToList();

                int zaporednaSt = 0;
                foreach (var item in listVnosov)
                {
                    zaporednaSt = zaporednaSt + 1;
                    item.zaporednaStevilka = zaporednaSt;
                }
            }
            else
                listVnosov = null;

            return View(listVnosov);
        }


        [HttpPost]
        public ActionResult IndiVpisOcen(IList<studis.Models.VnosTockModel> list)
        {
            UserHelper uHelper = new UserHelper();
            if (list.Any())
            {
                foreach (VnosTockModel m in list)
                {
                    //podatki o izpitnem roku
                    if (m.zaporednaStevilka == 1)
                    {
                        izpitnirok rok = db.izpitniroks.Where(r => r.id == m.idRoka).SingleOrDefault();

                        sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
                        izvajanje izv = db.izvajanjes.Where(i => i.id == rok.izvajanjeId).SingleOrDefault();

                        string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
                        if (izv.izvajalec2Id != null)
                            izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
                        if (izv.izvajalec3Id != null)
                            izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

                        ViewBag.idRoka = rok.id;
                        ViewBag.izvajalci = izvajalci;
                        ViewBag.prostor = predavalnica.naziv;
                        ViewBag.datum = GetDatumForIzpitniRok(rok.id);
                        ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
                        ViewBag.sifraPredmeta = izv.predmetId;
                        ViewBag.imePredmeta = izv.predmet.ime;
                    }

                    //vnos točk
                    if (ModelState.IsValid)
                    {
                        //vpisi tocke, če so bile vnese v view-u
                        if (m.ocena != null)
                        {
                            //VP=-1 tock(0 v bazi), drugače convert u int
                            int tempOcena = -1;
                            string stringOcena = m.ocena.ToLower();
                            if (!stringOcena.Equals("vp"))
                            {
                                tempOcena = Convert.ToInt32(m.ocena);
                            }

                            vpi vpis = db.vpis.Where(v => v.vpisnaStevilka == m.vpisnaStevilka && v.sifrant_studijskoleto.naziv == m.studijskoLeto).FirstOrDefault();

                            prijavanaizpit prijava = db.prijavanaizpits.Where(p => p.izpitnirokId == m.idRoka && p.vpisId == vpis.id).FirstOrDefault();
                            ocena ocena = null;
                            tocke tocke = null;
                            try
                            {
                                //preveri če že obstaja vnos?
                                ocena = db.ocenas.Where(t => t.prijavaId == prijava.id).FirstOrDefault();
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("Ni vnosa v bazi..");
                            }

                            //posodobi vnos
                            if (ocena != null)
                            {
                                try
                                {
                                    if (tempOcena == -1)
                                    {
                                        ocena.ocena1 = 0;
                                        prijava.stanje = 4; //VP??
                                        prijava.datumOdjave = DateTime.Now;
                                        prijava.odjavilId = uHelper.FindByName(User.Identity.Name).id;

                                        //v primeru VP točke na 0
                                        tocke = db.tockes.Where(t => t.prijavaId == prijava.id).FirstOrDefault();
                                        if (tocke != null)
                                        {
                                            tocke.tocke1 = 0;
                                            db.Entry(tocke).State = EntityState.Modified;
                                        }

                                        //sam za izpiz v viewu
                                        m.zeVpisanaOcena = "VP";
                                    }
                                    else
                                    {
                                        ocena.ocena1 = tempOcena;
                                        prijava.stanje = 2;

                                        //sam za izpiz v viewu
                                        m.zeVpisanaOcena = tempOcena.ToString();
                                    }
                                    ocena.prijavaId = prijava.id;
                                    ocena.datum = DateTime.Now;
                                    db.Entry(ocena).State = EntityState.Modified; //popravi oceno v bazi
                                    db.Entry(prijava).State = EntityState.Modified; //nastavi stanje prijave v bazi                                    
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine("Couldn't save changes to DB ocena!");
                                }
                            }
                            //nov vnos
                            else
                            {
                                try
                                {
                                    ocena = new ocena();
                                    if (tempOcena == -1)
                                    {
                                        ocena.ocena1 = 0;
                                        prijava.stanje = 4; //VP??
                                        prijava.datumOdjave = DateTime.Now;
                                        prijava.odjavilId = uHelper.FindByName(User.Identity.Name).id;

                                        //v primeru VP točke na 0
                                        tocke = db.tockes.Where(t => t.prijavaId == prijava.id).FirstOrDefault();
                                        if (tocke != null)
                                        {
                                            tocke.tocke1 = 0;
                                            db.Entry(tocke).State = EntityState.Modified;
                                        }

                                        //sam za izpiz v viewu
                                        m.zeVpisanaOcena = "VP";
                                    }
                                    else
                                    {
                                        ocena.ocena1 = tempOcena;
                                        prijava.stanje = 2;

                                        //sam za izpiz v viewu
                                        m.zeVpisanaOcena = tempOcena.ToString();
                                    }
                                    ocena.prijavaId = prijava.id;
                                    ocena.datum = DateTime.Now;
                                    db.ocenas.Add(ocena); //vpiši oceno v bazo
                                    db.Entry(prijava).State = EntityState.Modified; //nastavi stanje prijave v bazi
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Debug.WriteLine("Couldn't make new entry&save changes to DB tocke!");
                                }
                            }
                        }
                    }
                }
            }
            return View(list);
        }

        [Authorize(Roles = "Referent, Profesor")]
        public ActionResult VnosOcenBrezPrijave(int? izvajanjeId, int? vpisId)
        {
            StudentHelper sh = new StudentHelper();
            vpi v = db.vpis.Find(vpisId);

            int leto = sh.trenutnoSolskoLeto();
            var roki = db.izpitniroks.Where(a => a.izvajanjeId == izvajanjeId)
                                     .Where(b => b.datum <= DateTime.Now || b.fiktiven == true)
                                     .Where(c => (c.datum.Year == leto && c.datum.Month > 9) || (c.datum.Year == leto+1 && c.datum.Month <= 9));
            List<SelectListItem> seznam = new List<SelectListItem>();
            foreach (var i in roki)
            {
                string text = "";
                int ocena = sh.ocenaRoka(v.id, i.id);
                string ocena_s = "";
                if (ocena == -1) ocena_s = "/";
                else ocena_s = ocena.ToString();

                if (i.fiktiven) text="Brez prijave (ocena: "+ocena_s+")";
                else text = i.datum.ToString("dd.MM.yyyy") + " (razpisan, ocena: " + ocena_s + ")";

                seznam.Add(new SelectListItem() { Value = i.id.ToString(), Text = (text) });
            }
            

            ViewBag.roki = new SelectList(seznam, "Value", "text");
            ViewBag.vpisnast = v.vpisnaStevilka;

            return View();
        }

        //Stanje 0: študent je prijavljen na izpit
        //Stanje 1: študent se je odjavil od izpita
        //Stanje 2: študent je pisal izpit
        //Stanje 3: študent ni pisal izpita
        //Stanje 4: vrnjena prijava
        [Authorize(Roles = "Referent, Profesor")]
        [HttpPost]
        public ActionResult VnosOcenBrezPrijave(int? izvajanjeId, int? vpisId, KoncnaOcenaModel model)
        {
            //že preverjeno z ajaxom če je vse OK glede števila polaganj itd
            UserHelper uh = new UserHelper();
            StudentHelper sh = new StudentHelper();
            vpi v = db.vpis.Find(vpisId);
            if (ModelState.IsValid)
            {
                //ce ze obstaja prijava za ta rok jo ne kreiramo
                var prijava = db.prijavanaizpits.Where(a => a.vpisId == vpisId).Where(b => b.izpitnirokId == model.izpitnirok).FirstOrDefault();

                //pogledamo ce ze obstaja pozitivna ocena za kak prejšnji rok
                //dovolimo spremembo ce spreminjamo ravno rok s pozitivno oceno
                System.Diagnostics.Debug.WriteLine("Rok id je " + model.izpitnirok.ToString());

                if (sh.pozitivnaOcena(v.vpisnaStevilka, (int)izvajanjeId) && (model.izpitnirok != sh.pozitivnaOcenaRokId(v.vpisnaStevilka, (int)izvajanjeId)))
                {
                    ModelState.AddModelError("", "Študent že ima pozitivno oceno pri tem izvajanju!");
                }
                else
                {
                    if (prijava == null)
                    {
                        //ustvari prijavo
                        prijavanaizpit pni = new prijavanaizpit();
                        pni.datumPrijave = DateTime.Now;
                        pni.izpitnirokId = model.izpitnirok;
                        pni.prijavilId = uh.FindByName(User.Identity.Name).id;
                        pni.stanje = 2;
                        pni.vpisId = (int)vpisId;
                        db.prijavanaizpits.Add(pni);
                        db.SaveChanges();

                        //ustvari oceno
                        ocena o = new ocena();
                        o.datum = DateTime.Now;
                        o.ocena1 = model.ocena;
                        o.prijavaId = pni.id;
                        db.ocenas.Add(o);
                        db.SaveChanges();
                    }
                    else
                    {
                        prijava.stanje = 2;

                        //ce ze obstaja ocena za to prijavo jo ne kreiramo
                        var ocena = db.ocenas.Where(a => a.prijavaId == prijava.id).FirstOrDefault();

                        if (ocena == null)
                        {
                            //ustvari oceno
                            ocena o = new ocena();
                            o.datum = DateTime.Now;
                            o.ocena1 = model.ocena;
                            o.prijavaId = prijava.id;
                            db.ocenas.Add(o);
                        }
                        else
                        {
                            ocena.datum = DateTime.Now;
                            ocena.ocena1 = model.ocena;
                        }
                        db.SaveChanges();
                    }

                    return RedirectToAction("Izvajanja", new { vpisna = v.vpisnaStevilka });
                }
            }

            int leto = sh.trenutnoSolskoLeto();
            var roki = db.izpitniroks.Where(a => a.izvajanjeId == izvajanjeId)
                                     .Where(b => b.datum <= DateTime.Now || b.fiktiven == true)
                                     .Where(c => (c.datum.Year == leto && c.datum.Month > 9) || (c.datum.Year == leto + 1 && c.datum.Month <= 9));
            List<SelectListItem> seznam = new List<SelectListItem>();
            foreach (var i in roki)
            {
                string text = "";
                int ocena = sh.ocenaRoka(v.id, i.id);
                string ocena_s = "";
                if (ocena == -1) ocena_s = "/";
                else ocena_s = ocena.ToString();

                if (i.fiktiven) text = "Brez prijave (ocena: " + ocena_s + ")";
                else text = i.datum.ToString("dd.MM.yyyy") + " (razpisan, ocena: " + ocena_s + ")";

                seznam.Add(new SelectListItem() { Value = i.id.ToString(), Text = (text) });
            }


            ViewBag.roki = new SelectList(seznam, "Value", "text");
            ViewBag.vpisnast = v.vpisnaStevilka;

            return View(model);

        }


        public ActionResult Izvajanja(int vpisna)
        {
            var vpisi = db.vpis.Where(v => v.vpisnaStevilka == vpisna).ToList();

            List<izvajanje> izvajanja = new List<izvajanje>();


            if (User.IsInRole("Referent"))
            {
                foreach (var v in vpisi)
                {
                    foreach (var i in v.izvajanjes)
                    {
                        izvajanja.Add(i);
                    }
                }
            }
            else if (User.IsInRole("Profesor"))
            {
                UserHelper uh = new UserHelper();
                var profesor = uh.FindByName(User.Identity.Name).profesors.FirstOrDefault();

                foreach(var v in vpisi)
                {
                    foreach (var i in v.izvajanjes)
                    {
                        if (i.profesor.id == profesor.id)
                        {
                            izvajanja.Add(i);
                        }
                        else if (i.profesor1 != null && i.profesor1.id == profesor.id)
                        {
                            izvajanja.Add(i);
                        }
                        else if (i.profesor2 != null && i.profesor2.id == profesor.id)
                        {
                            izvajanja.Add(i);
                        }
                    }
                }

            }
            
            ViewBag.Izvajanja = izvajanja;
            ViewBag.Vpis = vpisi.First();

            return View();
        }


        public ActionResult IzpisTock(int rokID = -1, int seznam = -1)
        {
            if (rokID == -1 || seznam == -1)
                return RedirectToAction("Seznam");


            //podatki o izpitnem roku
            izpitnirok rok = db.izpitniroks.Where(r => r.id == rokID).SingleOrDefault();

            sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
            izvajanje izv = db.izvajanjes.Where(i => i.id == rok.izvajanjeId).SingleOrDefault();

            string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
            if (izv.izvajalec2Id != null)
                izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
            if (izv.izvajalec3Id != null)
                izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

            ViewBag.idRoka = rok.id;
            ViewBag.izvajalci = izvajalci;
            ViewBag.prostor = predavalnica.naziv;
            ViewBag.datum = GetDatumForIzpitniRok(rok.id);
            ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
            ViewBag.sifraPredmeta = izv.predmetId;
            ViewBag.imePredmeta = izv.predmet.ime;


            //pridobi prijavljene študente
            List<VnosTockModel> listVnosov = new List<VnosTockModel>();
            StudentHelper sh = new StudentHelper();

            var prijave = db.prijavanaizpits.Where(p => p.izpitnirokId == rok.id).ToList();

            foreach (prijavanaizpit prijava in prijave)
            {
                vpi vpiss = db.vpis.Where(v => v.id == prijava.vpisId).SingleOrDefault();
                student st = db.students.Where(s => s.vpisnaStevilka == vpiss.vpisnaStevilka).SingleOrDefault();
                VnosTockModel vnos = new VnosTockModel();

                if (st != null)
                {
                    vnos.idRoka = rokID;
                    vnos.vpisnaStevilka = st.vpisnaStevilka;
                    vnos.ime = st.ime;
                    vnos.priimek = st.priimek;
                    vnos.studijskoLeto = vpiss.sifrant_studijskoleto.naziv;
                    vnos.zaporednoSteviloPonavljanja = sh.zaporednoPolaganje(st.vpisnaStevilka, (int)izv.id, vpiss.studijskiProgram, prijava.izpitnirok.datum);

                    try
                    {
                        //preveri če že obstaja vnos?
                        tocke tocke = db.tockes.Where(t => t.prijavaId == prijava.id).FirstOrDefault();

                        if (prijava.stanje != 4)
                        {
                            vnos.zeVpisaneTocke = tocke.tocke1.ToString();
                        }
                        else
                        {
                            vnos.zeVpisaneTocke = "VP";
                        }
                    }
                    catch (Exception e)
                    {
                        vnos.zeVpisaneTocke = "/";
                        //Debug.WriteLine("Tocke za tega studenta in prijavo niso še vnesene..");
                    }

                    listVnosov.Add(vnos);
                }
            }

            if (listVnosov.Any())
            {
                //uredi seznam študentov
                listVnosov = listVnosov.OrderBy(o => o.priimek).ToList();

                int zaporednaSt = 0;
                foreach (var item in listVnosov)
                {
                    zaporednaSt = zaporednaSt + 1;
                    item.zaporednaStevilka = zaporednaSt;
                }
            }
            else
                listVnosov = null;

            if(seznam==0)
                return View(listVnosov);
            else
                return View("IzpisTockAnonymous",listVnosov);
        }


        public ActionResult IzpisOcen(int rokID = -1, int seznam = -1)
        {
            if (rokID == -1 || seznam == -1)
                return RedirectToAction("Seznam");


            //podatki o izpitnem roku
            izpitnirok rok = db.izpitniroks.Where(r => r.id == rokID).SingleOrDefault();

            sifrant_prostor predavalnica = db.sifrant_prostor.Where(s => s.id == rok.prostorId).SingleOrDefault();
            izvajanje izv = db.izvajanjes.Where(i => i.id == rok.izvajanjeId).SingleOrDefault();

            string izvajalci = izv.profesor.priimek + " " + izv.profesor.ime;
            if (izv.izvajalec2Id != null)
                izvajalci = izvajalci + ", " + izv.profesor1.priimek + " " + izv.profesor1.ime;
            if (izv.izvajalec3Id != null)
                izvajalci = izvajalci + ", " + izv.profesor2.priimek + " " + izv.profesor2.ime;

            ViewBag.idRoka = rok.id;
            ViewBag.izvajalci = izvajalci;
            ViewBag.prostor = predavalnica.naziv;
            ViewBag.datum = GetDatumForIzpitniRok(rok.id);
            ViewBag.ura = UserHelper.TimeToString((DateTime)rok.ura);
            ViewBag.sifraPredmeta = izv.predmetId;
            ViewBag.imePredmeta = izv.predmet.ime;


            //pridobi prijavljene študente
            List<VnosTockModel> listVnosov = new List<VnosTockModel>();
            StudentHelper sh = new StudentHelper();

            var prijave = db.prijavanaizpits.Where(p => p.izpitnirokId == rok.id).ToList();

            foreach (prijavanaizpit prijava in prijave)
            {
                vpi vpiss = db.vpis.Where(v => v.id == prijava.vpisId).SingleOrDefault();
                student st = db.students.Where(s => s.vpisnaStevilka == vpiss.vpisnaStevilka).SingleOrDefault();
                VnosTockModel vnos = new VnosTockModel();

                if (st != null)
                {
                    vnos.idRoka = rokID;
                    vnos.vpisnaStevilka = st.vpisnaStevilka;
                    vnos.ime = st.ime;
                    vnos.priimek = st.priimek;
                    vnos.studijskoLeto = vpiss.sifrant_studijskoleto.naziv;
                    vnos.zaporednoSteviloPonavljanja = sh.zaporednoPolaganje(st.vpisnaStevilka, (int)izv.id, vpiss.studijskiProgram, prijava.izpitnirok.datum);

                    try
                    {
                        //preveri če že obstaja vnos?
                        tocke tocke = db.tockes.Where(t => t.prijavaId == prijava.id).FirstOrDefault();

                        if (prijava.stanje != 4)
                        {
                            vnos.zeVpisaneTocke = tocke.tocke1.ToString();
                        }
                        else
                        {
                            vnos.zeVpisaneTocke = "VP";
                        }
                    }
                    catch (Exception e)
                    {
                        vnos.zeVpisaneTocke = "/";
                        //Debug.WriteLine("Tocke za tega studenta in prijavo niso še vnesene..");
                    }

                    try
                    {
                        //preveri če že obstaja vnos?
                        ocena ocena = db.ocenas.Where(o => o.prijavaId == prijava.id).FirstOrDefault();

                        if (prijava.stanje != 4)
                        {
                            vnos.zeVpisanaOcena = ocena.ocena1.ToString();
                        }
                        else
                        {
                            vnos.zeVpisanaOcena = "VP";
                        }
                    }
                    catch (Exception e)
                    {
                        vnos.zeVpisanaOcena = "/";
                        //Debug.WriteLine("Tocke za tega studenta in prijavo niso še vnesene..");
                    }

                    listVnosov.Add(vnos);
                }
            }

            if (listVnosov.Any())
            {
                //uredi seznam študentov
                listVnosov = listVnosov.OrderBy(o => o.priimek).ToList();

                int zaporednaSt = 0;
                foreach (var item in listVnosov)
                {
                    zaporednaSt = zaporednaSt + 1;
                    item.zaporednaStevilka = zaporednaSt;
                }
            }
            else
                listVnosov = null;

            if (seznam == 0)
                return View(listVnosov);
            else
                return View("IzpisOcenAnonymous", listVnosov);
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
            var izpitniRoki = izvajanje.izpitniroks.Where(a => a.fiktiven == false);//pPredmet.izpitniroks.ToList(); //Exception 
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
            var datum = db.izpitniroks.SingleOrDefault(r => r.id == id).datum;
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
                var prijave = db.izpitniroks.SingleOrDefault(r => r.id == id).prijavanaizpits;
                foreach (var p in prijave)
                    steviloOcen += p.ocenas.Count();
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
                st = db.izpitniroks.SingleOrDefault(r => r.id == id).prijavanaizpits.Count();
            }
            catch (Exception e) { st = -1; }
            return st.ToString();
        }

        //dobi izvajanja za predmet brez opcije "Izberi"
        public string GetIzvajanjaOnlyForPredmet(int id)
        {
            /*
             * TO DO TO DO TO DO TO DO TO DO TO DO
             */
            //Debug.WriteLine("ID " + id);
            int iid = Convert.ToInt32(id);
            //Debug.WriteLine("ID " + iid);
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
            return new JavaScriptSerializer().Serialize(seznamIzvajanja);
        }

        //Izpitni roki brez opcije "Izberi"
        public string GetIzpitniRoksOnlyForIzvajanja(int id)
        {

            //Debug.WriteLine("ID " + id);
            int iid = Convert.ToInt32(id);
            //Debug.WriteLine("ID " + iid);
            var izvajanje = db.izvajanjes.SingleOrDefault(i => i.id == iid);//db.predmets.SingleOrDefault(p => p.id == iid);
            var izpitniRoki = izvajanje.izpitniroks.Where(a => a.fiktiven == false);//pPredmet.izpitniroks.ToList(); //Exception 
            var seznamIzpitniRoki = new List<SelectListItem>();
            int c = 0;
            foreach (izpitnirok i in izpitniRoki)
            {
                c++;
                string prostor = "";
                if (i.sifrant_prostor != null)
                {
                    prostor = i.sifrant_prostor.naziv;
                }
                string ura = "";
                if (i.ura != null)
                {
                    ura = UserHelper.TimeToString((DateTime)i.ura);
                }
                seznamIzpitniRoki.Add(new SelectListItem() { Value = i.id.ToString(), Text = UserHelper.DateToString(i.datum) + " " + ura + " " + prostor });
            }
            if (c < 1)
            {
                seznamIzpitniRoki.Add(new SelectListItem() { Value = "", Text = "Ta predmet nima razpisanih rokov." });
            }

            return new JavaScriptSerializer().Serialize(seznamIzpitniRoki);
        }

        public string GetIzpitniRoksOnlyForIzvajanjaPlusFiktivni(int id)
        {

            //Debug.WriteLine("ID " + id);
            int iid = Convert.ToInt32(id);
            //Debug.WriteLine("ID " + iid);
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
                    prostor = i.sifrant_prostor.naziv;
                }
                string ura = "";
                if (i.ura != null)
                {
                    ura = UserHelper.TimeToString((DateTime)i.ura);
                }
                seznamIzpitniRoki.Add(new SelectListItem() { Value = i.id.ToString(), Text = UserHelper.DateToString(i.datum) + " " + ura + " " + prostor });
            }
            if (c < 1)
            {
                seznamIzpitniRoki.Add(new SelectListItem() { Value = "", Text = "Ta predmet nima razpisanih rokov." });
            }

            return new JavaScriptSerializer().Serialize(seznamIzpitniRoki);
        }
    }
}
