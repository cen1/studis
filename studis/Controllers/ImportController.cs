using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using studis.Models;

namespace studis.Controllers
{
    public class ImportController : Controller
    {

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            List<string> list = new List<string>();
            string name = "";
            string lname = "";
            string program = "";
            string mail = "";
            int line = 1;
            int counter = 0;

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
                                list.Add(mail);
                                line++;

                                if ((name.Length > 0) && (name.Length <= 30) && (lname.Length > 0) && (lname.Length <= 30) &&
                                    (program.Length > 0) && (program.Length <= 7) && (mail.Length > 0) && (mail.Length <= 60))
                                {
                                    string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
                                    var random = new Random();
                                    var password = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());

                                    // add user with name,lname,program,mail,password

                                    list.Add(password);
                                    counter++;
                                }

                                break;
                            case 5:
                                sr.ReadLine();
                                line = 1;
                                break;
                        }
                    }
                }
            }
            TempData["list"] = list;
            return RedirectToAction("Print");
        }

        public ActionResult Print()
        {
            ViewBag.List = TempData["list"] as List<string>;
            return View();
        }
    }
}