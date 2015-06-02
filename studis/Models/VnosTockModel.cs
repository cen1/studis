using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Models
{
    public class VnosTockModel
    {
        /*
        //izpitni rok
        public int sifraPredmeta { get; set; }
        public string imePredmeta { get; set; }
        public string izpraševalci { get; set; }
        public string datumIzvajanja { get; set; }
        public string uraIzvajanja { get; set; }
        public string prostor { get; set; }*/
        public int idRoka { get; set; }
        
        //študenti
        public int zaporednaStevilka { get; set; }
        public int vpisnaStevilka { get; set; }
        public string priimek { get; set; }
        public string ime { get; set; }
        public string studijskoLeto { get; set; }
        public int zaporednoSteviloPonavljanja { get; set; }
    
        //vnos točk&ocen
        public string zeVpisaneTocke { get; set; }
        public string zeVpisanaOcena { get; set; }

        [RegularExpression(@"^(vp)|(VP)|0*(?:0|[1-9][0-9]?|1[0-9][0-9]?|200)$", ErrorMessage = "Vnos mora biti število med 0 in 200 ali VP!")]
        public string tocke { get; set; }

        [RegularExpression(@"^(vp)|(VP)|([0-9]|10)$", ErrorMessage = "Vnos mora biti število med 0 in 10 ali VP!")]
        public string ocena { get; set; }
    }
}