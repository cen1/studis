using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace studis.Models
{
    public class KoncnaOcenaModel
    {
        [Range(0, 10)]
        [Display(Name = "Ocena")]
        public int ocena { get; set; }

        [Display(Name = "Predmet")]
        public int predmet { get; set; }

        [Display(Name = "Izvajanje")]
        public int izvajanje { get; set; }

        [Display(Name = "Izpitni rok")]
        public int izpitnirok { get; set; }

        
    }
}