using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace studis.Models
{
    public class PrijavaNaIzpitModel
    {
        [Display(Name = "Predmet")]
        public int student { get; set; }

        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Predmet")]
        public int izvajanje { get; set; }


        [Required(ErrorMessage = "Obvezno izbrati.")]
        [Display(Name = "Izpitni rok")]
        public int izpitniRok { get; set; }
    }
}