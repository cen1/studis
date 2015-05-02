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
        private studisEntities db;

        public PredmetHelper()
        {
            db = new studisEntities();
        }

        public IQueryable<predmet> obvezni1()
        {
            return db.predmets.Where(a => a.letnik == 1).Where(b => b.obvezen == true);
        }

        public IQueryable<predmet> obvezni2()
        {
            return db.predmets.Where(a => a.letnik == 2).Where(b => b.obvezen == true);
        }

        public IQueryable<predmet> obvezni3()
        {
            return db.predmets.Where(a => a.letnik == 3).Where(b => b.obvezen == true);
        }

        public IQueryable<predmet> prostoizbirni2()
        {
            return db.predmets.Where(a => a.letnik == 2).Where(b => b.prostoizbirni == true);
        }

        public IQueryable<predmet> strokovnoizbirni2()
        {
            return db.predmets.Where(a => a.letnik == 2).Where(b => b.strokovnoizbirni == true);
        }

        public IQueryable<predmet> izbirni3()
        {
            return db.predmets.Where(a => a.letnik == 3).Where(b => b.obvezen == false);
        }

        public bool preveriIzbirne3(List<modul> moduli, List<predmet> predmeti)
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

        public bool preveriKredite(List<predmet> predmeti) {
            int sum=0;
            foreach (var p in predmeti)
                sum+=p.kreditne;

            if (sum != 60) return false;
            else return true;
        }

        public bool preveriKredite(List<modul> moduli, List<predmet> predmeti)
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

        public int getKreditObv2()
        {
            int sumObv = 0;
            foreach (var pr in this.obvezni2())
                sumObv += pr.kreditne;

            return sumObv;
        }
    }
}