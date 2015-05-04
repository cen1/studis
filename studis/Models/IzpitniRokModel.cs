using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace studis.Models
{
    public class IzpitniRokModel
    {
        [Remote("PreveriIzpitniRok", "IzpitniRok", HttpMethod = "POST", ErrorMessage = "Za ta izpitni rok so že vnesene ocene, sprembe niso dovoljene.")]
        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Izpitni rok")]
        public int id { get; set; }

        [Remote("PreveriDatum", "IzpitniRok", HttpMethod = "POST", ErrorMessage = "Datum ne sme biti v preteklosti.")]
        [Required(ErrorMessage = "Obvezno izpolniti.")]
        [Display(Name = "Datum")]
        public string datum { get; set; }

        
        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Predmet")]
        public int predmet { get; set; }
        
        /*
        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Profesor")]
        public int profesor { get; set; }
         */

    }
}