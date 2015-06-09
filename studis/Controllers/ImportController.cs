using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;

namespace studis.Controllers
{
    [Authorize(Roles = "Referent, Skrbnik")]
    public class ImportController : Controller
    {
        public studisEntities db = new studisEntities();

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            // no file?
            if (file != null)
            {
                List<string> name = new List<string>();
                List<string> lname = new List<string>();
                List<string> program = new List<string>();
                List<string> email = new List<string>();
                List<string> username = new List<string>();
                List<string> password = new List<string>();
                List<string> added = new List<string>();
                int i = 0;
                int counter = 0;
                int counterAll = 0;

                var filename = Path.GetFileName(file.FileName);
                var directory = Server.MapPath("~/Content/");
                var path = Path.Combine(directory, filename);
                file.SaveAs(path);

                // blank file?
                if (file.ContentLength > 0)
                {
                    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
                    {
                        while (sr.Peek() != -1)
                        {
                            var line = sr.ReadLine();
                            var length = line.Length;
                            var st = length / 127;

                            if ((length%127) == 0) {
                                while (st != 0) 
                                {
                                    var ime = line.Substring(i, 30).Trim();
                                    i = i + 30;
                                    var priimek = line.Substring(i, 30).Trim();
                                    i = i + 30;
                                    var prog = line.Substring(i, 7).Trim();
                                    i = i + 7;
                                    var mail = line.Substring(i, 60).Trim();
                                    i = i + 60;

                                    st--;

                                    string[] tmp = mail.Split('@');
                                    string uname = tmp[0];
                                    // string pass = Guid.NewGuid().ToString().Substring(0, 8);
                                    string pass = "testtest";
                                    
                                    try
                                    {
                                        var model = new studis.Models.KandidatModel
                                        {
                                            ime = ime,
                                            priimek = priimek,
                                            studijskiProgram = Convert.ToInt32(prog),
                                            email = mail
                                        };

                                        var context = new ValidationContext(model, null, null);
                                        var results = new List<ValidationResult>();

                                        // preveri če parametri ustrezajo modelu in če je šifrant pravilen
                                        if ((Validator.TryValidateObject(model, context, results, true)) && (Sifranti.STUDIJSKIPROGRAM.Single(item => item.id == Convert.ToInt32(prog)) != null)
                                            && (db.my_aspnet_membership.Count(e => e.Email == mail) == 0))
                                        {
                                            // dodaj login podatke in vlogo
                                            MembershipUser user = Membership.CreateUser(uname, pass, mail);
                                            MembershipUser myObject = Membership.GetUser(uname);
                                            Roles.AddUserToRole(uname, "Študent");
                                            string id = myObject.ProviderUserKey.ToString();

                                            // dodaj v kandidat
                                            kandidat k = new kandidat();

                                            k.ime = ime;
                                            k.priimek = priimek;
                                            k.studijskiProgram = Convert.ToInt32(prog);
                                            k.email = mail;
                                            k.userId = Convert.ToInt32(id);

                                            db.kandidats.Add(k);
                                            db.SaveChanges();

                                            // dodaj v list če je OK
                                            name.Add(ime);
                                            lname.Add(priimek);
                                            program.Add(prog);
                                            email.Add(mail);
                                            username.Add(uname);
                                            password.Add(pass);
                                            added.Add("DA");
                                            
                                            // povečaj število uspešno dodanih
                                            counter++;                                            
                                        }
                                        else
                                        {
                                            // dodaj v list če ni OK
                                            name.Add(ime);
                                            lname.Add(priimek);
                                            program.Add(prog);
                                            email.Add(mail);
                                            username.Add("/");
                                            password.Add("/");
                                            added.Add("NE");
                                        }
                                    }
                                    catch
                                    {
                                        // dodaj v list če ni OK, baza vrne exception
                                        name.Add(ime);
                                        lname.Add(priimek);
                                        program.Add(prog);
                                        email.Add(mail);
                                        username.Add("/");
                                        password.Add("/");
                                        added.Add("NE");
                                    }
                                    
                                    // povečaj število vseh dodanih
                                    counterAll++;
                                }
                            }
                            else
                            {
                                return RedirectToAction("ImportError");
                            }
                        }
                    }
                }

                TempData["Name"] = name;
                TempData["Lname"] = lname;
                TempData["Program"] = program;
                TempData["Email"] = email;
                TempData["Username"] = username;
                TempData["Password"] = password;
                TempData["Added"] = added;
                TempData["Counter"] = counter;
                TempData["CounterAll"] = counterAll;

                return RedirectToAction("Print");
            }
            else
                return RedirectToAction("Import");
        }

        public ActionResult Print()
        {
            ViewBag.Name = TempData["Name"];
            ViewBag.Lname = TempData["Lname"];
            ViewBag.Program = TempData["Program"];
            ViewBag.Email = TempData["Email"];
            ViewBag.Username = TempData["Username"];
            ViewBag.Password = TempData["Password"];
            ViewBag.Added = TempData["Added"];
            ViewBag.Counter = TempData["Counter"];
            ViewBag.CounterAll = TempData["CounterAll"];
            return View();
        }

        public ActionResult ImportError()
        {
            return View();
        }
    }
}