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
        public ActionResult Seznam(int id)
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

            List<student> list = new List<student>();
            List<string> leta = new List<string>();
            List<int> vsaPolaganja = new List<int>();
            StudentHelper sh = new StudentHelper();
            foreach(prijavanaizpit prijava in prijave)
            {
                vpi vpiss = db.vpis.Where(v => v.id == prijava.vpisId).SingleOrDefault();
                student st = db.students.Where(s => s.vpisnaStevilka == vpiss.vpisnaStevilka).SingleOrDefault();
               
                if (st != null)
                {
                    list.Add(st);
                }
            }

            if (list.Any())
            {
                //uredi seznam študentov
                list = list.OrderBy(o => o.priimek).ToList();

                //naredi seznam let poslušanja predmeta v enakem vrstnem redu kot so študenti za izpis
                for(int i=0; i < list.Count; i++)
                {
                    foreach (vpi vpisan in list[i].vpis)
                    {
                        foreach (prijavanaizpit prijava in prijave)
                        {
                            if (vpisan.id == prijava.vpisId)
                            {
                                leta.Add(vpisan.sifrant_studijskoleto.naziv);
                                int polaganja = sh.zaporednoPolaganje(list[i].vpisnaStevilka, (int)izv.id, vpisan.studijskiProgram, prijava.izpitnirok.datum);
                                vsaPolaganja.Add(polaganja);
                            }
                        }
                    }
                }
                ViewBag.years = leta;
                ViewBag.vsaPolaganja = vsaPolaganja;
            }
            else
                list = null;

            return View(list);
        }


        public ActionResult VpisTock(int rokID)
        {
            ViewBag.idRoka = rokID; //parameter za klic view z izpisom

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


        public ActionResult VpisOcen(int rokID)
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


        public ActionResult IzpisTock(int rokID, int seznam)
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


        public ActionResult IzpisOcen(int rokID, int seznam)
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
    }
}
