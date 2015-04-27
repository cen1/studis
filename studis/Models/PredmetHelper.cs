using System;
using System.Linq;
using studis.Models;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Collections.Generic;

namespace studis.Models
{
    public class PredmetHelper
    {
        public static studisEntities db = new studisEntities();

        public static IQueryable<predmet> obvezni1()
        {
            return db.predmets.Where(a => a.letnik == 1).Where(b => b.obvezen == true);
        }

        public static IQueryable<predmet> obvezni2()
        {
            return db.predmets.Where(a => a.letnik == 2).Where(b => b.obvezen == true);
        }

        public static IQueryable<predmet> obvezni3()
        {
            return db.predmets.Where(a => a.letnik == 3).Where(b => b.obvezen == true);
        }

        public static IQueryable<predmet> prostoizbirni2()
        {
            return db.predmets.Where(a => a.letnik == 2).Where(b => b.prostoizbirni == true);
        }

        public static IQueryable<predmet> strokovnoizbirni2()
        {
            return db.predmets.Where(a => a.letnik == 2).Where(b => b.strokovnoizbirni == true);
        }

        public static IQueryable<predmet> izbirni3()
        {
            return db.predmets.Where(a => a.letnik == 3).Where(b => b.obvezen == false);
        }

        public static bool preveriIzbirne3(List<modul> moduli, List<predmet> predmeti)
        {
            foreach (var m in moduli)
            {
                foreach (var p in m.predmets)
                {
                    foreach (var pr in predmeti)
                        if (pr.id == p.id) return false; //izbirni predmet je tudi v izbranem modulu, napaka
                }
            }
            return true;
        }

        public static bool preveriKredite(List<predmet> predmeti) {
            int sum=0;
            foreach (var p in predmeti)
                sum+=p.kreditne;

            if (sum != 60) return false;
            else return true;
        }

        public static bool preveriKredite(List<modul> moduli, List<predmet> predmeti)
        {
            int sum = 0;
            foreach (var p in predmeti)
                sum += p.kreditne;

            foreach (var m in moduli)
            {
                foreach (var p in m.predmets)
                {
                    sum += p.kreditne;
                }
            }

            if (sum != 60) return false;
            else return true;
        }
    }
}