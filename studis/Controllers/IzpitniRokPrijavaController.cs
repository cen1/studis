using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace studis.Controllers
{
    [Authorize(Roles = "Študent, Referent")]
    public class IzpitniRokPrijavaController : Controller
    {
        public studisEntities db = new studisEntities();
        // GET: IzpitniRokPrijava
        public ActionResult Index()
        {
            StudentHelper sh = new StudentHelper();
            UserHelper uh = new UserHelper();
            my_aspnet_users u = uh.FindByName(User.Identity.Name);
            student s = db.students.Where(a => a.userId == u.id).FirstOrDefault();
            if (s != null)
            {
                var vpis = sh.trenutniVpis(s.vpisnaStevilka);
                /*if (vpis == null)
                    return RedirectToAction("VpisNiPotrjen", "Home");
                else if (vpis.potrjen == false)
                    return RedirectToAction("VpisNiPotrjen", "Home");*/
            }
            return View();
        }




        // GET: IzpitniRokPrijava/Prijavi
       public ActionResult Prijavi()
        {
            ViewBag.Izvajanja = null;
            ViewBag.StudentIme = null;
            ViewBag.StudentVpisna = null;
            ViewBag.Izvajanja = null;
            ViewBag.IzvajanjaZaVpisna = null;
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izberi" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            
            if (User.IsInRole("Študent"))
            {
                student stud = UserHelper.GetStudentByUserName(User.Identity.Name);
                ViewBag.Izvajanja = new SelectList(GetIzvajanaForStudentPrijava(), "Value", "Text");
                ViewBag.StudentIme = stud.ime + " " + stud.priimek;
                ViewBag.StudentVpisna = stud.vpisnaStevilka.ToString();
            }
            else
            {
                ViewBag.Izvajanja = new SelectList(ltemp, "Value", "Text");
            }
            List<student> temp = db.students.OrderBy(a => a.priimek).ToList();

            List<SelectListItem> studenti = new List<SelectListItem>();
            foreach (student i in temp)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.vpisnaStevilka.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString() + " - " + i.ime + " " + i.priimek;
                studenti.Add(p);
            }
            ViewBag.Studenti = new SelectList(studenti, "Value", "Text");
            return View();
        }

        // GET: IzpitniRokPrijava/PrijaviStudenta/vpisna
        [Authorize(Roles = "Referent")]
        public ActionResult PrijaviStudenta(int vpisna)
        {
            Debug.WriteLine("Vpisna: " + vpisna);
            ViewBag.Izvajanja = null;
            ViewBag.StudentIme = null;
            ViewBag.StudentVpisna = null;
            ViewBag.Izvajanja = null;
            ViewBag.IzvajanjaZaVpisna = null;
            student stud = UserHelper.GetStudentByVpisna(vpisna);
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izberi" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            if (User.IsInRole("Študent"))
            {
                ViewBag.Izvajanja = GetIzvajanaForStudentPrijava();
                ViewBag.StudentIme = stud.ime + " " + stud.priimek;
                ViewBag.StudentVpisna = stud.vpisnaStevilka.ToString();
            }
            else if(User.IsInRole("Referent"))
            {
                ViewBag.StudentIme = stud.ime + " " + stud.priimek;
                ViewBag.StudentVpisna = stud.vpisnaStevilka.ToString();
                ViewBag.IzvajanjaZaVpisna = new SelectList(GetIzvajanaForStudentVpisnaPrijava(vpisna), "Value", "Text");
            }
            return View("Prijavi");
        }

        // POST: IzpitniRokPrijava/Prijavi
        [HttpPost]
        public ActionResult Prijavi(PrijavaNaIzpitModel model)
        {
            StudentHelper sh = new StudentHelper();
            prijavanaizpit prijava = new prijavanaizpit();
            prijava.datumPrijave = DateTime.Now;
            prijava.izpitnirok = db.izpitniroks.FirstOrDefault(a => a.id == model.izpitniRok);
            prijava.stanje = 0;
            if (User.IsInRole("Študent"))
            {
               prijava.vpisId = sh.trenutniVpis(UserHelper.GetStudentByUserName(User.Identity.Name).vpisnaStevilka).id;
            }
            else //referent
            {

                prijava.vpisId = sh.trenutniVpis(model.student).id;
            }
            UserHelper uh = new UserHelper();
            prijava.prijavilId = uh.FindByName(User.Identity.Name).id;
            try
            {
                db.prijavanaizpits.Add(prijava);
                db.SaveChanges();
                if (User.IsInRole("Referent"))
                {
                    TempData["success"] = "Uspešno prijavljen";
                    return RedirectToAction("PrijaviStudenta", new { vpisna = model.student });
                }
                else
                {
                    TempData["success"] = "Uspešno prijavljen";
                    return RedirectToAction("Prijavi");
                }
            }
            catch
            {
                return View("Neuspesno");
            }
        }

        [HttpPost]
        public ActionResult PrijaviStudenta(int vpisna, PrijavaNaIzpitModel model)
        {
            StudentHelper sh = new StudentHelper();
            prijavanaizpit prijava = new prijavanaizpit();
            prijava.datumPrijave = DateTime.Now;
            prijava.izpitnirok = db.izpitniroks.FirstOrDefault(a => a.id == model.izpitniRok);
            prijava.stanje = 0;
            if (User.IsInRole("Študent"))
            {
                prijava.vpisId = sh.trenutniVpis(UserHelper.GetStudentByUserName(User.Identity.Name).vpisnaStevilka).id;
            }
            else //referent
            {

                prijava.vpisId = sh.trenutniVpis(model.student).id;
            }
            UserHelper uh = new UserHelper();
            prijava.prijavilId = uh.FindByName(User.Identity.Name).id;
            try
            {
                db.prijavanaizpits.Add(prijava);
                db.SaveChanges();
                if (User.IsInRole("Referent"))
                {
                    TempData["success"] = "Uspešno prijavljen";
                    return RedirectToAction("PrijaviStudenta", new { vpisna = model.student });
                }
                else
                {
                    TempData["success"] = "Uspešno prijavljen";
                    return RedirectToAction("Prijavi");
                }
            }
            catch
            {
                return View("Neuspesno");
            }
        }



        // GET: IzpitniRokPrijava/Odjavi/
        public ActionResult Odjavi() //sem gre student ali referent, refernt dobi se dropdown
        {
            ViewBag.Izvajanja = null;
            ViewBag.StudentIme = null;
            ViewBag.StudentVpisna = null;
            ViewBag.Izvajanja = null;
            ViewBag.IzvajanjaZaVpisna = null;
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izberi" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");

            if (User.IsInRole("Študent"))
            {
                student stud = UserHelper.GetStudentByUserName(User.Identity.Name);
                ViewBag.Izvajanja = new SelectList(GetIzvajanaForStudentOdjava(), "Value", "Text");
                ViewBag.StudentIme = stud.ime + " " + stud.priimek;
                ViewBag.StudentVpisna = stud.vpisnaStevilka.ToString();
            }
            else
            {
                ViewBag.Izvajanja = new SelectList(ltemp, "Value", "Text");
            }
            List<student> temp = db.students.OrderBy(a => a.priimek).ToList();

            List<SelectListItem> studenti = new List<SelectListItem>();
            foreach (student i in temp)
            {
                SelectListItem p = new SelectListItem();
                p.Value = i.vpisnaStevilka.ToString();
                p.Text = Convert.ToInt32(p.Value).ToString() + " - " + i.ime + " " + i.priimek;
                studenti.Add(p);
            }
            ViewBag.Studenti = new SelectList(studenti, "Value", "Text");
            return View();
        }

        [Authorize(Roles = "Referent")]
        // GET: IzpitniRokPrijava/Odjavi/
        public ActionResult OdjaviDnevnik(int vpisna)
        {
            StudentHelper sh = new StudentHelper();
            vpi v = sh.trenutniVpis(vpisna);

            ViewBag.odjave = v.prijavanaizpits.Where(a => a.stanje == 1).OrderBy(a => a.datumOdjave);
            ViewBag.vpisna = vpisna;
            return View();

        }

        [Authorize(Roles = "Referent")]
        // GET: IzpitniRokPrijava/Odjavi/
        public ActionResult OdjaviStudenta(int vpisna)
        {
            ViewBag.Izvajanja = null;
            ViewBag.StudentIme = null;
            ViewBag.StudentVpisna = null;
            ViewBag.Izvajanja = null;
            ViewBag.IzvajanjaZaVpisna = null;
            student stud = UserHelper.GetStudentByVpisna(vpisna);
            List<SelectListItem> ltemp = new List<SelectListItem>();
            ltemp.Add(new SelectListItem() { Value = "", Text = "Izberi" });
            ViewBag.Prazen = new SelectList(ltemp, "Value", "Text");
            if (User.IsInRole("Študent"))
            {
                ViewBag.Izvajanja = GetIzvajanaForStudentOdjava();
            }
            else if (User.IsInRole("Referent"))
            {
                ViewBag.StudentIme = stud.ime + " " + stud.priimek;
                ViewBag.StudentVpisna = stud.vpisnaStevilka.ToString();
                ViewBag.IzvajanjaZaVpisna = new SelectList(GetIzvajanaForStudentVpisnaOdjava(vpisna), "Value", "Text");
            }
            return View("Odjavi");
        }

        [HttpPost]
        // GET: IzpitniRokPrijava/Odjavi/
        public ActionResult OdjaviStudenta(int vpisna, PrijavaNaIzpitModel model)
        {
            //model.izpitniRok;
            // model.izvajanje;
            student stud;
            if (User.IsInRole("Študent"))
            {
                stud = UserHelper.GetStudentByUserName(User.Identity.Name);
            }
            else //refernt
            {
                stud = UserHelper.GetStudentByVpisna(model.student);
            }
            UserHelper uh = new UserHelper();
            StudentHelper sh = new StudentHelper();

            try
            {
                vpi trenutniVpis = sh.trenutniVpis(stud.vpisnaStevilka);
                prijavanaizpit prijava = db.prijavanaizpits.SingleOrDefault(a => a.id == model.izpitniRok);
                prijava.stanje = 1;
                prijava.odjavilId = uh.FindByName(User.Identity.Name).id;
                prijava.datumOdjave = DateTime.Now;

                db.SaveChanges();
                if (User.IsInRole("Referent"))
                {
                    TempData["success"] = "Uspešno odjavljen";
                    return RedirectToAction("OdjaviStudenta", new { vpisna = model.student });
                }
                else
                {
                    TempData["success"] = "Uspešno odjavljen";
                    return RedirectToAction("Odjavi");
                }
            }
            catch
            {
                return View("Neuspesno");
            }
        }

        // POST: IzpitniRokPrijava/Odjavi
        [HttpPost]
        public ActionResult Odjavi(PrijavaNaIzpitModel model)
        {
            //model.izpitniRok;
            // model.izvajanje;
            int vpisna;
            student stud;
            if (User.IsInRole("Študent"))
            {
                stud = UserHelper.GetStudentByUserName(User.Identity.Name);
            }
            else //refernt
            {
                stud = UserHelper.GetStudentByVpisna(model.student);
            }
            UserHelper uh = new UserHelper();
            StudentHelper sh = new StudentHelper();
            
            try
            {
                vpi trenutniVpis = sh.trenutniVpis(stud.vpisnaStevilka);
                prijavanaizpit prijava = db.prijavanaizpits.SingleOrDefault(a => a.id == model.izpitniRok);
                prijava.stanje = 1;
                prijava.odjavilId = uh.FindByName(User.Identity.Name).id;
                prijava.datumOdjave = DateTime.Now;

                db.SaveChanges();
                if (User.IsInRole("Referent"))
                {
                    TempData["success"] = "Uspešno odjavljen";
                    return RedirectToAction("OdjaviStudenta", new { vpisna = model.student });
                }
                else
                {
                    TempData["success"] = "Uspešno odjavljen";
                    return RedirectToAction("Odjavi");
                }
            }
            catch
            {
                return View("Neuspesno");
            }
        }


        [Authorize(Roles = "Študent")]
        private List<SelectListItem> GetIzvajanaForStudentPrijava()
        {
            var student = UserHelper.GetStudentByUserName(User.Identity.Name);
            //var izvajanja = student.vpis.LastOrDefault().izvajanjes.ToList();
            StudentHelper sh = new StudentHelper();
            var trenutniVpis = sh.trenutniVpis(student.vpisnaStevilka);
            var prvivpis = trenutniVpis;
            Debug.WriteLine("Trenutni "+trenutniVpis.id.ToString());
            if (trenutniVpis.vrstaVpisa > 1)
            {
                Debug.WriteLine("Inside");
                prvivpis = sh.prviVpisVLetnik(trenutniVpis.id); //ce je ponavljanje ali pavziranje damo na prvi vpis v ta letnik
                Debug.WriteLine(prvivpis.id.ToString());
            }
            List<izvajanje> izvajanja = null;
            if (prvivpis != null)
            {
                Debug.WriteLine("Prvi Vpis id je " + prvivpis.id.ToString());
                izvajanja = prvivpis.izvajanjes.ToList();
            }
            else
            {
                Debug.WriteLine("Trenutni vpis je null.");
                izvajanja = new List<izvajanje>();
            }
            //ce je ze prijavljen na kak izpit odstrani
            //prav tako ce ze ima pozitivno/zakljuceno oceno
            //uporabimo trenutni vpis ekr se na tega veze prijava
            List<izvajanje> izvajanjafinal = new List<izvajanje>();
            foreach (var i in izvajanja)
            {
                var prijave0 = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                .Where(a => a.izpitnirok.izvajanjeId == i.id)
                                                .Where(a => a.stanje == 0)
                                                .Where(a => a.izpitnirok.fiktiven == false)
                                                .ToList();
                bool ocena = sh.pozitivnaOcena(trenutniVpis.vpisnaStevilka, Convert.ToInt32(i.id));
                if (prijave0.Count() == 0  && !ocena)
                {
                    izvajanjafinal.Add(i);
                }
            }
            //dodamo se izvajanja iz prejsnjega letnika, kjer ni nardil
            //kao vpis brez par predmetov
            if (trenutniVpis.letnikStudija > 1) {
                vpi prvivletnik = sh.prviVpisVLetnikN(trenutniVpis.vpisnaStevilka, trenutniVpis.letnikStudija - 1);
                List<izvajanje> izvajanjaodprej = prvivletnik.izvajanjes.ToList();
                foreach (var i in izvajanjaodprej)
                {
                    var prijave0 = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                .Where(a => a.izpitnirok.izvajanjeId == i.id)
                                                .Where(a => a.stanje == 0)
                                                .Where(a => a.izpitnirok.fiktiven == false)
                                                .ToList();
                    bool ocena = sh.pozitivnaOcena(trenutniVpis.vpisnaStevilka, Convert.ToInt32(i.id));
                    if (prijave0.Count() == 0 && !ocena)
                    {
                        izvajanjafinal.Add(i);
                    }
                }
            }


            Debug.WriteLine("Stevilo izvajanj: " + izvajanjafinal.Count);    
            return IzvajanaToSeznam(izvajanjafinal);
        }

        [Authorize(Roles = "Študent")]
        private List<SelectListItem> GetIzvajanaForStudentOdjava()
        {
            var student = UserHelper.GetStudentByUserName(User.Identity.Name);
            //var izvajanja = student.vpis.LastOrDefault().izvajanjes.ToList();
            StudentHelper sh = new StudentHelper();
            var trenutniVpis = sh.trenutniVpis(student.vpisnaStevilka);
            var prvivpis = trenutniVpis;
            if (trenutniVpis.vrstaVpisa != 1)
            {
                prvivpis = sh.prviVpisVLetnik(trenutniVpis.id); //ce je ponavljanje ali pavziranje damo na prvi vpis v ta letnik
            }
            List<izvajanje> izvajanja = null;
            if (trenutniVpis != null)
            {
                Debug.WriteLine("Vpis id je " + prvivpis.id.ToString());
                izvajanja = prvivpis.izvajanjes.ToList();
            }
            else
            {
                Debug.WriteLine("Trenutni vpis je null.");
                izvajanja = new List<izvajanje>();
            }
            //pustimo na seznamu le ce ima prijavo z stanjem 0
            List<izvajanje> izvajanjafinal = new List<izvajanje>();
            foreach (var i in izvajanja)
            {
                var prijave0 = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                .Where(a => a.izpitnirok.izvajanjeId == i.id)
                                                .Where(a => a.izpitnirok.fiktiven == false)
                                                .Where(a => a.stanje == 0);
                bool ocena = sh.pozitivnaOcena(trenutniVpis.vpisnaStevilka, Convert.ToInt32(i.id));
                if (prijave0.Count() > 0 && !ocena)
                {
                    izvajanjafinal.Add(i);
                }
            }
            //dodamo se izvajanja iz prejsnjega letnika, kjer ni nardil
            //kao vpis brez par predmetov
            if (trenutniVpis.letnikStudija > 1)
            {
                vpi prvivletnik = sh.prviVpisVLetnikN(trenutniVpis.vpisnaStevilka, trenutniVpis.letnikStudija - 1);
                List<izvajanje> izvajanjaodprej = prvivletnik.izvajanjes.ToList();
                foreach (var i in izvajanjaodprej)
                {
                    var prijave0 = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                .Where(a => a.izpitnirok.izvajanjeId == i.id)
                                                .Where(a => a.stanje == 0)
                                                .Where(a => a.izpitnirok.fiktiven == false)
                                                .ToList();
                    bool ocena = sh.pozitivnaOcena(trenutniVpis.vpisnaStevilka, Convert.ToInt32(i.id));
                    if (prijave0.Count() > 0 && !ocena)
                    {
                        izvajanjafinal.Add(i);
                    }
                }
            }

            Debug.WriteLine("Stevilo izvajanj: " + izvajanjafinal.Count);
            return IzvajanaToSeznam(izvajanjafinal);
        }

        [Authorize(Roles = "Referent")]
        private List<SelectListItem> GetIzvajanaForStudentVpisnaPrijava(int vpisna)
        {
            var student = UserHelper.GetStudentByVpisna(vpisna);
            //var izvajanja = student.vpis.LastOrDefault().izvajanjes.ToList();
            StudentHelper sh = new StudentHelper();
            var trenutniVpis = sh.trenutniVpis(student.vpisnaStevilka);
            var prvivpis = trenutniVpis;
            Debug.WriteLine("Trenutni vpis id je "+trenutniVpis.id.ToString());
            if (trenutniVpis.vrstaVpisa != 1)
            {
                Debug.WriteLine("Inside");
                prvivpis = sh.prviVpisVLetnik(trenutniVpis.id); //ce je ponavljanje ali pavziranje damo na prvi vpis v ta letnik
                //Debug.WriteLine("Trenutni vpis id je " + trenutniVpis.id.ToString());
            }
            List<izvajanje> izvajanja = null;
            if (prvivpis != null)
            {
                Debug.WriteLine("Prvi Vpis id je " + prvivpis.id.ToString());
                izvajanja = prvivpis.izvajanjes.ToList();
            }
            else
            {
                Debug.WriteLine("Trenutni vpis je null.");
                izvajanja = new List<izvajanje>();
            }
            //ce je ze prijavljen na kak izpit odstrani
            //prav tako ce ze ima pozitivno/zakljuceno oceno
            //uporabimo trenutni vpis ekr se na tega veze prijava
            List<izvajanje> izvajanjafinal = new List<izvajanje>();
            foreach (var i in izvajanja)
            {
                var prijave0 = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                .Where(a => a.izpitnirok.izvajanjeId == i.id)
                                                .Where(a => a.stanje == 0)
                                                .Where(a => a.izpitnirok.fiktiven == false)
                                                .ToList();
                bool ocena = sh.pozitivnaOcena(trenutniVpis.vpisnaStevilka, Convert.ToInt32(i.id));
                if (prijave0.Count() == 0 && !ocena)
                {
                    izvajanjafinal.Add(i);
                }
            }
            //dodamo se izvajanja iz prejsnjega letnika, kjer ni nardil
            //kao vpis brez par predmetov
            if (trenutniVpis.letnikStudija > 1)
            {
                vpi prvivletnik = sh.prviVpisVLetnikN(trenutniVpis.vpisnaStevilka, trenutniVpis.letnikStudija - 1);
                List<izvajanje> izvajanjaodprej = prvivletnik.izvajanjes.ToList();
                foreach (var i in izvajanjaodprej)
                {
                    var prijave0 = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                .Where(a => a.izpitnirok.izvajanjeId == i.id)
                                                .Where(a => a.stanje == 0)
                                                .Where(a => a.izpitnirok.fiktiven == false)
                                                .ToList();
                    bool ocena = sh.pozitivnaOcena(trenutniVpis.vpisnaStevilka, Convert.ToInt32(i.id));
                    if (prijave0.Count() == 0 && !ocena)
                    {
                        izvajanjafinal.Add(i);
                    }
                }
            }


            Debug.WriteLine("Stevilo izvajanj: " + izvajanjafinal.Count);
            return IzvajanaToSeznam(izvajanjafinal);
        }

        private List<SelectListItem> GetIzvajanaForStudentVpisnaOdjava(int vpisna)
        {
            var student = UserHelper.GetStudentByVpisna(vpisna);
            //var izvajanja = student.vpis.LastOrDefault().izvajanjes.ToList();
            StudentHelper sh = new StudentHelper();
            var trenutniVpis = sh.trenutniVpis(student.vpisnaStevilka);
            var prvivpis = trenutniVpis;
            if (trenutniVpis.vrstaVpisa != 1)
            {
                prvivpis = sh.prviVpisVLetnik(trenutniVpis.id); //ce je ponavljanje ali pavziranje damo na prvi vpis v ta letnik
            }
            List<izvajanje> izvajanja = null;
            if (trenutniVpis != null)
            {
                Debug.WriteLine("Vpis id je " + prvivpis.id.ToString());
                izvajanja = prvivpis.izvajanjes.ToList();
            }
            else
            {
                Debug.WriteLine("Trenutni vpis je null.");
                izvajanja = new List<izvajanje>();
            }
            //pustimo na seznamu le ce ima prijavo z stanjem 0
            List<izvajanje> izvajanjafinal = new List<izvajanje>();
            foreach (var i in izvajanja)
            {
                var prijave0 = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                .Where(a => a.izpitnirok.izvajanjeId == i.id)
                                                .Where(a => a.izpitnirok.fiktiven == false)
                                                .Where(a => a.stanje == 0);
                bool ocena = sh.pozitivnaOcena(trenutniVpis.vpisnaStevilka, Convert.ToInt32(i.id));
                if (prijave0.Count() > 0 && !ocena)
                {
                    izvajanjafinal.Add(i);
                }
            }
            //dodamo se izvajanja iz prejsnjega letnika, kjer ni nardil
            //kao vpis brez par predmetov
            if (trenutniVpis.letnikStudija > 1)
            {
                vpi prvivletnik = sh.prviVpisVLetnikN(trenutniVpis.vpisnaStevilka, trenutniVpis.letnikStudija - 1);
                List<izvajanje> izvajanjaodprej = prvivletnik.izvajanjes.ToList();
                foreach (var i in izvajanjaodprej)
                {
                    var prijave0 = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                .Where(a => a.izpitnirok.izvajanjeId == i.id)
                                                .Where(a => a.stanje == 0)
                                                .Where(a => a.izpitnirok.fiktiven == false)
                                                .ToList();
                    bool ocena = sh.pozitivnaOcena(trenutniVpis.vpisnaStevilka, Convert.ToInt32(i.id));
                    if (prijave0.Count() > 0 && !ocena)
                    {
                        izvajanjafinal.Add(i);
                    }
                }
            }

            Debug.WriteLine("Stevilo izvajanj: " + izvajanjafinal.Count);
            return IzvajanaToSeznam(izvajanjafinal);
        }

        private List<SelectListItem> IzvajanaToSeznam(List<izvajanje> izvajanja)
        {
            var seznamIzvajanja = new List<SelectListItem>();
            int c = 0;
            foreach (izvajanje i in izvajanja)
            {
                c++;
                string profesorji = "";
                var profesor1 = i.profesor;
                profesorji += profesor1.ime + " " + profesor1.priimek;
                if (i.profesor1 != null)
                {
                    var profesor2 = i.profesor1;
                    profesorji += ", " + profesor2.ime + " " + profesor2.priimek;
                }
                if (i.profesor2 != null)
                {
                    var profesor3 = i.profesor2;
                    profesorji += ", " + profesor3.ime + " " + profesor3.priimek;
                }
                profesorji = i.predmet.ime + " (" + i.predmet.koda + ")  -  " + profesorji;
                //izvajanje.izvajanjeleto.sifrant_studijskoleto.naziv
                //profesorji = profesorji + " (" + i.sifrant_studijskoleto.naziv + ")";

                seznamIzvajanja.Add(new SelectListItem() { Value = i.id.ToString(), Text = profesorji });
            }
            if (c < 1)
            {
                seznamIzvajanja.Add(new SelectListItem() { Value = "", Text = "Ni predmetov." });
            }
            else
            {
                seznamIzvajanja.Insert(0, new SelectListItem() { Value = "", Text = "Izberi" });
            }
            return seznamIzvajanja;
        }

        public string GetIzvajanjaForStudent(int id)
        {
            int vpisna;
            if (User.IsInRole("Študent"))
            {
                //student = UserHelper.GetStudentByUserName(User.Identity.Name);
                vpisna = UserHelper.GetStudentByUserName(User.Identity.Name).vpisnaStevilka;
            }
            else
            {
                //student = UserHelper.GetStudentByVpisna(id);
                vpisna = UserHelper.GetStudentByVpisna(id).vpisnaStevilka;
            }
            StudentHelper shhlp = new StudentHelper();
            var vpi = shhlp.trenutniVpis(vpisna);
            List<izvajanje> izvajanja;
            if(vpi == null)
            {
                izvajanja = new List<izvajanje>();
                Debug.WriteLine("Trenutni vpis je null.");
            }
            else 
            {
                izvajanja = vpi.izvajanjes.ToList();
            }
            Debug.WriteLine("Stevilo izvajanj: " + izvajanja.Count);    

            return new JavaScriptSerializer().Serialize(IzvajanaToSeznam(izvajanja));
        }

        public string GetIzpitniRoksForIzvajanja(int id)
        {
            StudentHelper sh = new StudentHelper();
            int leto = sh.trenutnoSolskoLeto();

            Debug.WriteLine("ID " + id);
            int iid = Convert.ToInt32(id);
            Debug.WriteLine("ID " + iid);
            var izvajanje = db.izvajanjes.SingleOrDefault(i => i.id == iid);//db.predmets.SingleOrDefault(p => p.id == iid);
            var izpitniRoki = izvajanje.izpitniroks.Where(a => a.fiktiven == false)
                                                   //.Where(a => (a.datum.Year == leto && a.datum.Month > 9) || (a.datum.Year == leto + 1 && a.datum.Month < 10))
                                                   .Where( a => a.datum > DateTime.Now);
                  
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
            else
            {
                seznamIzpitniRoki.Insert(0, new SelectListItem() { Value = "", Text = "Izberi" });
            }
            return new JavaScriptSerializer().Serialize(seznamIzpitniRoki);
        }

        [HttpPost]
        public JsonResult GetPrijaveForStudent(int studentId, int izvajanjeId)
        {
            StudentHelper sh = new StudentHelper();
            int vpisna;
            if (User.IsInRole("Referent"))
            {
                vpisna = studentId;
            }
            else
            {
                vpisna = UserHelper.GetStudentByUserName(User.Identity.Name).vpisnaStevilka;
            }
            int vpisID = sh.trenutniVpis(vpisna).id;
            List<prijavanaizpit> prijave;
            if (izvajanjeId == 0)
            {
                prijave = db.prijavanaizpits.Where(a => a.vpisId == vpisID).Where(a => a.stanje == 0).ToList();
            }
            else
            {
                prijave = db.prijavanaizpits.Where(a => a.vpisId == vpisID)
                                            .Where(a => a.stanje == 0)
                                            .Where(a => a.izpitnirok.izvajanjeId == izvajanjeId)
                                            .Where(a => a.izpitnirok.fiktiven == false)
                                            .ToList();
            }

            var seznamIzpitniRoki = new List<SelectListItem>();
            int c = 0;
            foreach (prijavanaizpit i in prijave)
            {
                c++;
                string prostor = "";
                if (i.izpitnirok.sifrant_prostor != null)
                {
                    prostor = i.izpitnirok.sifrant_prostor.naziv;
                }
                string ura = "";
                if (i.izpitnirok.ura != null)
                {
                    ura = UserHelper.TimeToString((DateTime)i.izpitnirok.ura);
                }
                seznamIzpitniRoki.Add(new SelectListItem() { Value = i.id.ToString(), Text = UserHelper.DateToString(i.izpitnirok.datum) + " " + ura + " " + prostor });
            }
            if (c < 1)
            {
                seznamIzpitniRoki.Add(new SelectListItem() { Value = "", Text = "Ni prijav." });
            }
            else
            {
                seznamIzpitniRoki.Insert(0, new SelectListItem() { Value = "", Text = "Izberi" });
            }
            return Json(new { Prijave = new JavaScriptSerializer().Serialize(seznamIzpitniRoki) } );
        }

        //PREVERJANJA
        /*
        //-preveri cas min 23 ur pred dnem izpita
        //-max 3 polaganja letos
        //-max 6 polaganj vse skupaj (ponavljanje resetira)
        //-preveri ce prijva pri tem predmetu(izvajanju) ze obstaja
        //-preveri prijavo na rok na katerga je ze prijavljen
        //-preveri ce je izpit ze opravljen
        //-preveri da je od prijave minilo x dni
        //-preveri ce za prejsnjo prijavo ze obstaja ocena
        //-preveri ce mora student placati izpit (4+ redni, 1+ izredni)
        */
        [HttpPost]
        public JsonResult PreveriPrijavo(string vpisna, string izpitniRok)
        {
            if(vpisna == "") {
                vpisna = UserHelper.GetStudentByUserName(User.Identity.Name).vpisnaStevilka.ToString();
            }
            int ivpisna= Convert.ToInt32(vpisna);
            int iizpitniRok= Convert.ToInt32(izpitniRok);
            Debug.WriteLine("Vpisna: " + vpisna + ", IzpitniRokId: " + izpitniRok);
            izpitnirok iRok = db.izpitniroks.SingleOrDefault(a => a.id == iizpitniRok);
            student stud = db.students.SingleOrDefault(a => a.vpisnaStevilka == ivpisna);

            List<string> opozorila = new List<string>();
            
            StudentHelper sh = new StudentHelper();
            var trenutniVpis = sh.trenutniVpis(stud.vpisnaStevilka);

            //OPOZORILA
            //-preveri cas min 23 ur pred dnem izpita
            if (DateTime.Now > iRok.datum.Subtract(new TimeSpan(23, 0, 0)) )
            {
                opozorila.Add("Rok za prijavo je potekel.");
            }
            //-preveri da je od zadnje prijave minilo 7 dni
            var zadnjaPrijava = db.prijavanaizpits.Where(a => a.vpisId == trenutniVpis.id)
                                                  .Where(a => a.izpitnirok.izvajanjeId == iRok.izvajanjeId)
                                                  .Where(a => a.stanje != 1)
                                                  .Where(a => a.stanje != 4)
                                                  .ToList()
                                                  .LastOrDefault();

            
            if (zadnjaPrijava != null)
            {
                Debug.WriteLine("Zadnja prijava id " + zadnjaPrijava.id.ToString());
                Debug.WriteLine("Zadnja prijava datum polaganja " + zadnjaPrijava.izpitnirok.datum.ToString());
                
                TimeSpan razlika = DateTime.Now - zadnjaPrijava.izpitnirok.datum;
                
                Debug.WriteLine(razlika.ToString());
                if (razlika.Days < 7)
                {
                    opozorila.Add("Od zadnjega polaganja pri temu predmetu še ni minilo 7 dni." + "Datum zadnjega polaganja: " + UserHelper.DateToString(zadnjaPrijava.izpitnirok.datum));
                }
            }
            else
            {
                Debug.WriteLine("Zadnja prijava null");
            }

            //-max 3 polaganja letos
            int letos = sh.polaganjaLetos(trenutniVpis.id, (int) iRok.izvajanjeId);
            if (letos >= 3)
            {
                opozorila.Add("Preseženo število dovoljenih prijav za letos (3).");
            }
            //-max 6 polaganj vse skupaj (ponavljanje resetira)
            int skupaj = sh.polaganjaVsa(trenutniVpis.vpisnaStevilka, (int) iRok.izvajanjeId, trenutniVpis.studijskiProgram);
            if (skupaj >= 6)
            {
                opozorila.Add("Preseženo število dovoljenih prijav (6).");
            }
            //-preveri ce prijva pri tem predmetu(izvajanju) ze obstaja
            if (db.prijavanaizpits.Where(a => a.izpitnirokId == iRok.id).Where(a => a.vpisId == trenutniVpis.id).Where(a => a.stanje == 0).FirstOrDefault() != null)
            {
                opozorila.Add("Prijava pri tem predmetu že obstaja.");
            }
            //-preveri prijavo na rok na katerga je ze prijavljen |^
            //-preveri ce je izpit ze opravljen
            //-preveri ce za prejsnjo prijavo ze obstaja ocena

            //opozorila.Add("--1test server--");
            //opozorila.Add("--2test server--");

            //SAMO OBVSETILO
            //-preveri ce mora student placati izpit (4+ redni, 1+ izredni)
            string obvestilo = "";
            if (trenutniVpis.vrstaVpisa == 3 && opozorila.Count() == 0)
            {
                obvestilo = "Za ta izpit bo potrbeno plačati 140€.";
            }

            //preveri ce je ze 3 polaganj v prejsnjem letniku in nisi naredil
            int stpolaganj = sh.polaganjaVsa(trenutniVpis.vpisnaStevilka, Convert.ToInt32(iRok.izvajanjeId), trenutniVpis.studijskiProgram);
            
            if (stpolaganj >= 3 && trenutniVpis.vrstaVpisa != 3 && opozorila.Count() == 0)
            {
                obvestilo = "Za ta izpit bo potrbeno plačati 140€.";
            }

            return Json(new { Warnings = new JavaScriptSerializer().Serialize(opozorila), Notice = obvestilo });
        }


        [HttpPost]
        public JsonResult PreveriOdjavo(string vpisna, string prijavaId)
        {
            if (vpisna == "" || User.IsInRole("Študent"))
            {
                vpisna = UserHelper.GetStudentByUserName(User.Identity.Name).vpisnaStevilka.ToString();
            }
            int ivpisna = Convert.ToInt32(vpisna);
            int iprijavaId = Convert.ToInt32(prijavaId);
            Debug.WriteLine("Vpisna: " + vpisna + ", IzpitniRokId: " + iprijavaId);
            prijavanaizpit prijava = db.prijavanaizpits.SingleOrDefault(a => a.id == iprijavaId);
            izpitnirok iRok = prijava.izpitnirok;//db.izpitniroks.SingleOrDefault(a => a.id == iizpitniRok);
            student stud = db.students.SingleOrDefault(a => a.vpisnaStevilka == ivpisna);

            List<string> opozorila = new List<string>();

            StudentHelper sh = new StudentHelper();
            var trenutniVpis = sh.trenutniVpis(stud.vpisnaStevilka);

            if (iRok == null)
            {
                Debug.WriteLine("iRok je null");
            }
            if (iRok.datum == null)
            {
                Debug.WriteLine("iRok.datum je null");
            }

            //OPOZORILA
            //-preveri cas min 23 ur pred dnem izpita
            if (DateTime.Now > iRok.datum.Subtract(new TimeSpan(23, 0, 0)))
            {
                opozorila.Add("Rok za odjavo je potekel.");
            }

            List<string> napake = new List<string>();
            //-preveri ce je izpit ze opravljen
            //-preveri ce za prejsnjo prijavo ze obstaja ocena
            if (prijava.stanje == 2 )
            {
                napake.Add("Odjava ni več mogoča.");
            }

            //opozorila.Add("--1test server--");
            //opozorila.Add("--2test server--");

            //SAMO OBVSETILO
            //-preveri ce mora student placati izpit (4+ redni, 1+ izredni)
            string obvestilo = "";


            return Json(new { Warnings = new JavaScriptSerializer().Serialize(opozorila), Notice = obvestilo, Errors = napake });
        }
    }
}
