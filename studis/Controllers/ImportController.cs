using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            string name;
            string lname;
            string program;
            string mail;
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
                                line++;
                                break;
                            case 5:
                                sr.ReadLine();
                                line = 1;
                                break;
                        }

                        //students[counter] =
                        counter++;


                    }
                }
            }

            return RedirectToAction("Import", "Import");
        }
    }
}