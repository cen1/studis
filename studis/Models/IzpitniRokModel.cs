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

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        public int dan { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        public int mesec { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        public int leto { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        public int ura { get; set; }

        [Required(ErrorMessage = "Obvezno izpolniti.")]
        public int min { get; set; }
        
        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Predmet")]
        public int predmet { get; set; }
        
        
        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Izvaja")]
        public int izvajanje { get; set; }
        

    }
}