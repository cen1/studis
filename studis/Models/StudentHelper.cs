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

        public StudentHelper()
        {
            db = new studisEntities();
        }

        // vrne končni seštevek polaganj za to leto
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

        // vrne zaporedno polaganje za to leto
        public int zaporednoPolaganjaLetos(int vpisId, int izvajanjeId, int rokId)
        {
            int sum = 0;
            vpi v = db.vpis.Find(vpisId);
            foreach (var p in v.prijavanaizpits.Where(a => a.izpitnirok.izvajanjeId == izvajanjeId))
            {
                if (p.stanje == 2 && p.izpitnirok.id <= rokId) sum++;
            }
            return sum;
        }

        // vrne končni seštevek polaganj
        public int polaganjaVsa(int vpisna, int predmetId, int studijskiprogram)
        {
            int sum = 0;
            bool reset = false;
            student s = db.students.Find(vpisna);

            foreach (var v in s.vpis.Where(a => a.studijskiProgram == studijskiprogram))
            {
                foreach (var p in v.prijavanaizpits.Where(p => p.stanje == 2))
                {
                    if (p.izpitnirok.izvajanje.predmetId == predmetId)
                    {
                        if (v.vrstaVpisa == 2 && reset==false) //ponavljanje
                        {
                            sum = 0;
                            reset = true;
                        }
                        sum++;
                    }
                }
            }
            return sum;
        }

        // vrne zaporedno stevilko izpita
        public int zaporednoPolaganje(int vpisna, int predmetId, int studijskiprogram, DateTime datum)
        {
            int sum = 0;
            bool reset = false;
            student s = db.students.Find(vpisna);

            foreach (var v in s.vpis.Where(a => a.studijskiProgram == studijskiprogram))
            {
                foreach (var p in v.prijavanaizpits)
                {
                    if (p.izpitnirok.izvajanje.predmetId == predmetId && p.izpitnirok.datum <= datum)
                    {
                        if (v.vrstaVpisa == 2 && reset == false) //ponavljanje
                        {
                            sum = 0;
                            reset = true;
                        }
                        sum++;
                    }
                }
            }
            return sum;
        }

        public vpi trenutniVpis(int vpisna)
        {
            int tr = this.trenutnoSolskoLeto();
            return db.vpis.Where(a => a.vpisnaStevilka == vpisna).Where(b => b.studijskoLeto == tr).FirstOrDefault();
        }

        public int trenutnoSolskoLeto()
        {
            DateTime d = DateTime.Now;
            if (d.Month < 10) return d.Year - 1;
            else return d.Year;
        }

        public int pridobiOceno(int prijavaId)
        {
            var ocena = db.ocenas.Where(o => o.prijavaId == prijavaId).FirstOrDefault();

            if (ocena != null)
            {
                return ocena.ocena1;
            }
            else
            {
                return 0;
            }       
        }

        public System.DateTime pridobiDatum(int prijavaId)
        {
            var datum = db.ocenas.Where(o => o.prijavaId == prijavaId).FirstOrDefault();

            return datum.datum;
        }
    }
}