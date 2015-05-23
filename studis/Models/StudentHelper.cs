using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using studis.Models;

namespace studis.Models
{
    public class StudentHelper
    {

        private studisEntities db;

        public int polaganjaLetos(int vpisId, int izvajanjeId)
        {
            int sum = 0;
            vpi v = db.vpis.Find(vpisId);
            foreach (var p in v.prijavanaizpits.Where(a => a.izpitnirok.izvajanjeId==izvajanjeId))
            {
                if (p.stanje == 2) sum++;
            }
            return sum;
        }

        public int polaganjaVsa(int vpisna, int predmetId)
        {
            int sum = 0;
            vpi v = db.vpis.Find(vpisId);
            foreach (var p in v.prijavanaizpits.Where(a => a.izpitnirok.izvajanjeId == izvajanjeId))
            {
                if (p.stanje == 2) sum++;
            }
            return sum;
        }

        public vpi trenutniVpis(int vpisna)
        {
            return db.vpis.Where(a => a.vpisnaStevilka == vpisna).Where(b => b.studijskoLeto == this.trenutnoSolskoLeto()).FirstOrDefault();
        }

        public int trenutnoSolskoLeto()
        {
            DateTime d = DateTime.Now;
            if (d.Month < 10) return d.Year - 1;
            else return d.Year;
        }
    }

    

}