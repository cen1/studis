using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;
using System.Web.Security;

namespace studis.Controllers
{
    [Authorize(Roles = "Referent")]
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
                            try
                            {
                                while (true) 
                                {
                                    var ime = line.Substring(i, 30).Trim();
                                    i = i + 30;
                                    var priimek = line.Substring(i, 30).Trim();
                                    i = i + 30;
                                    var prog = line.Substring(i, 7).Trim();
                                    i = i + 7;
                                    var mail = line.Substring(i, 60).Trim();
                                    i = i + 60;

                                    string[] tmp = mail.Split('@');
                                    string uname = tmp[0];
                                    string pass = Guid.NewGuid().ToString().Substring(0, 8);
                                    /*
                                    try
                                    {
                                        // dodaj login podatke in vlogo
                                        MembershipUser user = Membership.CreateUser(uname, pass, mail);
                                        MembershipUser myObject = Membership.GetUser(uname);
                                        Roles.AddUserToRole(uname, "Študent");
                                        string userid = myObject.ProviderUserKey.ToString();

                                        // dodaj v kandidat
                                        kandidat k = new kandidat();
                                        k.ime = ime;
                                        k.priimek = priimek;
                                        k.program = Convert.ToInt32(prog);
                                        k.email = mail;
                                        k.sprejet = false;

                                        db.kandidats.Add(k);
                                        db.SaveChanges();

                                        name.Add(ime);
                                        lname.Add(priimek);
                                        program.Add(prog);
                                        email.Add(mail);
                                        username.Add(uname);
                                        password.Add(pass);
                                        added.Add("DA");

                                        counter++;

                                    }
                                    catch
                                    {
                                        name.Add(ime);
                                        lname.Add(priimek);
                                        program.Add(prog);
                                        email.Add(mail);
                                        username.Add("/");
                                        password.Add("/";
                                        added.Add("NE");
                                    }
                                    */

                                    counterAll++;

                                    // temp
                                    name.Add(ime);
                                    lname.Add(priimek);
                                    program.Add(prog);
                                    email.Add(mail);
                                    username.Add(uname);
                                    password.Add(pass);
                                    added.Add("DA");

                                    counter++;
                                    // end temp
                                }
                            }
                            catch
                            {
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
    }
}