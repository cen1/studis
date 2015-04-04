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
            if (file != null)
            {
                List<string> list = new List<string>();
                string name = "";
                string lname = "";
                string program = "";
                string mail = "";
                string username = "";
                int line = 1;
                int counter = 0;
                int counterAll = 0;

                var filename = Path.GetFileName(file.FileName);
                var directory = Server.MapPath("~/Content/");
                var path = Path.Combine(directory, filename);
                file.SaveAs(path);

                if (file.ContentLength > 0)
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        while (sr.Peek() != -1)
                        {
                            switch (line)
                            {
                                case 1:
                                    name = sr.ReadLine();
                                    line++;
                                    break;
                                case 2:
                                    lname = sr.ReadLine();
                                    line++;
                                    break;
                                case 3:
                                    program = sr.ReadLine();
                                    line++;
                                    break;
                                case 4:
                                    mail = sr.ReadLine();
                                    line++;
                                    counterAll++;

                                    if ((name.Length > 0) && (name.Length <= 30) && (lname.Length > 0) && (lname.Length <= 30) &&
                                        (program.Length > 0) && (program.Length <= 7) && (mail.Length > 0) && (mail.Length <= 60))
                                    {

                                        string[] tmp = mail.Split('@');
                                        username = tmp[0];

                                        string password = Guid.NewGuid().ToString().Substring(0, 8);

                                        // membership
                                        MembershipUser user = Membership.CreateUser(username, password, mail);
                                        MembershipUser myObject = Membership.GetUser(username);
                                        string userid = myObject.ProviderUserKey.ToString();

                                        // student
                                        student stud = new student();
                                        stud.ime = name;
                                        stud.priimek = lname;
                                        stud.userId = Convert.ToInt32(userid);

                                        db.students.Add(stud);
                                        db.SaveChanges();
                                            
                                        list.Add("Uporabniško ime: " + username);
                                        list.Add("Geslo: " + password);
                                        counter++;
                                    }

                                    break;
                                case 5:
                                    sr.ReadLine();
                                    line = 1;
                                    break;
                            }
                        }
                        list.Add("Dodanih " + counter + " od " + counterAll);
                    }
                }
                TempData["list"] = list;
                return RedirectToAction("Print");
            }
            else
                return RedirectToAction("Import");
        }

        public ActionResult Print()
        {
            ViewBag.List = TempData["list"] as List<string>;
            return View();
        }
    }
}