using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace studis.Models
{
    public class VpisniList
    {
        public int vpisnaStevilka { get; set; }
        public string ime { get; set; }
        public string priimek { get; set; }
        public DateTime datumRojstva { get; set; }
        public string krajRojstva { get; set; }
        public string obcinaRojstva { get; set; }
        public string drzavaRojstva { get; set; }
        public string drzavljanstvo { get; set; }
        public char spol { get; set; }
        public long emso { get; set; }
        public int davcnaStevilka { get; set; }
        public string email { get; set; }
        public string prenosniTelefon { get; set; }
        
        //prebivalisce
        public string naslov { get; set; }
        public bool vrocanje { get; set; }
        public int postnaStevilka { get; set; }
        public string obcina { get; set; }
        public string drzava { get; set; }
        //zacasni naslov
        public string naslovZacasni { get; set; }
        public bool vrocanjeZacasni { get; set; }
        public int postnaStevilkaZacasni { get; set; }
        public string obcinaZacasni { get; set; }
        public string drzavaZacasni { get; set; }

        //studij
        public int studijskiProgram { get; set; }
        public int smer { get; set; }
        public string krajIzvajanja { get; set; }
        public string izbirnaSkupina { get; set; }
        public int studijskiProgram2 { get; set; }
        public int smer2 { get; set; }
        public string krajIzvajanja2 { get; set; }
        public string izbirnaSkupina2 { get; set; }
        public int vrtsaStudija { get; set; }
        public int vrstaVpisa { get; set; }
        public int letnikStudija { get; set; }
        public int nacinStudija { get; set; }
        public int oblikaStudija { get; set; }
        public string studijskoLetoPrvegaVpisa { get; set; }
        public bool soglasje1 { get; set; }
        public bool soglasje2 { get; set; }
        
    }
}