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

        // vrne končni seštevek polaganj za neko izvajanje
        public int polaganjaVsa(int vpisna, int izvajanjeId, int studijskiprogram)
        {
            int sum = 0;
            bool reset = false;
            student s = db.students.Find(vpisna);

            var vpisi = s.vpis.Where(a => a.studijskiProgram == studijskiprogram);
            
            if(vpisi != null)foreach (var v in vpisi)
            {
                foreach (var p in v.prijavanaizpits.Where(p => p.stanje == 2))
                {
                    if (p.izpitnirok.izvajanje.id == izvajanjeId)
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

        //vrne oceno zadnjega izpita po datumu
        public int zadnjaOcena(int vpisna, int izvajanjeId)
        {
            student s = db.students.Find(vpisna);
            int o = -1;
            DateTime dmax = DateTime.MinValue;

            foreach (var v in s.vpis)
            {
                prijavanaizpit pni = db.prijavanaizpits.Where(a => a.vpisId == v.id).Where(b => b.izpitnirok.izvajanjeId == izvajanjeId).ToList().LastOrDefault();
                if (pni != null)
                {
                    if (pni.izpitnirok.datum >= dmax)
                    {
                        dmax = pni.izpitnirok.datum;

                        var oc = pni.ocenas.LastOrDefault();
                        if (oc != null)
                        {
                            o = oc.ocena1;
                        }
                    }
                }
            }
            return o;
        }

        public int ocenaRoka(int vpisId, int rokId)
        {
            System.Diagnostics.Debug.WriteLine("Vpis "+vpisId.ToString()+" "+rokId.ToString());
            var prijava = db.prijavanaizpits.Where(a => a.vpisId == vpisId).Where(b => b.izpitnirokId == rokId).Where(c => c.stanje == 2).ToList().LastOrDefault();
            if (prijava != null)
            {
                var ocena = prijava.ocenas.ToList().LastOrDefault();
                if (ocena == null)
                {
                    System.Diagnostics.Debug.WriteLine("Ocena null");
                    return -1;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Ocena obstaja");
                    return ocena.ocena1;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Prijava null");
                return -1;                
            }
        }

        // vrne zaporedno stevilko izpita
        public int zaporednoPolaganje(int vpisna, int izvajanjeId, int studijskiprogram, DateTime datum)
        {
            int sum = 0;
            bool reset = false;
            student s = db.students.Find(vpisna);

            foreach (var v in s.vpis.Where(a => a.studijskiProgram == studijskiprogram))
            {
                foreach (var p in v.prijavanaizpits)
                {
                    if (p.izpitnirok.izvajanje.id == izvajanjeId && p.izpitnirok.datum <= datum && p.stanje != 1 && p.stanje != 4)
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

        public int pridobiTocke(int prijavaId)
        {
            var tocke = db.tockes.Where(o => o.prijavaId == prijavaId).FirstOrDefault();

            if (tocke != null)
            {
                return tocke.tocke1;
            }
            else
            {
                return 0;
            }
        }

        public string pridobiDatum(int prijavaId)
        {
            var datum = db.ocenas.Where(o => o.prijavaId == prijavaId).FirstOrDefault();

            if (datum != null)
            {
                return datum.datum.ToString("dd.MM.yyyy");
            }
            else
            {
                return "/";
            }
        }

        public int pridobiPrijavaId(int vpisna, int rokId)
        {

            var vpisi = db.vpis.Where(v => v.vpisnaStevilka == vpisna).ToList();

            foreach (var v in vpisi)
            {
                foreach (var p in v.prijavanaizpits)
                {
                    if (p.izpitnirokId == rokId)
                    {
                        return Convert.ToInt32(p.id);
                    }
                }
            }

            return 0;
        }

        public int pridobiVpisIzIzvajanja(int izvajanjeId, int vpisnaStevilka)
        {
            var izvajanje = db.izvajanjes.Where(i => i.id == izvajanjeId);
            var vpis = izvajanje.FirstOrDefault().vpis.Where(v => v.vpisnaStevilka == vpisnaStevilka).FirstOrDefault();
            return vpis.id;
        }
        public vpi pridobiVpisIzIzvajanjaObj(int izvajanjeId, int vpisnaStevilka)
        {
            var izvajanje = db.izvajanjes.Where(i => i.id == izvajanjeId);
            var vpis = izvajanje.FirstOrDefault().vpis.Where(v => v.vpisnaStevilka == vpisnaStevilka).FirstOrDefault();
            return vpis;
        }
    }
}